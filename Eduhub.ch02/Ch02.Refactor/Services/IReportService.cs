namespace EduHub.Ch02.Services;

public interface IReportService
{
    string Generate(Patterns.Strategies.IReportStrategy strategy);
}
