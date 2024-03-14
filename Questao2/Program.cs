using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class Program
{
    public static async Task Main()
    {
        await PrintTotalScoredGoals("Paris Saint-Germain", 2013);
        await PrintTotalScoredGoals("Chelsea", 2014);

        // Output expected:
        // Team Paris Saint-Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task PrintTotalScoredGoals(string teamName, int year)
    {
        int totalGoals = await GetTotalScoredGoals(teamName, year);
        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int page = 1;
        bool hasNextPage = true;

        while (hasNextPage)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(apiResponse);
                    JArray data = (JArray)jsonObject["data"];

                    foreach (var match in data)
                    {
                        int team1Goals = int.Parse(match["team1goals"].ToString());
                        totalGoals += team1Goals;
                    }

                    // Check if there are more pages to fetch
                    int totalPages = int.Parse(jsonObject["total_pages"].ToString());
                    hasNextPage = page < totalPages;
                    page++;
                }
            }
        }

        return totalGoals;
    }
}
