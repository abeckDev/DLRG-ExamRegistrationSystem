using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Services;
public class DlrgCloudService
{
    private readonly HttpClient _httpClient;
    private readonly string BaseUrl;

    public DlrgCloudService()
    {
        BaseUrl = Environment.GetEnvironmentVariable("dlrgCloudBasePath");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{System.Environment.GetEnvironmentVariable("dlrgCloudUsername")}:{System.Environment.GetEnvironmentVariable("dlrgCloudPassword")}")));
        _httpClient.DefaultRequestHeaders.Authorization = authValue;
    }

    //Create a method that sends a MKCOL request to the DLRG Cloud
    public Task<string> CreateDirectory(string directoryDir)
    {
        var request = new HttpRequestMessage
        {
            Method = new HttpMethod("MKCOL"),
            RequestUri = new Uri(BaseUrl+directoryDir)
        };
        
        return _httpClient.SendAsync(request).ContinueWith(responseTask =>
        {
            var response = responseTask.Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync();
        }).Unwrap();
    }


    public async Task<string> UploadBlobToDlrgCloudAsync(string endpoint, byte[] content)
    {
        var httpContent = new ByteArrayContent(content);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        var response = await _httpClient.PutAsync(endpoint, httpContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
