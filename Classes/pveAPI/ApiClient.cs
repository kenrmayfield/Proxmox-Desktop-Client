using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OtpNet;
using Proxmox_Desktop_Client.Classes.pveAPI.objects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Proxmox_Desktop_Client.Classes.pveAPI
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        public ApiTicket _apiTicket;
        public dataServerUser _ServerUserData;
        
        
        public List<MachineData> Machines;

        public ApiClient(string server, string port, bool skipSSL)
        {
            // Storing Server Data
            _ServerUserData = new dataServerUser();
            _ServerUserData.server = server;
            _ServerUserData.port = port;
            _ServerUserData.skipSSL = skipSSL;
            
            // Initialize HttpClient
            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            if(skipSSL)
            {
                handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;    
            }
            
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{server}:{port}/api2/json/")
            };
            
        }

        public async Task<List<RealmData>> GetRealmsAsync()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    var response = await _httpClient.GetAsync("access/domains", cts.Token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<RealmData>>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    return jsonData?["data"] ?? new List<RealmData>();
                }
                catch (TaskCanceledException)
                {
                    MessageBox.Show("Could not reach the Proxmox API.\n\rPlease check your settings and try again.", "Server Error");
                    return new List<RealmData>();
                }
                catch (Exception)
                {
                    MessageBox.Show("The server information provided doesn't lead to the Proxmox API.\n\rPlease check your settings and try again.", "Server Error");
                    return new List<RealmData>();
                }
            }
        }


        public async Task<bool> LoginAsync(string username, string password, string realm, string otp = null)
        {
        
            _ServerUserData.username = username;
            _ServerUserData.password = password;
            _ServerUserData.realm = realm;
            
            var content = new
            {
                username = username,
                password = password,
                realm = realm
            };
            
            var contentOTP = new
            {
                username = username,
                password = password,
                realm = realm,
                otp = otp
            };

            // Serialize the object to JSON
            var jsonContent = String.Empty;
            
            if (otp != null) { jsonContent = JsonConvert.SerializeObject(content); }
            else { jsonContent = JsonConvert.SerializeObject(contentOTP); }
            
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("access/ticket", httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                dynamic apiResponse = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                _apiTicket = new ApiTicket
                {
                    ticket = apiResponse.data.ticket,
                    CSRFPreventionToken = apiResponse.data.CSRFPreventionToken,
                    username = apiResponse.data.username
                };
                
                if (_apiTicket.ticket.Contains("PVE:!tfa!") && (otp == null || otp == String.Empty))
                {
                    MessageBox.Show("This account has TOTP enabled. You must enter your TOTP key.", "Login Failure");
                    return false;
                } else if (_apiTicket.ticket.Contains("PVE:!tfa!") && (otp != null || otp != String.Empty))
                {
                    return await LoginTotpAsync(_apiTicket, otp);
                }
                
                return true;
                
            }

            MessageBox.Show("There were problems logging in... Please try again.", "Login Failure");
            return false;
        }

        public async Task<bool> LoginTotpAsync(ApiTicket apiTicket, string otpCode)
        {

   
            var contentOtp = new Dictionary<string, string>();
            contentOtp.Add("username", _apiTicket.username);
            contentOtp.Add("password", "totp:" + otpCode);
            contentOtp.Add("tfa-challenge", _apiTicket.ticket);
            
            // Serialize the object to JSON
            var jsonContent = String.Empty;
            
            jsonContent = JsonConvert.SerializeObject(contentOtp);
            
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("access/ticket", httpContent);
            
            Console.WriteLine(JsonConvert.SerializeObject(response.Content));
            if (response.IsSuccessStatusCode)
            {
                dynamic apiResponse = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                _apiTicket = new ApiTicket
                {
                    ticket = apiResponse.data.ticket,
                    CSRFPreventionToken = apiResponse.data.CSRFPreventionToken,
                    username = apiResponse.data.username
                };
                
                return true;
                
            }

            MessageBox.Show("There were problems logging in... Please try again.", "Login Failure");
            return false;
        }

        public async Task GetPermissionsAsync()
        {
            
            using (var request = new HttpRequestMessage(HttpMethod.Get, "access/permissions"))
            {
                // Add the authentication cookie
                request.Headers.TryAddWithoutValidation("Cookie", $"PVEAuthCookie={_apiTicket.ticket}");

                // Set the Accept header to indicate that we expect JSON
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    UserPermissions userPermissions = new UserPermissions(content);
                    Program._Permissions = userPermissions;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
        
        }
        
        public async Task<string> GetAsync(string path)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{path}"))
            {
                // Add the authentication cookie
                request.Headers.TryAddWithoutValidation("Cookie", $"PVEAuthCookie={_apiTicket.ticket}");

                // Set the Accept header to indicate that we expect JSON
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }

            return null;
        }
        
        public async Task<string> PostAsync(string path, Dictionary<string, string> postData)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{path}"))
            {
                // Add the authentication cookie
                request.Headers.TryAddWithoutValidation("Cookie", $"PVEAuthCookie={_apiTicket.ticket}");
                request.Headers.TryAddWithoutValidation("CSRFPreventionToken", $"{_apiTicket.CSRFPreventionToken}");
                
                // Set the Accept header to indicate that we expect JSON
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Add POST data
                if (postData != null)
                {
                    request.Content = new FormUrlEncodedContent(postData);
                }

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }

            return null;
        }
        
    }
}