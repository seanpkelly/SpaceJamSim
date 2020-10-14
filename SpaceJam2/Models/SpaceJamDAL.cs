using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpaceJam2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
namespace SpaceJam2.Models
{
    public class SpaceJamDAL
    {
        public SpaceJamDAL()
        {
        }
        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.balldontlie.io");
            return client;
        }
        public async Task<Stats> GetStats(int playerIds)
        {
            var client = GetClient();
            //data from the API based off of a certain endpoint.
            var response = await client.GetAsync($"/api/v1/season_averages?season=2018&player_ids[]={playerIds}");
            string jasonData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jasonData);
            List<JToken> data = json["data"].ToList();
            Stats newStats = new Stats();
            List<Stats> statsList = new List<Stats>();
            for (int i = 0; i < data.Count; i++)
            {
                newStats = JsonConvert.DeserializeObject<Stats>(data[i].ToString());
                //statsList.Add(newStats);
            }
            return newStats;
        }
        public async Task<List<Players>> GetPlayers(int page, string search)
        {
            var client = GetClient();
            //data from the API based off of a certain endpoint.
            //taking in page number as a parameter.
            string player = search.Trim();
            string[] result = player.Split(" ");
            string final = "";
            if (result.Length > 1)
            {
                final = result[0] + "_" + result[1];
            }
            else
            {
                final = result[0];
            }
            var response = await client.GetAsync($"/api/v1/players?page={page}&search={final}");
            string jasonData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jasonData);
            List<JToken> data = json["data"].ToList();
            //List<Jtoken> meta = json["meta"].ToList();
            Players newPlayers = new Players();
            List<Players> playersList = new List<Players>();
            for (int i = 0; i < data.Count; i++)
            {
                newPlayers = JsonConvert.DeserializeObject<Players>(data[i].ToString());
                playersList.Add(newPlayers);
            }
            return playersList;
        }

        public async Task<Players> GetSpecificPlayer(int id)
        {
            var client = GetClient();
            //data from the API based off of a certain endpoint.
            //taking in page number as a parameter.

            var response = await client.GetAsync($"/api/v1/players/{id}");
            string jasonData = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jasonData);
         

            Players newPlayers = new Players();
            Players playersList = new Players();

            newPlayers = JsonConvert.DeserializeObject<Players>(jasonData);

            return newPlayers;
        }
    }
}