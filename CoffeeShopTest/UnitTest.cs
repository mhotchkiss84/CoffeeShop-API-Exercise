﻿using CoffeeShop.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace CoffeeShopTest
{
    public class CoffeeTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public CoffeeTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task Test_Get_All_Coffees()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/coffees");
                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var coffeeList = JsonConvert.DeserializeObject<List<Coffee>>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(coffeeList.Count > 0);
            }
        }
        [Fact]
        public async Task Create_One_Coffee()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Coffee newCoffee = new Coffee()
                {
                    Title = "Test Coffee",
                    BeanType = "New Bean Type"
                };
                string jsonCoffee = JsonConvert.SerializeObject(newCoffee);
                // Act
                HttpResponseMessage response = await client.PostAsync("/api/coffees",
                    new StringContent(jsonCoffee, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Coffee coffeeResponse = JsonConvert.DeserializeObject<Coffee>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(coffeeResponse.Title, newCoffee.Title);
            }
        }
        [Fact]
        public async Task Test_One_Coffee()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/coffees/{fixture.TestCoffee.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Coffee singleCoffee = JsonConvert.DeserializeObject<Coffee>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //Assert.Equal(fixture.TestCoffee.Title, singleCoffee.Title);
            }
        }
        [Fact]
        public async Task Edit_Coffee()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Coffee editedCoffee = new Coffee()
                {
                    Title = "EDITED COFFEE",
                    BeanType = "New Bean Type"
                };
                // Act
                string jsonCoffee = JsonConvert.SerializeObject(editedCoffee);
                HttpResponseMessage response = await client.PutAsync($"/api/coffees/{fixture.TestCoffee.Id}",
                    new StringContent(jsonCoffee, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
        [Fact]
        public async Task Delete_Coffee()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/coffees/{fixture.TestCoffee.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
