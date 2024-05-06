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

namespace KinoPr.Tests
{
    [TestClass()]
    public class GenreTests
    {
        [TestMethod()]
        public async Task AddGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = true;
            string genreName = "da";
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
            Genre newGenre = new Genre
            {
                Name = genreName
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/genre", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = true;
            int genreid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                    Genre daGenre = genres.FirstOrDefault(g => g.Name == "da");
                    genreid = daGenre.Id;
                }
            }
            Genre updatedGenre = new Genre
            {
                Id = genreid,
                Name = "Da"
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/genre/{genreid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailAddGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = false;
            string genreName = "";
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
            Genre newGenre = new Genre
            {
                Name = genreName
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/genre", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenAddGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = false;
            string genreName = "da";
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
            Genre newGenre = new Genre
            {
                Name = genreName
            };
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/genre", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod()]
        public async Task FailEditGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = false;
            int genreid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                    Genre daGenre = genres.FirstOrDefault(g => g.Name == "Da");
                    genreid = daGenre.Id;
                }
            }
            Genre updatedGenre = new Genre
            {
                Id = genreid,
                Name = ""
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/genre/{genreid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenEditGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = false;
            int genreid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                    Genre daGenre = genres.FirstOrDefault(g => g.Name == "Da");
                    genreid = daGenre.Id;
                }
            }
            // Создаем объект Genre для обновления данных
            Genre updatedGenre = new Genre
            {
                Id = genreid,
                Name = "Da"
            };

            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedGenre.Name), "name");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/genre/{genreid}", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelGenreTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            bool actual = false;
            bool expected = true;
            int genreid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                    Genre daGenre = genres.FirstOrDefault(g => g.Name == "Da");
                    genreid = daGenre.Id;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/genre/{genreid}");
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}