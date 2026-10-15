using System.Text;

namespace EduHub.Ch02.Patterns.Strategies;

public class CsvReportStrategy : IReportStrategy
{
    public string Name => "CSV";

    public string Render(IReadOnlyList<ClubStat> stats)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Club,Members,Events,Registered,CheckedIn");
        foreach (var stat in stats)
        {
            builder.Append(CsvEscape(stat.ClubName))
                .Append(',')
                .Append(stat.Members)
                .Append(',')
                .Append(stat.Events)
                .Append(',')
                .Append(stat.Registered)
                .Append(',')
                .AppendLine(stat.CheckedIn.ToString());
        }

        return builder.ToString();
    }

    private static string CsvEscape(string value) =>
        $"\"{value.Replace("\"", "\"\"")}\"";
}
