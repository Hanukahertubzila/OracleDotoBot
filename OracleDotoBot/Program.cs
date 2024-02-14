using OracleDotoBot.StratzApiParser.Api;

var api = new DotaApi("https://api.stratz.com/graphql", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTdWJqZWN0IjoiOWEzNzQwYzUtZDFiYS00NTY0LWEyNDItZGFjNTQyZGUzYTZlIiwiU3RlYW1JZCI6IjEwMzczNjc4NjMiLCJuYmYiOjE3MDc4NDM5MDksImV4cCI6MTczOTM3OTkwOSwiaWF0IjoxNzA3ODQzOTA5LCJpc3MiOiJodHRwczovL2FwaS5zdHJhdHouY29tIn0.4V2fJcmWtJ3hETc13qR1LzQVma6swt_dZZRxDq1A5BM");

var aboba = await api.GetMatchUpStatistics(new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
