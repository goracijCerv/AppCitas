using Apcitas.WebService.DTOs;
using AppcitasUniTest.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppcitasUniTest.Tests
{
    public class MessageControlerTest
    {
        private string apiRoute = "api/messages";
        private readonly HttpClient _client;
        private HttpResponseMessage? httpResponse;
        public string requestUrl = String.Empty;
        private string registerObject = String.Empty;
        private string loginObject = String.Empty;
        private HttpContent? httpContent;

        public MessageControlerTest()
        {
            _client = TestHelper.Instance.Client;
        }

        //Envio a si mismo
        [Theory]
        [InlineData("BadRequest", "lisa", "I love my self")]
        public async Task SendMessage_ShouldBeBadRequest(string statusCode, string sendTo, string content)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}";
            var message = new MessageCreateDto
            {
                RecipientUsername = sendTo,
                Content = content
            };
            registerObject = GetMessageCreate(message);
            httpContent = GetHttpContent(registerObject);
            //Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //Enviando a persona no registrada

        [Theory]
        [InlineData("NotFound", "PedritoSola", "¿Sabes?, siempre crei que yo te habia rescatado de la torre del dragon")]
        public async Task SendMessage_ShouldBeNotFound(string statusCode, string sendTo, string content)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}";
            var message = new MessageCreateDto
            {
                RecipientUsername = sendTo,
                Content = content
            };
            registerObject = GetMessageCreate(message);
            httpContent = GetHttpContent(registerObject);
            //Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //Enviando mensjae a todd
        [Theory]
        [InlineData("OK", "todd", "Hola guapo a que hora sales por el pan")]
        public async Task SendMessage_ShouldBeOk(string statusCode, string sendTo, string content)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}";
            var message = new MessageCreateDto
            {
                RecipientUsername = sendTo,
                Content = content
            };
            registerObject = GetMessageCreate(message);
            httpContent = GetHttpContent(registerObject);
            //Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //Obteniendo los mensajes default
        [Theory]
        [InlineData("OK")]
        public async Task GetMessageDeafult_ShouldBeOk(string statusCode)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}";

            //Act
            httpResponse = await _client.GetAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //Variando los valores de container
        [Theory]
        [InlineData("OK", "outbox")]
        [InlineData("OK", "Inbox")]
        public async Task GetMessageVariable_ShouldBeOk(string statusCode, string containerValue)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}?container={containerValue}";

            //Act
            httpResponse = await _client.GetAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        //obteniendo hilo de conversacion
        [Theory]
        [InlineData("OK", "todd")]
        public async Task GetMessageThread_ShouldBeOk(string statusCode, string otherPerson)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}/thread/{otherPerson}";

            //Act
            httpResponse = await _client.GetAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("MethodNotAllowed", "")]
        public async Task GetMessageThread_ShouldBeNotAllowed(string statusCode, string error)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}/thread/{error}";

            //Act
            httpResponse = await _client.GetAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        
        [Theory]
        [InlineData("OK", "1")]
        public async Task DelateMessage_ShouldBeOk(string statusCode, string idMessage)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}/{idMessage}";

            //Act
            httpResponse = await _client.DeleteAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
       

        [Theory]
        [InlineData("MethodNotAllowed", "")]
        public async Task DelateMessage_ShouldBeNotAllowed(string statusCode, string idMessage)
        {
            //Arrange
            _client.DefaultRequestHeaders.Authorization = await GetAuthoritation();
            requestUrl = $"{apiRoute}/{idMessage}";

            //Act
            httpResponse = await _client.DeleteAsync(requestUrl);

            //Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        
        


        #region Privated methods        
        private async Task<AuthenticationHeaderValue> GetAuthoritation()
        {
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = "lisa",
                Password = "Pa$$w0rd"
            };
            loginObject = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObject);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new AuthenticationHeaderValue("Bearer", userDto.Token);
        }

        private static string GetMessageCreate(MessageCreateDto messageCreateDto)
        {
            var entityObject = new JObject()
            {
                { nameof(messageCreateDto.RecipientUsername), messageCreateDto.RecipientUsername},
                { nameof(messageCreateDto.Content), messageCreateDto.Content },

            };

            return entityObject.ToString();
        }

        private static string GetLoginObject(LoginDto loginDto)
        {
            var entityObj = new JObject()
            {
                {nameof (loginDto.Username), loginDto.Username },
                {nameof (loginDto.Password), loginDto.Password }
            };
            return entityObj.ToString();

        }

        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
