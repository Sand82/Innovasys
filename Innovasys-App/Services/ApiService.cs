using Innovasys_App.Models.DTOs;
using System.Text.Json;

namespace Innovasys_App.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<UserDTO>> LoadData()
        {
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

            response.EnsureSuccessStatusCode(); //Throw error 

            var json = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return users ?? new List<UserDTO>();
        }
    }
}
