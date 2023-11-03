namespace IntegrationTests.Reference;

using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApiDemo.Api.Startup;
using WebApiDemo.Dal.Records;
using WebApiDemo.Service.Domain;

public class ControllerActions : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public int CategoryCount { get; set; }

    public CategoryRecord FirstCategoryItem { get; set; }

    public ControllerActions()
    {
        var webApplicationFactory = new TestWebApplicationFactory<Startup>();
        _client = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        this.CategoryCount = webApplicationFactory.CategoryCount();
        this.FirstCategoryItem = webApplicationFactory.GetFirstCategory();
    }

    public async Task<TestResult<T>> GetAsync<T>(string path) where T : class
    {
        return await ConvertResponse<T>(await _client.GetAsync(path));
    }

    public async Task<TestResult<T>> PatchAsync<T>(string path, object obj) where T : class
    {
        return await ConvertResponse<T>(await _client.PatchAsync(path, CreateContent(obj)));
    }

    public async Task<TestResult<T>> PostAsync<T>(string path, object obj) where T : class
    {
        return await ConvertResponse<T>(await _client.PostAsync(path, CreateContent(obj)));
    }

    public async Task<TestResult<T>> PutAsync<T>(string path, object obj) where T : class
    {
        return await ConvertResponse<T>(await _client.PutAsync(path, CreateContent(obj)));
    }

    public async Task<TestResult<T>> DeleteAsync<T>(string path) where T : class
    {
        return await ConvertResponse<T>(await _client.DeleteAsync(path));
    }

    private static async Task<TestResult<T>> ConvertResponse<T>(HttpResponseMessage message) where T : class
    {
        TestResult<T> newResult = new()
        {
            HttpResponseStatus = message.StatusCode
        };
        if (message.StatusCode == HttpStatusCode.OK)
        {
            string resultString = await message.Content.ReadAsStringAsync();

            newResult.Content = JsonSerializer.Deserialize<T>(resultString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            newResult.ExceptionMessage = await message.Content.ReadAsStringAsync();
        }

        return newResult;
    }

    private static StringContent CreateContent(object obj)
    {
        string json = JsonSerialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static string JsonSerialize(object obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}