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
    public class SessionsTests
    {
        [TestMethod()]
        public async Task AddSessionTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 1;
            int FilmId = 1;
            int hall = 1;
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
            Session newSession = new Session
            {
                time_start = time_start,
                time_end = time_end,
                session_status_id = session_status_id,
                FilmId = FilmId,
                hall = hall
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(newSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(newSession.time_end.ToString("yyyy-MM-dd HH:mm:ss")), "time_end");
                multiContent.Add(new StringContent(newSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(newSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(newSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/session", multiContent);

                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenAddSessionest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = false;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 1;
            int FilmId = 1;
            int hall = 1;
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
            Session newSession = new Session
            {
                time_start = time_start,
                time_end = time_end,
                session_status_id = session_status_id,
                FilmId = FilmId,
                hall = hall
            };

            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(newSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(newSession.time_end.ToString("yyyy-MM-dd HH:mm:ss")), "time_end");
                multiContent.Add(new StringContent(newSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(newSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(newSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/session", multiContent);

                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailAddSessionest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = false;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 10;
            int FilmId = 10;
            int hall = 10;
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
            Session newSession = new Session
            {
                time_start = time_start,
                time_end = time_end,
                session_status_id = session_status_id,
                FilmId = FilmId,
                hall = hall
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(newSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(newSession.time_end.ToString("yyyy-MM-dd HH:mm:ss")), "time_end");
                multiContent.Add(new StringContent(newSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(newSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(newSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/session", multiContent);

                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditSessionTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;
            int sessionid = 0;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 4;
            int FilmId = 2;
            int hall = 1;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/session");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(responseBody);
                    Session nsession = sessions.FirstOrDefault(f => f.time_start == DateTime.Parse("2005-5-5 16:0:0"));
                    sessionid = nsession.id;
                }
            }
            Session updatedSession = new Session
            {
                time_start = time_start,
                time_end = time_start,
                session_status_id = session_status_id,
                hall = hall,
                FilmId = FilmId,
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(updatedSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(updatedSession.time_end.ToString("yyyy-M-d H:m:s")), "time_end");
                multiContent.Add(new StringContent(updatedSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(updatedSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(updatedSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/session/{sessionid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailEditSessionTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = false;
            int sessionid = 0;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 10;
            int FilmId = 10;
            int hall = 10;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/session");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(responseBody);
                    Session nsession = sessions.FirstOrDefault(f => f.time_start == DateTime.Parse("2005-5-5 16:0:0"));
                    sessionid = nsession.id;
                }
            }
            Session updatedSession = new Session
            {
                time_start = time_start,
                time_end = time_start,
                session_status_id = session_status_id,
                hall = hall,
                FilmId = FilmId,
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(updatedSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(updatedSession.time_end.ToString("yyyy-M-d H:m:s")), "time_end");
                multiContent.Add(new StringContent(updatedSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(updatedSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(updatedSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/session/{sessionid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenEditSessionTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = false;
            int sessionid = 0;
            DateTime time_start = DateTime.Parse("2005-5-5 16:0:0");
            DateTime time_end = DateTime.Parse("2005-5-5 16:0:0");
            int session_status_id = 4;
            int FilmId = 2;
            int hall = 1;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/session");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(responseBody);
                    Session nsession = sessions.FirstOrDefault(f => f.time_start == DateTime.Parse("2005-5-5 16:0:0"));
                    sessionid = nsession.id;
                }
            }
            Session updatedSession = new Session
            {
                time_start = time_start,
                time_end = time_start,
                session_status_id = session_status_id,
                hall = hall,
                FilmId = FilmId,
            };
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(updatedSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                multiContent.Add(new StringContent(updatedSession.time_end.ToString("yyyy-M-d H:m:s")), "time_end");
                multiContent.Add(new StringContent(updatedSession.session_status_id.ToString()), "session_status_id");
                multiContent.Add(new StringContent(updatedSession.FilmId.ToString()), "film_id");
                multiContent.Add(new StringContent(updatedSession.hall.ToString()), "hall_id");

                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/session/{sessionid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelSessionTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;
            int sessionid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/session");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(responseBody);
                    Session nsession = sessions.FirstOrDefault(f => f.time_start == DateTime.Parse("2005-5-5 16:0:0"));
                    sessionid = nsession.id;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/session/{sessionid}");
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}