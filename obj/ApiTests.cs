using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace RestfulApiTests
{
    public class ApiTests
    {
        private readonly HttpClient _client;

        public ApiTests()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://restful-api.dev/") };
        }

        [Fact]
        public async Task GetListOfAllObjects()
        {
            // Act
            var response = await _client.GetAsync("api/objects");
            
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), "Response content should not be empty");
        }

        [Fact]
        public async Task AddObjectUsingPost()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object" };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/objects", content);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
            Assert.NotNull(createdObject.id);
        }

        [Fact]
        public async Task GetSingleObjectById()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object" };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("api/objects", content);
            var createdObject = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            var objectId = createdObject.id;

            // Act
            var response = await _client.GetAsync($"api/objects/{objectId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var fetchedObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
            Assert.Equal(objectId.ToString(), fetchedObject.id.ToString());
        }

        [Fact]
        public async Task UpdateObjectUsingPut()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object" };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("api/objects", content);
            var createdObject = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            var objectId = createdObject.id;
            
            var updatedObject = new { name = "Updated Test Object", description = "This is an updated test object" };
            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedObject), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"api/objects/{objectId}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var fetchedObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
            Assert.Equal("Updated Test Object", fetchedObject.name.ToString());
            Assert.Equal("This is an updated test object", fetchedObject.description.ToString());
        }

        [Fact]
        public async Task DeleteObjectUsingDelete()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object" };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("api/objects", content);
            var createdObject = JsonConvert.DeserializeObject<dynamic>(await createResponse.Content.ReadAsStringAsync());
            var objectId = createdObject.id;

            // Act
            var response = await _client.DeleteAsync($"api/objects/{objectId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var getResponse = await _client.GetAsync($"api/objects/{objectId}");
            Assert.False(getResponse.IsSuccessStatusCode, "Object should be deleted");
        }
    }
}
