using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Services;
public class DlrgCloudService
{
    private readonly HttpClient _httpClient;

    public DlrgCloudService(string baseUrl, string username, string password)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
        _httpClient.DefaultRequestHeaders.Authorization = authValue;
    }

    //URL https://www.dlrg.cloud/remote.php/dav/files/albe/1904000-Leitung%20Einsatz/10-FB_Bootswesen/Test/
    public async Task<string> UploadBlobToDlrgCloudAsync(string endpoint, byte[] content)
    {
        var httpContent = new ByteArrayContent(content);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        var response = await _httpClient.PutAsync(endpoint, httpContent);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
