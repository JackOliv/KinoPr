using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinoPr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace KinoPr.Tests
{
   
    [TestClass()]
    public class AutorizationTests
    {
        [TestMethod()]
        public async Task AutorizationAdminTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = true;

            using (HttpClient client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "login", login },
                    { "password", password }
                };

                string queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));

                HttpResponseMessage response = await client.PostAsync($"{BaseUrl}?{queryString}", null);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);

                    int roleId = (int)responseData["data"]["role_id"];
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                    switch (roleId)
                    {
                        case 2:
                            break;
                        case 3:
                            actual = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task AutorizationManagerTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;

            using (HttpClient client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "login", login },
                    { "password", password }
                };

                string queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));

                HttpResponseMessage response = await client.PostAsync($"{BaseUrl}?{queryString}", null);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);

                    int roleId = (int)responseData["data"]["role_id"];
                    switch (roleId)
                    {
                        case 2:
                            actual = true;
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task AutorizationWrongRoleTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "user";
            string password = "useruser";
            bool actual = false;
            bool expected = true;

            using (HttpClient client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "login", login },
                    { "password", password }
                };

                string queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));

                HttpResponseMessage response = await client.PostAsync($"{BaseUrl}?{queryString}", null);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);

                    int roleId = (int)responseData["data"]["role_id"];
                    switch (roleId)
                    {
                        case 2:
                            
                            break;
                        case 3:
                            break;
                        default:
                            actual = true;
                            break;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task AutorizationFailTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "da";
            string password = "da";
            bool actual = false;
            bool expected = false;

            using (HttpClient client = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "login", login },
                    { "password", password }
                };

                string queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));

                HttpResponseMessage response = await client.PostAsync($"{BaseUrl}?{queryString}", null);

                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject responseData = JObject.Parse(responseContent);

                    int roleId = (int)responseData["data"]["role_id"];
                   
                    switch (roleId)
                    {
                        case 2:
                            break;
                        case 3:
                            break;
                        default:
                            
                            break;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        } 
        
    }
}