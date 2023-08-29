

//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Genrate_Token;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Titanium.Web.Proxy;
//using Titanium.Web.Proxy.EventArguments;
//using Titanium.Web.Proxy.Models;

//namespace Genrate_Token
//{
//    class Program
//    {

//        static async Task Main(string[] args)
//        {
//            var configuration = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.json")
//                .Build();

//            var appSettings = configuration.Get<AppSetting>();

//            var services = new ServiceCollection();
//            ConfigureServices(services, appSettings);

//            var serviceProvider = services.BuildServiceProvider();
//            var apiEndpoint = "/api/User";
//            var proxyServer = new ProxyServer();
//            proxyServer.BeforeRequest += OnBeforeRequest;
//            proxyServer.Start();

//            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
//            var httpClient = httpClientFactory.CreateClient();

//            var randomTokenIndex = new Random().Next(0, appSettings.AuthTokens.Count);
//            var authToken = appSettings.AuthTokens[randomTokenIndex];

//            for (int i = 0; i < appSettings.NumberOfRequests; i++)
//            {
//                var fakeIpAddress = GenerateFakeIpAddress();

//                httpClient.DefaultRequestHeaders.Clear();
//                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
//                httpClient.DefaultRequestHeaders.Add("X-Forwarded-For", fakeIpAddress);

//                try
//                {
//                    // var response = await httpClient.GetAsync($"{appSettings.ApiBaseUrl}/UserController");
//                    var response = await httpClient.GetAsync($"{appSettings.ApiBaseUrl}{apiEndpoint}");
//                    Console.WriteLine($"Request {i + 1}: Token: {authToken},\n\n IP Address: {fakeIpAddress}");
//                    Console.WriteLine("\n\nResponse  : \n\n " + await response.Content.ReadAsStringAsync());
//                    Console.WriteLine("\n..............................\n\n");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Request {i + 1} failed: {ex.Message}");
//                    Console.WriteLine("\n..............................\n\n");
//                }
//            }


//            proxyServer.Stop();
//        }



//        private static async Task OnBeforeRequest(object sender, SessionEventArgs e)
//        {
//            var fakeIpAddress = GenerateFakeIpAddress();

//            // Change the IP address in the "X-Forwarded-For" header
//            e.WebSession.Request.Headers.AddHeader("X-Forwarded-For", fakeIpAddress);
//        }

//        private static string GenerateFakeIpAddress()
//        {
//            // Replace this logic with your actual fake IP address generation logic
//            Random random = new Random();
//            return $"{random.Next(1, 256)}.{random.Next(1, 256)}.{random.Next(1, 256)}.{random.Next(1, 256)}";
//        }
//        static void ConfigureServices(IServiceCollection services, AppSetting appSettings)
//        {
//            services.AddHttpClient();

//            services.AddOptions<AppSetting>().Configure(options =>
//            {
//                options.ApiBaseUrl = appSettings.ApiBaseUrl;
//                options.NumberOfRequests = appSettings.NumberOfRequests;
//                options.AuthTokens = appSettings.AuthTokens;
//            });
//        }
//    }

//    public class AppSetting
//    {
//        public string ApiBaseUrl { get; set; }
//        public int NumberOfRequests { get; set; }
//        public List<string> AuthTokens { get; set; }
//    }
//}






///////////////////////////////////////////////////////////////////
///

//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;

//namespace Genrate_Token
//{
//    class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var configuration = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.json")
//                .Build();

//            var appSettings = configuration.Get<AppSetting>();
//            var maliciousPayloads = appSettings.MaliciousPayloads; // Retrieve the list of malicious payloads

//            var services = new ServiceCollection();
//            ConfigureServices(services, appSettings);

//            var serviceProvider = services.BuildServiceProvider();
//            var apiEndpoint = "/api/User";
//            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
//            var httpClient = httpClientFactory.CreateClient();

//            var randomTokenIndex = new Random().Next(0, appSettings.AuthTokens.Count);
//            var authToken = appSettings.AuthTokens[randomTokenIndex];

