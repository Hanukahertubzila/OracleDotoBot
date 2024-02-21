namespace OracleDotoBot.StratzApiParser.OutputDataTypes
{
    public class LaningWDL
    {
        public int WinCount { get; set; }

        public int DrawCount { get; set; }

        public int LossCount { get; set; }

        public LaningWDL GetWDLSum(params LaningWDL[] wdl)
        {
            var result = this;
            foreach (var i in wdl)
            {
                result.WinCount += i.WinCount;
                result.DrawCount += i.DrawCount;
                result.LossCount += i.LossCount;
            }
            result.WinCount = (int)Math.Round(result.WinCount / (wdl.Length + 1d));
            result.DrawCount = (int)Math.Round(result.DrawCount / (wdl.Length + 1d));
            result.LossCount = (int)Math.Round(result.LossCount / (wdl.Length + 1d));
            return result;
        }
    }
}
