using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace WebUsers
{
    public static class RandomUsers
    {


        static readonly HttpClient client = new();

        /// <summary>
        /// Gets an an observable list with some of the fields provided from randomuser.me
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<User>> GetUsers(int count)
        {
            Users? users = null;
            var httpResponse = await client.GetAsync("https://randomuser.me/api/?results=" + count);
            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content is not null && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<Users>(contentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });               
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }
            return users?.Results ?? new ObservableCollection<User>();
        }
    }
}
