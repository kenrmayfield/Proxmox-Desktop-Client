using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Proxmox_Desktop_Client.Classes.pveAPI
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        public dataTicket DataTicket;
        public dataServerInfo DataServerInfo;
        
        
        public List<MachineData> Machines;

        public ApiClient(string server, string port, bool skipSSL)
        {
            try
            {
                // Storing Server Data
                DataServerInfo = new dataServerInfo();
                DataServerInfo.server = server;
                DataServerInfo.port = port;
                DataServerInfo.skipSSL = skipSSL;
                
                // Initialize HttpClient Handler
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                // If we are not checking for SSL do this...
                if(skipSSL)
                {
                    handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;    
                }
                // Initialize HttpClient
                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri($"https://{server}:{port}/api2/json/")
                };
            } 
            catch (Exception ex)
            {
                Program.DebugPoint("Program Error (ApiClient): " + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("Something went wrong building the Client connection.", "Application Error");
            }
        }
        public List<RealmData> GetRealms()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    // Attempt to collect realm data
                    var response = _httpClient.GetAsync("access/domains", cts.Token).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    
                    // Serialize Data and provide to ClientLogin
                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<RealmData>>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    return jsonData?["data"] ?? new List<RealmData>();
                }
                catch (Exception ex)
                {
                    Program.DebugPoint("Server Error (GetRealmsAsync): " + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show("The server information provided doesn't lead to the Proxmox API.\n\rPlease check your settings and try again.", "Server Error");
                    return new List<RealmData>();
                }
            }
        }
        public bool LoginRequest(string username, string password, string realm, string otp = null)
        {
            try
            {
                DataServerInfo.username = username;
                DataServerInfo.password = password;
                DataServerInfo.realm = realm;
                
                Dictionary<string, string> loginData = new Dictionary<string, string>();
                
                loginData.Add("username", username);
                loginData.Add("password", password);
                loginData.Add("realm", realm);
                
                if (otp != null)
                {
                    loginData.Add("otp", otp);
                }

                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(loginData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                // Attempt Ticket Request
                var response = _httpClient.PostAsync("access/ticket", httpContent).GetAwaiter().GetResult();
                
                // Process If Successful
                if (response.IsSuccessStatusCode)
                {
                    // Decode Response
                    dynamic apiResponse = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    
                    // Process Ticket 
                    DataTicket = new dataTicket
                    {
                        ticket = apiResponse.data.ticket,
                        CSRFPreventionToken = apiResponse.data.CSRFPreventionToken,
                        username = apiResponse.data.username
                    };
                    
                    // Does Ticket State TOTP Required
                    if (DataTicket.ticket.Contains("PVE:!tfa!") && string.IsNullOrEmpty(otp))
                    {
                        MessageBox.Show("This account has TOTP enabled. You must enter your TOTP key.", "Login Failure");
                        return false;
                    } 
                    // Process TOTP Authentication
                    if (DataTicket.ticket.Contains("PVE:!tfa!"))
                    {
                        return TOTPChallengeRequest(DataTicket, otp);
                    }
                    
                    // Return True
                    return true;
                }
                
                // Process If Unsuccessful
                MessageBox.Show("There were problems logging in... Please try again.", "Login Failure");
                return false;
                
            } catch (Exception ex)
            {
                MessageBox.Show("There were problems logging in... likely to a software error.", "Application Failure");
                Program.DebugPoint("Program Error: " + Environment.NewLine + ex.StackTrace);
                return false;
            }

        }
        private bool TOTPChallengeRequest(dataTicket dataTicket, string otpCode)
        {
            try
            {
                Dictionary<string, string> otpChallenge = new Dictionary<string, string>();
                otpChallenge.Add("username", DataTicket.username);
                otpChallenge.Add("password", "totp:" + otpCode);
                otpChallenge.Add("tfa-challenge", DataTicket.ticket);
                
                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(otpChallenge);
                
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync("access/ticket", httpContent).GetAwaiter().GetResult();
                
                Console.WriteLine(JsonConvert.SerializeObject(response.Content));
                if (response.IsSuccessStatusCode)
                {
                    dynamic apiResponse = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    DataTicket = new dataTicket
                    {
                        ticket = apiResponse.data.ticket,
                        CSRFPreventionToken = apiResponse.data.CSRFPreventionToken,
                        username = apiResponse.data.username
                    };
                    
                    return true;
                }

                MessageBox.Show("There were problems logging in... Please try again.", "Login Failure");
                return false;
            } catch (Exception ex)
            {
                MessageBox.Show("There were problems logging in... likely to a software error.", "Application Failure");
                Program.DebugPoint("Program Error (TOTPChallengeRequest): " + Environment.NewLine + ex.StackTrace);
                return false;
            }
            
        }
        
         public string GetRequest(string path)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{path}"))
            {
                // Add the authentication cookie
                request.Headers.TryAddWithoutValidation("Cookie", $"PVEAuthCookie={DataTicket.ticket}");

                // Set the Accept header to indicate that we expect JSON
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return content;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                }
            }

            return null;
        }
        
        public string PostRequest(string path, Dictionary<string, string> postData)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{path}"))
            {
                // Add the authentication cookie
                request.Headers.TryAddWithoutValidation("Cookie", $"PVEAuthCookie={DataTicket.ticket}");
                request.Headers.TryAddWithoutValidation("CSRFPreventionToken", $"{DataTicket.CSRFPreventionToken}");
                
                // Set the Accept header to indicate that we expect JSON
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add POST data
                if (postData != null)
                {
                    request.Content = new FormUrlEncodedContent(postData);
                }

                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return content;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                }
            }

            return null;
        }
        
    }
}