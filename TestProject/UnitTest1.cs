using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace TestProject
{
    public class UnitTest1:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public UnitTest1()
        {
            var factory=new WebApplicationFactory<Program>();
            this._factory = factory;
        }
        [Fact]
        public async Task TestRegisterPage()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Account/Register");
            int status = (int)response.StatusCode;

            Assert.Equal(200, status);
        }
        [Fact]
        public async Task TestAuthrization()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Admin/Show");
            int status = (int)response.StatusCode;

            Assert.Equal(405, status);

        }
        [Fact]
        public async Task LoginAsAdmin()
        {
            var client = _factory.CreateClient();

            var loginData = new
            {
                Username = "Admin", 
                Password = "12345" 
            };

            // Serialize the login data to JSON
            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Account/Login", content);

            Assert.Equal(200, (int)response.StatusCode); 
        }
        [Fact]
        public async Task TestAuthorizationOfAdminRole()
        {
            var client = _factory.CreateClient();
            var loginData = new
            {
                Username = "mohamed", 
                Password = "123"
            };
            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var loginResponse = await client.PostAsync("/Account/Login", content);
            loginResponse.EnsureSuccessStatusCode();

            var response = await client.GetAsync("/Admin/Show");

            Assert.Equal(400, (int)response.StatusCode); 
        }
        [Fact]
        public async Task TestAuthorizedAdmin()
        {
            var client = _factory.CreateClient();
            var loginData = new
            {
                Username = "Admin",
                Password = "12345"
            };
            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var loginResponse = await client.PostAsync("/Account/Login", content);
            loginResponse.EnsureSuccessStatusCode();

            var response = await client.GetAsync("/Admin/Show");

            Assert.Equal(200, (int)response.StatusCode);
        }
        [Fact]
        public async Task TestLoginPage()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Account/LoginPage");
            int status = (int)response.StatusCode;

            Assert.Equal(200, status);
        }
    }
}
