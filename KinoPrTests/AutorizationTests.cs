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
            int actual = 0;
            int expected = 200;
            int roleac = 0;
            int roleex = 3;
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
                    roleac = roleId;
                }
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(roleex, roleac);
        }
        [TestMethod()]
        public async Task AutorizationManagerTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            int actual = 0;
            int expected = 200;
            int roleac = 0;
            int roleex = 2;

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
                    roleac = roleId;
                }
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(roleex, roleac);
        }
        [TestMethod()]
        public async Task AutorizationWrongRoleTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "user";
            string password = "useruser";
            int actual = 0;
            int expected = 200;
            int roleac = 0;
            int roleex = 1;

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
                    roleac = roleId;
                }
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(roleex, roleac);
        }
        [TestMethod()]
        public async Task AutorizationFailTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "da";
            string password = "da";
            int actual = 0;
            int expected = 401;
            int roleac = 0;
            int roleex = 0;

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
                    roleac = roleId;
                }
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(roleex, roleac);
        } 
        
    }
}