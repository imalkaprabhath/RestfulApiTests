using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

public class ApiTests
{
    private readonly HttpClient _client;

    public ApiTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("https://restful-api.dev/") };
    }

    public async Task GetListOfAllObjects()
    {
        var response = await _client.GetAsync("api/objects");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseString);
    }

    public async Task AddObjectUsingPost()
    {
        var newObject = new { Name = "Test Object", Value = "Test Value" };
        var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/objects", content);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var createdObject = JsonConvert.DeserializeObject<dynamic>(responseString);

        Assert.Equal(newObject.Name, (string)createdObject.name);
        Assert.Equal(newObject.Value, (string)createdObject.value);
    }

    public async Task GetSingleObjectUsingId()
    {
        var newObject = new { Name = "Test Object", Value = "Test Value" };
        var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

        var createResponse = await _client.PostAsync("api/objects", content);
        createResponse.EnsureSuccessStatusCode();
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createdObject = JsonConvert.DeserializeObject<dynamic>(createResponseString);
        var objectId = (string)createdObject.id;

        var getResponse = await _client.GetAsync($"api/objects/{objectId}");
        getResponse.EnsureSuccessStatusCode();
        var getResponseString = await getResponse.Content.ReadAsStringAsync();
        var retrievedObject = JsonConvert.DeserializeObject<dynamic>(getResponseString);

        Assert.Equal(newObject.Name, (string)retrievedObject.name);
        Assert.Equal(newObject.Value, (string)retrievedObject.value);
    }

    public async Task UpdateObjectUsingPut()
    {
        var newObject = new { Name = "Test Object", Value = "Test Value" };
        var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

        var createResponse = await _client.PostAsync("api/objects", content);
        createResponse.EnsureSuccessStatusCode();
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createdObject = JsonConvert.DeserializeObject<dynamic>(createResponseString);
        var objectId = (string)createdObject.id;

        var updatedObject = new { Name = "Updated Object", Value = "Updated Value" };
        var updateContent = new StringContent(JsonConvert.SerializeObject(updatedObject), Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync($"api/objects/{objectId}", updateContent);
        updateResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"api/objects/{objectId}");
        getResponse.EnsureSuccessStatusCode();
        var getResponseString = await getResponse.Content.ReadAsStringAsync();
        var retrievedObject = JsonConvert.DeserializeObject<dynamic>(getResponseString);

        Assert.Equal(updatedObject.Name, (string)retrievedObject.name);
        Assert.Equal(updatedObject.Value, (string)retrievedObject.value);
    }

    public async Task DeleteObjectUsingDelete()
    {
        var newObject = new { Name = "Test Object", Value = "Test Value" };
        var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

        var createResponse = await _client.PostAsync("api/objects", content);
        createResponse.EnsureSuccessStatusCode();
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createdObject = JsonConvert.DeserializeObject<dynamic>(createResponseString);
        var objectId = (string)createdObject.id;

        var deleteResponse = await _client.DeleteAsync($"api/objects/{objectId}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"api/objects/{objectId}");
        Assert.False(getResponse.IsSuccessStatusCode);
    }
}
