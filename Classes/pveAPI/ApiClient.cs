using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;

namespace Proxmox_Desktop_Client.Classes.pveAPI
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        public DataTicket DataTicket;
        public readonly dataServerInfo DataServerInfo;
        
        
        public List<MachineData> Machines;

        public ApiClient(string server, string port, bool skipSsl)
        {
            try
            {
                // Storing Server Data
                DataServerInfo = new dataServerInfo();
                DataServerInfo.server = server;
                DataServerInfo.port = port;
                DataServerInfo.skipSSL = skipSsl;
                
                // Initialize HttpClient Handler
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                // If we are not checking for SSL do this...
                if(skipSsl)
                {
                    handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;    
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
                
                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(loginData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                // Attempt Ticket Request
                var response = _httpClient.PostAsync("access/ticket", httpContent).GetAwaiter().GetResult();
                Program.DebugPoint("Program Error (Login Request): " + Environment.NewLine + JsonConvert.SerializeObject(response));
                
                // Process If Successful
                if (response.IsSuccessStatusCode)
                {
                    // Decode Response
                    dynamic apiResponse = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                    
                    // Process Ticket 
                    DataTicket = new DataTicket
                    {
                        ticket = apiResponse!.data.ticket,
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
                        return TotpChallengeRequest(otp);
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
        private bool TotpChallengeRequest(string otpCode)
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
                
                Program.DebugPoint("Program Error (TOTPChallengeRequest): " + Environment.NewLine + JsonConvert.SerializeObject(response));
                
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dynamic apiResponse = JsonConvert.DeserializeObject(responseData);
                    
                    DataTicket = new DataTicket
                    {
                        ticket = apiResponse!.data.ticket,
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
        public void RenewTicket(object sender = null, EventArgs e = null)
        {
            try
            {
                Dictionary<string, string> ticketData = new Dictionary<string, string>();
                ticketData.Add("username", DataTicket.username);
                ticketData.Add("password", DataTicket.ticket);
                
                // Serialize the object to JSON
                var jsonContent = JsonConvert.SerializeObject(ticketData);
                
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync("access/ticket", httpContent).GetAwaiter().GetResult();
                
                Program.DebugPoint("Program Error (Renew Ticket): " + Environment.NewLine + JsonConvert.SerializeObject(response));
                
                if (response.IsSuccessStatusCode)
                {
                    string responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dynamic apiResponse = JsonConvert.DeserializeObject(responseData);
                    
                    DataTicket = new DataTicket
                    {
                        ticket = apiResponse!.data.ticket,
                        CSRFPreventionToken = apiResponse.data.CSRFPreventionToken,
                        username = apiResponse.data.username
                    };
                    
                    return;
                }

                MessageBox.Show("There were problem renewing your ticket.", "API Failure");
                return;
            } catch (Exception ex)
            {
                MessageBox.Show("There were problem renewing your ticket.", "Application Failure");
                Program.DebugPoint("Program Error (Renew Ticket): " + Environment.NewLine + ex.StackTrace);
                return;
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
                    Console.WriteLine($"Program Error (Get Request): {JsonConvert.SerializeObject(response)}");
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return "403";
                    }
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
                    Console.WriteLine($"Program Error (Post Request): {JsonConvert.SerializeObject(response)}");
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return "403";
                    }
                }
            }

            return null;
        }
        
    }
}