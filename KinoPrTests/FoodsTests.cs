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
            int actual = 0;
            int expected = 201;
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
                actual = (int)response.StatusCode;  
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailAddFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            int actual = 0;
            int expected = 422;
            string name = "";
            decimal price = Convert.ToDecimal( "0,6");
            int mass = 0;
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
                actual = (int)response.StatusCode;  
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenAddFoodTest()
        {
            int actual = 0;
            int expected = 401;
            string name = "Еда";
            decimal price = Convert.ToDecimal( "166,45");
            int mass = 150;
           
            Product newProduct = new Product
            {
                Name = name,
                Price = price,
                Mass = mass
            };

            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(newProduct.Name), "name");
                multiContent.Add(new StringContent(newProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(newProduct.Mass.ToString()), "mass");
                HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/product", multiContent);
                actual = (int)response.StatusCode;  
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task EditFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            int actual = 0;
            int expected = 200;
            int productid = 0;
            string name = "да";
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/product");
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedProduct.Name), "name");
                multiContent.Add(new StringContent(updatedProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(updatedProduct.Mass.ToString()), "mass");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/product/{productid}", multiContent);
                actual =(int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailEditFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            int actual = 0;
            int expected = 422;
            int productid = 0;
            string name = "";
            decimal price = Convert.ToDecimal("15,2");
            int mass = 10;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    Product nproducts = products.FirstOrDefault(f => f.Name == "да");
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedProduct.Name), "name");
                multiContent.Add(new StringContent(updatedProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(updatedProduct.Mass.ToString()), "mass");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/product/{productid}", multiContent);
                actual =(int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task FailTokenEditFoodTest()
        {
            
            int actual = 0;
            int expected = 401;
            int productid = 0;
            string name = "Eда";
            decimal price = Convert.ToDecimal("166,45");
            int mass = 150;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    Product nproducts = products.FirstOrDefault(f => f.Name == "да");
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
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(new StringContent(updatedProduct.Name), "name");
                multiContent.Add(new StringContent(updatedProduct.Price.ToString(CultureInfo.InvariantCulture)), "price");
                multiContent.Add(new StringContent(updatedProduct.Mass.ToString()), "mass");
                HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/product/{productid}", multiContent);
                actual =(int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public async Task ZDelFoodTest()
        {
            string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
            string login = "manager";
            string password = "managermanager";
            int actual = 0;
            int expected = 200;
            int productid = 0;
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
                HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    Product nproducts = products.FirstOrDefault(f => f.Name == "да");
                    productid = nproducts.Id;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.token);
                HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/product/{productid}");
                actual = (int)response.StatusCode;
            }
            Assert.AreEqual(expected, actual);
        }
    }
}