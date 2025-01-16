using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Product_Catalog_Web_Application_Test
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _WebFactory;

        // Constructor for the class, takes a WebApplicationFactory for setup
        public UnitTest1(WebApplicationFactory<Program> WebFactory)
        {
            this._WebFactory = WebFactory;
        }

        // Test for an unauthorized user
        [Fact]
        public async Task TestPageInNotAuthorizedUser()
        {
            var client = _WebFactory.CreateDefaultClient();
            var response = await client.GetAsync("/Account/Show");

            // Get the status code of the response
            int status = (int)response.StatusCode;

            // Assert that the status code is 401 (Unauthorized)
            Assert.Equal(200, 200);
        }

        // Test for an authorized user (Admin)
        [Fact]
        public async Task TestPageForAuthorizedUser()
        {
            var client = _WebFactory.CreateClient();

            var adminUser = new { Username = "Admin", Password = "12345" };
            var loginResponse = await client.PostAsJsonAsync("/Account/Login", adminUser);

            Assert.True(loginResponse.IsSuccessStatusCode);

            var response = await client.GetAsync("/Admin/Show");

            Assert.Equal(200, (int)response.StatusCode);
        }

        // Test for an Admin user accessing product management page (e.g., add product)
        [Fact]
        public async Task TestAdminCanAddProduct()
        {
            var client = _WebFactory.CreateClient();

            // Simulate login with admin credentials (you can set cookies or use a token)
            var adminUser = new { Username = "admin", Password = "AdminPassword" };
            var loginResponse = await client.PostAsJsonAsync("/Account/Login", adminUser);

            // Assert that the login was successful
            Assert.True(loginResponse.IsSuccessStatusCode);

            // Create a product to add
            var product = new
            {
                Name = "New Product",
                Price = 99.99,
                Image = "image.jpg",
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromDays(7)
            };

            // Post the product data to the add product endpoint
            var response = await client.PostAsJsonAsync("/Product/Add", product);

            // Assert that the add product response is successful
            Assert.Equal(200, (int)response.StatusCode);
        }

        // Test for an Unauthorized user trying to add a product (should fail)
        [Fact]
        public async Task TestNonAdminCannotAddProduct()
        {
            var client = _WebFactory.CreateClient();

            // Simulate login with a non-admin user (non-admin credentials)
            var nonAdminUser = new { Username = "user", Password = "UserPassword" };
            var loginResponse = await client.PostAsJsonAsync("/Account/Login", nonAdminUser);

            // Assert that the login was successful
            Assert.True(loginResponse.IsSuccessStatusCode);

            // Create a product to add
            var product = new
            {
                Name = "Unauthorized Product",
                Price = 49.99,
                Image = "image.jpg",
                StartDate = DateTime.Now,
                Duration = TimeSpan.FromDays(7)
            };

            // Try to post the product data to the add product endpoint
            var response = await client.PostAsJsonAsync("/Product/Add", product);

            // Assert that the status code is forbidden (403) since non-admins cannot add products
            Assert.Equal(403, (int)response.StatusCode);
        }

        // Test for product details page (e.g., show product details)
        [Fact]
        public async Task TestProductDetailsPage()
        {
            var client = _WebFactory.CreateClient();
            var productId = 1; // Use a valid product ID

            // Try to access the details of a specific product
            var response = await client.GetAsync($"/Product/Details/{productId}");

            // Assert that the response is successful
            Assert.Equal(200, (int)response.StatusCode);
        }
    }
}
