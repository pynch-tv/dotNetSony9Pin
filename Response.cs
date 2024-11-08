using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace lathoub;

[DebuggerDisplay("Code: {Code}, Parameters count: {Parameters.Count}")]
public class Response
{
    public ResponseCode Code = ResponseCode.Uninitialised;

    public NameValueCollection Parameters = new();

    public string Text = "";

    internal static bool TryParse(string s, out Response response)
    {
        response = new Response();

        var parts = s.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1) return false;

        var i = parts[0].IndexOf(" ");
        response.Code = (ResponseCode)Convert.ToInt32(parts[0].Substring(0, i));
        response.Text = parts[0].Substring(i + 1).TrimEnd(':');

        if (response.Code == ResponseCode.Commands)
        {
            parts = parts.Skip(1).ToArray();
            response.Parameters["commands"] = string.Join("\n", parts);
            return true;
        }

        // Uptime gies an inconsistent response, parse it
        // separately and provide back only the seconds part
        if (response.Code == ResponseCode.Uptime)
        {
            var times = parts[1].Split(" ");
            response.Parameters["uptime"] = times[0]; // in seconds
            return true;
        }

        // Don't read first line
        for (var j = 1; j < parts.Length; j++)
        {
            var parameter = parts[j];
            i = parameter.IndexOf(":");
            if (i < 0) continue;

            var key = parameter.Substring(0, i);
            var value = parameter.Substring(i + 1).Trim();
            response.Parameters[key] = value;
        }

        return true;
    }

    public static bool IsErrorResponse(ResponseCode code)
    {
        return (code is >= (ResponseCode)100 and <= (ResponseCode)199);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (string key in Parameters)
            sb.Append($"'{key}': {Parameters[key]}; ");

        return $"{Code} {sb}";
    }

}
