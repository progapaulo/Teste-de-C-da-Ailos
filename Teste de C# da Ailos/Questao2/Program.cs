using Newtonsoft.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" - scored "+ totalGoals.ToString() + " - goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " - scored " + totalGoals.ToString() + " - goals in " + year);
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        var service = new FootballStatsService();
            
        return await service.GetGoalsScoredAsync(year, team);
    }

}

public class FootballMatch
{
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public int Team1Goals { get; set; }
    public int Team2Goals { get; set; }
}

public class ApiResponse
{
    public List<FootballMatch> Data { get; set; }
    public int TotalPages { get; set; }
}

public class FootballStatsService
{
    private static readonly HttpClient client = new HttpClient();
    
    public async Task<ApiResponse> GetFootballMatchesAsync(int year, string team = null, int page = 1)
    {
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&page={page}";
        if (!string.IsNullOrEmpty(team))
        {
            url += $"&team1={team}";
        }

        var response = await client.GetStringAsync(url);
        return JsonConvert.DeserializeObject<ApiResponse>(response);
    }

    public int CalculateGoalsScored(List<FootballMatch> matches, string team)
    {
        return matches
                   .Where(m => m.Team1 == team)
                   .Sum(m => m.Team1Goals) + 
               matches
                   .Where(m => m.Team2 == team)
                   .Sum(m => m.Team2Goals);
    }

    public async Task<int> GetGoalsScoredAsync(int year, string team)
    {
        int totalGoals = 0;
        int page = 1;

        // Itera sobre todas as páginas da API até alcançar a última
        while (true)
        {
            var response = await GetFootballMatchesAsync(year, team, page);
            totalGoals += CalculateGoalsScored(response.Data, team);

            if (page >= response.TotalPages)
                break;

            page++;
        }

        return totalGoals;
    }
}