//            for (int i = 0; i < appSettings.NumberOfRequests; i++)
//            {
//                var random = new Random();
//                //var isMaliciousRequest = random.Next(0, 2) == 0; // 50% chance of being malicious
//                // var isMaliciousRequest = random.Next(0, 10) < 7;  //70% chance of being malicious

//                var isMaliciousRequest = true; // Always send a malicious request

//                var requestUrl = $"{appSettings.ApiBaseUrl}{apiEndpoint}";
//                if (isMaliciousRequest)
//                {
//                    var randomMaliciousIndex = random.Next(0, maliciousPayloads.Count);
//                    var maliciousPayload = maliciousPayloads[randomMaliciousIndex];
//                    requestUrl += "?maliciousParam=" + maliciousPayload; // Append malicious payload
//                }

//                httpClient.DefaultRequestHeaders.Clear();
//                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
//                var response = await httpClient.GetAsync(requestUrl);

//                Console.WriteLine($"Request {i + 1}: Token: {authToken},\n\n Malicious: {isMaliciousRequest}");
//                Console.WriteLine("\n\nResponse  : \n\n " + await response.Content.ReadAsStringAsync());
//                Console.WriteLine("\n..............................\n\n");
//            }
//        }
//        static void ConfigureServices(IServiceCollection services, AppSetting appSettings)
//        {
//            // Add necessary using directive for HttpClient
//            services.AddHttpClient();

//            // Configure AppSetting using IOptions
//            services.AddOptions<AppSetting>().Configure(options =>
//            {
//                options.ApiBaseUrl = appSettings.ApiBaseUrl;
//                options.NumberOfRequests = appSettings.NumberOfRequests;
//                options.AuthTokens = appSettings.AuthTokens;
//                options.MaliciousPayloads = appSettings.MaliciousPayloads;

//            });
//        }

//    }

//    public class AppSetting
//    {

//        public string ApiBaseUrl { get; set; }
//        public int NumberOfRequests { get; set; }
//        public List<string> AuthTokens { get; set; }
//        public List<string> MaliciousPayloads { get; set; }
//    }
//}


//////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Genrate_Token
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettings = configuration.Get<AppSetting>();

            var services = new ServiceCollection();
            ConfigureServices(services, appSettings);

            var serviceProvider = services.BuildServiceProvider();
            var apiEndpoint = "/api/User";  // Update this to the sensitive data endpoint

            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();

            // Simulate sending requests with unverified-like tokens
            for (int i = 0; i < appSettings.NumberOfRequests; i++)
            {
                // Choose a random valid token from your list
                var randomValidTokenIndex = new Random().Next(0, appSettings.AuthTokens.Count);
                var validToken = appSettings.AuthTokens[randomValidTokenIndex];

                // Create an unverified-like token
                var unverifiedToken = "unverified_" + validToken; // Add a prefix or suffix

                try
                {
                    // Sending a request with an unverified-like token to the sensitive data endpoint
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {unverifiedToken}");
                    var response = await httpClient.GetAsync($"{appSettings.ApiBaseUrl}{apiEndpoint}");

                    // Display the response
                    Console.WriteLine($"Request {i + 1}: Token: {unverifiedToken}");
                    Console.WriteLine("\nResponse:\n" + await response.Content.ReadAsStringAsync());
                    Console.WriteLine("\n..............................\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Request {i + 1} failed: {ex.Message}");
                    Console.WriteLine("\n..............................\n");
                }
            }
        }

        static void ConfigureServices(IServiceCollection services, AppSetting appSettings)
        {
            services.AddHttpClient();

            services.AddOptions<AppSetting>().Configure(options =>
            {
                options.ApiBaseUrl = appSettings.ApiBaseUrl;
                options.NumberOfRequests = appSettings.NumberOfRequests;
                options.AuthTokens = appSettings.AuthTokens;
            });
        }
    }

    public class AppSetting
    {
        public string ApiBaseUrl { get; set; }
        public int NumberOfRequests { get; set; }
        public List<string> AuthTokens { get; set; }
    }
}
