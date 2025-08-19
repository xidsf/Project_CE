using System;
using System.Text;

public static class SerialNumberGenerator
{
    private static readonly StringBuilder sb = new StringBuilder();
    private static int lastSeqNum = 0;

    public static long GenerateSerialNumber()
    {
        var utc = DateTime.UtcNow;
        var utcStr = utc.ToString("yyMMddHHmmss");
        
        if(sb.ToString() == utcStr)
        {
            lastSeqNum++;
            if(lastSeqNum > 99)
            {
                lastSeqNum = 0; // Reset if it exceeds 99
            }
        }
        else
        {
            lastSeqNum = 0;
            sb.Clear();
            sb.Append(utcStr);
        }
        int randNum = UnityEngine.Random.Range(0, 100);
        var serialNumber = $"{utcStr.ToString()}{randNum.ToString("D2")}{lastSeqNum.ToString("D2")}";
        Logger.Log(serialNumber);
        return long.Parse(serialNumber);
    }
}
