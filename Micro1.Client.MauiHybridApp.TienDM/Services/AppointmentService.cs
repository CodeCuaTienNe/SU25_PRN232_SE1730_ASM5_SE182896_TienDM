using Micro1.Client.MauiHybridApp.TienDM.Models;
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
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        public async Task<ApiResponse<AppointmentResponse>> CreateAppointmentAsync(AppointmentCreateRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"[AppointmentService] POST Request: {json}");

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

        public async Task<ApiResponse<List<AppointmentResponse>>> GetAllAppointmentsAsync()
        {
            try
            {
                Console.WriteLine($"[AppointmentService] GET Request to /gateway/AppointmentsTienDm");

                var response = await _httpClient.GetAsync("/gateway/AppointmentsTienDm");
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[AppointmentService] GET Response Status: {response.StatusCode}");
                Console.WriteLine($"[AppointmentService] GET Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var appointments = JsonSerializer.Deserialize<List<AppointmentResponse>>(responseContent, _jsonOptions);
                    return new ApiResponse<List<AppointmentResponse>>
                    {
                        Success = true,
                        Message = "Appointments retrieved successfully",
                        Data = appointments ?? new List<AppointmentResponse>()
                    };
                }
                else
                {
                    return new ApiResponse<List<AppointmentResponse>>
                    {
                        Success = false,
                        Message = $"Error: {response.StatusCode} - {responseContent}",
                        Data = new List<AppointmentResponse>()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AppointmentService] GET Exception: {ex.Message}");
                return new ApiResponse<List<AppointmentResponse>>
                {
                    Success = false,
                    Message = $"Exception: {ex.Message}",
                    Data = new List<AppointmentResponse>()
                };
            }
        }
    }
}
