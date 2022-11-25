using Apcitas.WebService.DTOs;
using AppcitasUniTest.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppcitasUniTest.Tests;

public class BuggyControllerTest
{
    private string apiRoute = "api/buggy";
    private readonly HttpClient _client;
    private HttpResponseMessage? httpResponse;
    private string requestUrl = String.Empty;
    private string loginObject = String.Empty;
    private HttpContent? httpContent;

    public BuggyControllerTest()
    {
        _client = TestHelper.Instance.Client;
    }

    [Theory]
    [InlineData("OK", "lisa", "Pa$$w0rd")]
    public async Task GetSecret_ShouldOK(string statusCode, string username, string password)
    {
        //Arrange
        requestUrl = "api/account/login";
        var loginDto = new LoginDto
        {
            Username = username,
            Password = password
        };
        loginObject = GetLoginObject(loginDto);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);
        //Todo esto se puede  considerar como el loging (lo anterior)
        requestUrl = $"{apiRoute}/auth";

        //Act
        httpResponse = await _client.GetAsync(requestUrl);

        //Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("not-found", "NotFound")]
    [InlineData("server-error", "InternalServerError")]
    [InlineData("bad-request", "BadRequest")]
    public async Task GetEndpoints_ShouldValidate(string endpoint, string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/{endpoint}";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NotFound")]
    public async Task GetNotFound_ShouldNotFound(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/not-found";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("InternalServerError")]
    public async Task GetServerError_ShouldNotInternalServerError(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/server-error";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest")]
    public async Task GetBadRequest_ShouldBadRequest(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/bad-request";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Private Methods
    private static string GetLoginObject(LoginDto loginDto)
    {
        var entityObj = new JObject()
        {
            {nameof (loginDto.Username), loginDto.Username },
            {nameof (loginDto.Password), loginDto.Password }
        };
        return entityObj.ToString();

    }

    private static StringContent GetHttpContent(string objectToCode)
    {
        return new StringContent(objectToCode, Encoding.UTF8, "application/json");
    }
    #endregion
}
