using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpelArkad.Models;

namespace SpelArkad.Services
{
    public class SpelService
    {
        private readonly HttpClient _httpClient;

        public SpelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Spel>> GetSpelAsync()
        {
            var response = await _httpClient.GetAsync("https://informatik7.ei.hv.se/Gameapi/api/Spel");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Spel>(); 
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Spel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task CreateSpelAsync(Spel spel)
        {
            try
            {
                var json = JsonSerializer.Serialize(spel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://informatik7.ei.hv.se/Gameapi/api/Spel", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Misslyckades att skapa spel: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid skapande av spel: {ex.Message}");
                throw;
            }
        }

        public async Task RaderaSpelAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://informatik7.ei.hv.se/Gameapi/api/Spel/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Misslyckades att radera spel: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid radering av spel: {ex.Message}");
                throw;
            }
        }
    }
}
