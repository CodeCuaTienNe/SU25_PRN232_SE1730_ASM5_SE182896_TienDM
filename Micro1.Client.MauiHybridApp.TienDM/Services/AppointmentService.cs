using Micro1.Client.MauiHybridApp.TienDM.Models;
using Micro1.Client.MauiHybridApp.TienDM.Converters;
using System.Text.Json;
using System.Text;

namespace Micro1.Client.MauiHybridApp.TienDM.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public AppointmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Set 30s timeout
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new TimeOnlyConverter(),
                    new DateOnlyConverter()
                }
            };
        }


        public async Task<ApiResponse<AppointmentResponse>> CreateAppointmentAsync(AppointmentCreateRequest request)
        {
            try
            {
                Console.WriteLine($"[AppointmentService] BaseAddress: {_httpClient.BaseAddress}");

                var json = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"[AppointmentService] POST Request: {json}");
                Console.WriteLine($"[AppointmentService] POST URL: {_httpClient.BaseAddress}gateway/AppointmentsTienDm");

                var response = await _httpClient.PostAsync("/gateway/AppointmentsTienDm", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[AppointmentService] POST Response Status: {response.StatusCode}");
                Console.WriteLine($"[AppointmentService] POST Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var appointment = JsonSerializer.Deserialize<AppointmentResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<AppointmentResponse>
                    {
                        Success = true,
                        Message = "Appointment created successfully",
                        Data = appointment
                    };
                }
                else
                {
                    return new ApiResponse<AppointmentResponse>
                    {
                        Success = false,
                        Message = $"Error: {response.StatusCode} - {responseContent}",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AppointmentService] POST Exception: {ex.Message}");
                return new ApiResponse<AppointmentResponse>
                {
                    Success = false,
                    Message = $"Exception: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
