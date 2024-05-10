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
    public class FilmTests
    {
        [TestMethod()]
        public async Task AddFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 201;
            string Name = "джокер";
            int GenreId = 1;
            string Duration = "1 ч 30 мин";
            int Year = 2005;
            string Description = "Джокер плахой";
            string Director = "Тимофеев Александр";
            string Country = "США";
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
            Movie newMovie = new Movie
            {
                Name = Name,
                GenreId = GenreId,
                Duration = Duration,
                Year = Year,
                Description = Description,
                Director = Director,
                Country = Country
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newMovie.Name), "name");
                multiContent.Add(new StringContent(newMovie.Duration), "duration");
                multiContent.Add(new StringContent(newMovie.Description), "description");
                multiContent.Add(new StringContent(newMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(newMovie.Country), "country");
                multiContent.Add(new StringContent(newMovie.Director), "director");
                multiContent.Add(new StringContent(newMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/film", multiContent);
                actual = (int)response.StatusCode;
                
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailAddFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 422;
            string Name = "";
            int GenreId = 0;
            string Duration = "";
            int Year = 0;
            string Description = "";
            string Director = "";
            string Country = "";
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
            Movie newMovie = new Movie
            {
                Name = Name,
                GenreId = GenreId,
                Duration = Duration,
                Year = Year,
                Description = Description,
                Director = Director,
                Country = Country
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newMovie.Name), "name");
                multiContent.Add(new StringContent(newMovie.Duration), "duration");
                multiContent.Add(new StringContent(newMovie.Description), "description");
                multiContent.Add(new StringContent(newMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(newMovie.Country), "country");
                multiContent.Add(new StringContent(newMovie.Director), "director");
                multiContent.Add(new StringContent(newMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/film", multiContent);
                actual = (int)response.StatusCode;
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenAddFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 401;
            string Name = "джокер";
            int GenreId = 1;
            string Duration = "1 ч 30 мин";
            int Year = 2005;
            string Description = "Джокер плахой";
            string Director = "Тимофеев Александр";
            string Country = "США";
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
            Movie newMovie = new Movie
            {
                Name = Name,
                GenreId = GenreId,
                Duration = Duration,
                Year = Year,
                Description = Description,
                Director = Director,
                Country = Country
            };

            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newMovie.Name), "name");
                multiContent.Add(new StringContent(newMovie.Duration), "duration");
                multiContent.Add(new StringContent(newMovie.Description), "description");
                multiContent.Add(new StringContent(newMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(newMovie.Country), "country");
                multiContent.Add(new StringContent(newMovie.Director), "director");
                multiContent.Add(new StringContent(newMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/film", multiContent);
                actual = (int)response.StatusCode;
                
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenEditFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 401;
            int filmid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/film");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
                    Movie nfilm = movies.FirstOrDefault(f => f.Name == "Джокер");
                    filmid = nfilm.Id;
                }
            }
            Movie updatedMovie = new Movie
            {
                Name = "Джокер",
                GenreId = 2,
                Duration = "1 ч 31мин",
                Year = 2000,
                Description = "Джокер хороший",
                Director = "Я",
                Country = "Россия"
            };
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedMovie.Name), "name");
                multiContent.Add(new StringContent(updatedMovie.Duration), "duration");
                multiContent.Add(new StringContent(updatedMovie.Description), "description");
                multiContent.Add(new StringContent(updatedMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(updatedMovie.Country), "country");
                multiContent.Add(new StringContent(updatedMovie.Director), "director");
                multiContent.Add(new StringContent(updatedMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/film/{filmid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailEditFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 422;
            int filmid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/film");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
                    Movie nfilm = movies.FirstOrDefault(f => f.Name == "Джокер");
                    filmid = nfilm.Id;
                }
            }
            Movie updatedMovie = new Movie
            {
                Name = "",
                GenreId = 0,
                Duration = "",
                Year = 0,
                Description = "",
                Director = "",
                Country = ""
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedMovie.Name), "name");
                multiContent.Add(new StringContent(updatedMovie.Duration), "duration");
                multiContent.Add(new StringContent(updatedMovie.Description), "description");
                multiContent.Add(new StringContent(updatedMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(updatedMovie.Country), "country");
                multiContent.Add(new StringContent(updatedMovie.Director), "director");
                multiContent.Add(new StringContent(updatedMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/film/{filmid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 200;
            int filmid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/film");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
                    Movie nfilm = movies.FirstOrDefault(f => f.Name == "джокер");
                    filmid = nfilm.Id;
                }
            }
            Movie updatedMovie = new Movie
            {
                Name = "Джокер",
                GenreId = 2,
                Duration = "1 ч 31мин",
                Year = 2000,
                Description = "Джокер хороший",
                Director = "Я",
                Country = "Россия"
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedMovie.Name), "name");
                multiContent.Add(new StringContent(updatedMovie.Duration), "duration");
                multiContent.Add(new StringContent(updatedMovie.Description), "description");
                multiContent.Add(new StringContent(updatedMovie.Year.ToString()), "year");
                multiContent.Add(new StringContent(updatedMovie.Country), "country");
                multiContent.Add(new StringContent(updatedMovie.Director), "director");
                multiContent.Add(new StringContent(updatedMovie.GenreId.ToString()), "genre_id");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/film/{filmid}", multiContent);
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelFilmTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "admin";
            string password = "adminadmin";
            int actual = 0;
            int expected = 200;
            int filmid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/film");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
                    Movie nfilm = movies.FirstOrDefault(f => f.Name == "Джокер");
                    filmid = nfilm.Id;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/film/{filmid}");
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
    }
}