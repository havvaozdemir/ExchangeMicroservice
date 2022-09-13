using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;

using ExchangeAPI.Controllers.Data;

namespace ExchangeAPITests;

public class ExchangeControllerTests : IClassFixture<WebApplicationFactory<ExchangeAPI.Startup>>
{
    readonly HttpClient m_Client;

    public ExchangeControllerTests(WebApplicationFactory<ExchangeAPI.Startup> application)
    {
        m_Client = application.CreateClient();
    }

    [Fact]
    public async Task PostRequestSuccess()
    {
        RatesRequest data = new RatesRequest
        {
            Base = "USD",
            Target = "TRY"
        };

        var myContent = JsonConvert.SerializeObject(data);
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await m_Client.PostAsync("/exchange/customers/1/currency/convert", byteContent);
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonConvert.DeserializeObject<RatesResponse>(
                await response.Content.ReadAsStringAsync()
              );
        result!.Success.Should().Be(true);
    }
    [Fact]
    public async Task PostRequestCurrencyNotFoundSuccesWillFalse()
    {
        RatesRequest data = new RatesRequest
        {
            Base = "USD",
            Target = "AUD"
        };

        var myContent = JsonConvert.SerializeObject(data);
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await m_Client.PostAsync("/exchange/customers/1/currency/convert", byteContent);
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = JsonConvert.DeserializeObject<RatesResponse>(
                await response.Content.ReadAsStringAsync()
              );
        result!.Success.Should().Be(false);
        result!.Info.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostRequestsMoreThan10TimesGivesError()
    {
        RatesRequest data = new RatesRequest
        {
            Base = "USD",
            Target = "AUD"
        };

        var myContent = JsonConvert.SerializeObject(data);
        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        for (int i = 0; i < 8; i++)
        {
            var res = await m_Client.PostAsync("/exchange/customers/1/currency/convert", byteContent);
            res.EnsureSuccessStatusCode();

        }
        var response = await m_Client.PostAsync("/exchange/customers/1/currency/convert", byteContent);

        response.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }
}