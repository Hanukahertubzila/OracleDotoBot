using OracleDotoBot.StratzApiParser.Api;

var api = new DotaApi("https://api.stratz.com/graphql", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTdWJqZWN0IjoiOWEzNzQwYzUtZDFiYS00NTY0LWEyNDItZGFjNTQyZGUzYTZlIiwiU3RlYW1JZCI6IjEwMzczNjc4NjMiLCJuYmYiOjE3MDc4NDM5MDksImV4cCI6MTczOTM3OTkwOSwiaWF0IjoxNzA3ODQzOTA5LCJpc3MiOiJodHRwczovL2FwaS5zdHJhdHouY29tIn0.4V2fJcmWtJ3hETc13qR1LzQVma6swt_dZZRxDq1A5BM");

var aboba = await api.GetMatchUpStatistics(new List<int> { 98, 95, 19, 17, 26, 86, 129, 113, 49, 45 });

foreach (var item in aboba.stats)
{
    Console.WriteLine();
    Console.WriteLine(item.HeroId);
    Console.WriteLine(item.WinRate);
    Console.WriteLine(item.WinsVs);
    Console.WriteLine(item.WinsWith);
}
