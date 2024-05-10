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
using Newtonsoft.Json;
using System.IO;

namespace KinoPr.Tests
{
    [TestClass()]
    public class UserTests
    {
        [TestMethod()]
        public async Task AddUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 200;
            string Surname = "Михайличенко";
            string Name = "Елена";
            string Patronumic = "Феликсовна";
            string PhoneNumber = "89196975141";
            DateTime Birh = DateTime.Parse("1981-4-4");
            string Login = "elena7735";
            string Password = "e75bbf55f";
            string Email = "elena7735@hotmail.com";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            User newUser = new User
            {
               surname = Surname,
               name = Name,
               patronymic  = Patronumic,
               phone_number = PhoneNumber,
               birth = Birh,
               login = Login,
               password = Password,
               email = Email,
               role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newUser.name), "name");
                multiContent.Add(new StringContent(newUser.surname), "surname");
                multiContent.Add(new StringContent(newUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(newUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(newUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                multiContent.Add(new StringContent(newUser.login), "login");
                multiContent.Add(new StringContent(newUser.password), "password");
                multiContent.Add(new StringContent(newUser.email), "email");
                multiContent.Add(new StringContent(newUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/user/create", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailAddUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 422;
            string Surname = "";
            string Name = "";
            string Patronumic = "";
            string PhoneNumber = "";
            DateTime Birh = DateTime.Parse("1981-4-4");
            string Login = "";
            string Password = "";
            string Email = "";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            User newUser = new User
            {
               surname = Surname,
               name = Name,
               patronymic  = Patronumic,
               phone_number = PhoneNumber,
               birth = Birh,
               login = Login,
               password = Password,
               email = Email,
               role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newUser.name), "name");
                multiContent.Add(new StringContent(newUser.surname), "surname");
                multiContent.Add(new StringContent(newUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(newUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(newUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                multiContent.Add(new StringContent(newUser.login), "login");
                multiContent.Add(new StringContent(newUser.password), "password");
                multiContent.Add(new StringContent(newUser.email), "email");
                multiContent.Add(new StringContent(newUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/user/create", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenAddUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 401;
            string Surname = "Михайличенко";
            string Name = "Елена";
            string Patronumic = "Феликсовна";
            string PhoneNumber = "89196975141";
            DateTime Birh = DateTime.Parse("1981-4-4");
            string Login = "elena7735";
            string Password = "e75bbf55f";
            string Email = "elena7735@hotmail.com";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            User newUser = new User
            {
               surname = Surname,
               name = Name,
               patronymic  = Patronumic,
               phone_number = PhoneNumber,
               birth = Birh,
               login = Login,
               password = Password,
               email = Email,
               role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newUser.name), "name");
                multiContent.Add(new StringContent(newUser.surname), "surname");
                multiContent.Add(new StringContent(newUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(newUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(newUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                multiContent.Add(new StringContent(newUser.login), "login");
                multiContent.Add(new StringContent(newUser.password), "password");
                multiContent.Add(new StringContent(newUser.email), "email");
                multiContent.Add(new StringContent(newUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/user/create", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 200;
            int userid = 0;
            string Surname = "Швардыгула";
            string Name = "Герман";
            string Patronumic = "Георгиевич";
            string PhoneNumber = "89097070101";
            DateTime Birh = DateTime.Parse("1999-4-4");
            string Login = "german90";
            string Password = "ba658cd5f";
            string Email = "german90@ya.ru";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/users");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    User nuser = users.FirstOrDefault(f => f.login == "elena7735");
                    userid = nuser.id;
                }
            }
            User updatedUser = new User
            {
                surname = Surname,
                name = Name,
                patronymic = Patronumic,
                phone_number = PhoneNumber,
                birth = Birh,
                login = Login,
                password = Password,
                email = Email,
                role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedUser.name), "name");
                multiContent.Add(new StringContent(updatedUser.surname), "surname");
                multiContent.Add(new StringContent(updatedUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(updatedUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(updatedUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                multiContent.Add(new StringContent(updatedUser.login), "login");
                multiContent.Add(new StringContent(updatedUser.password), "password");
                multiContent.Add(new StringContent(updatedUser.email), "email");
                multiContent.Add(new StringContent(updatedUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/user/update/{userid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailEditUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 422;
            int userid = 0;
            string Surname = "da";
            string Name = "da";
            string Patronumic = "da";
            string PhoneNumber = "da";
            string Login = "da";
            string Password = "da";
            string Email = "da@da.ru";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/users");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    User nuser = users.FirstOrDefault(f => f.login == "german90");
                    userid = nuser.id;
                }
            }
            User updatedUser = new User
            {
                surname = Surname,
                name = Name,
                patronymic = Patronumic,
                phone_number = PhoneNumber,
                login = Login,
                password = Password,
                email = Email,
                role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedUser.name), "name");
                multiContent.Add(new StringContent(updatedUser.surname), "surname");
                multiContent.Add(new StringContent(updatedUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(updatedUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(updatedUser.login), "login");
                multiContent.Add(new StringContent(updatedUser.password), "password");
                multiContent.Add(new StringContent(updatedUser.email), "email");
                multiContent.Add(new StringContent(updatedUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/user/update/{userid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenEditUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 401;
            int userid = 0;
            string Surname = "Швардыгула";
            string Name = "Герман";
            string Patronumic = "Георгиевич";
            string PhoneNumber = "89097070101";
            DateTime Birh = DateTime.Parse("1999-4-4");
            string Login = "german90";
            string Password = "ba658cd5f";
            string Email = "german90@ya.ru";
            int Role = 1;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/users");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    User nuser = users.FirstOrDefault(f => f.login == "german90");
                    userid = nuser.id;
                }
            }
            User updatedUser = new User
            {
                surname = Surname,
                name = Name,
                patronymic = Patronumic,
                phone_number = PhoneNumber,
                birth = Birh,
                login = Login,
                password = Password,
                email = Email,
                role_id = Role
            };
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedUser.name), "name");
                multiContent.Add(new StringContent(updatedUser.surname), "surname");
                multiContent.Add(new StringContent(updatedUser.patronymic), "patronymic");
                multiContent.Add(new StringContent(updatedUser.phone_number), "phone_number");
                multiContent.Add(new StringContent(updatedUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                multiContent.Add(new StringContent(updatedUser.login), "login");
                multiContent.Add(new StringContent(updatedUser.password), "password");
                multiContent.Add(new StringContent(updatedUser.email), "email");
                multiContent.Add(new StringContent(updatedUser.role_id.ToString()), "role_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/user/update/{userid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelUserTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 200;
            int userid = 0;
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
                    string token = (string)responseData["data"]["api_token"];
                    Data.token = token;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/users");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    User nuser = users.FirstOrDefault(f => f.login == "german90");
                    userid = nuser.id;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/user/delete/{userid}");
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
    }
}