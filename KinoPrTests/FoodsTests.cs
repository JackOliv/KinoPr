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
using System.Diagnostics;
using System.Globalization;

namespace KinoPr.Tests
{
    [TestClass()]
    public class FoodsTests
    {
        [TestMethod()]
        public async Task AddFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;
            string name = "Еда";
            decimal price = Convert.ToDecimal( "166,45");
            int mass = 150;
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
            // Создаем объект продукта для добавления
            Product newProduct = new Product
            {
                Name = name,
                Price = price,
                Mass = mass
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newProduct.Name), "name");
                multiContent.Add(new StringContent(newProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(newProduct.Mass.ToString()), "mass");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/product", multiContent);
                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            bool actual = false;
            bool expected = true;
            int productid = 0;
            string name = "Еда";
            decimal price = Convert.ToDecimal("166,45");
            int mass = 150;
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
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    Product nproducts = products.FirstOrDefault(f => f.Name == "Еда");
                    productid = nproducts.Id;
                }
            }
            Product updatedProduct = new Product
            {
                Name = name,
                Price = price,
                Mass = mass
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                // Добавляем данные формы в мультипарт контент
                multiContent.Add(new StringContent(updatedProduct.Name), "name");
                multiContent.Add(new StringContent(updatedProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(updatedProduct.Mass.ToString()), "mass");


                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/product/{productid}", multiContent);

                if (response.IsSuccessStatusCode)
                {
                    actual = true;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelFoodTest()
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