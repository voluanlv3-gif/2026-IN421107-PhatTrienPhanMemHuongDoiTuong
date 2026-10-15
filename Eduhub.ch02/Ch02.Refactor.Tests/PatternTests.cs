using EduHub.Ch02.Domain;
using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

namespace Ch02.Refactor.Tests;

public class PatternTests
{
    private sealed class FakeLogger : IAppLogger
    {
        public List<string> Lines { get; } = new();
        public void Info(string message) => Lines.Add(message);
    }

    private sealed class SpyObserver : IEventObserver
    {
        public int Calls { get; private set; }
        public void OnEventCreated(Event ev, Club club) => Calls++;
    }

    [Theory]
    [InlineData(NotificationChannel.Email, typeof(EmailNotificationService))]
    [InlineData(NotificationChannel.Sms, typeof(SmsNotificationService))]
    [InlineData(NotificationChannel.Slack, typeof(SlackNotificationService))]
    public void Factory_Create_ReturnsServiceForSelectedChannel(
        NotificationChannel channel,
        Type expectedType)
    {
        // Arrange
        INotificationServiceFactory factory = new NotificationServiceFactory();

        // Act
        var service = factory.Create(channel);

        // Assert
        Assert.IsType(expectedType, service);
    }

    [Fact]
    public void Factory_Create_UnsupportedChannel_Throws()
    {
        // Arrange
        var factory = new NotificationServiceFactory();

        // Act
        var act = () => factory.Create((NotificationChannel)99);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void Decorator_Add_LogsAndStoresClub()
    {
        // Arrange
        var logger = new FakeLogger();
        var inner = new InMemoryClubRepository();
        IClubRepository repository = new LoggingClubRepository(inner, logger);
        var club = Club.Create("Tech Club", "mô tả");

        // Act
        repository.Add(club);

        // Assert
        Assert.Contains(logger.Lines, line => line.Contains("Tech Club"));
        Assert.Same(club, inner.GetById(club.Id));
    }

    [Fact]
    public void Strategy_Render_FormatsSameDataAsConsoleAndCsv()
    {
        // Arrange
        var stats = new List<ClubStat> { new("Tech \"Club\"", 2, 1, 2, 1) };

        // Act
        var console = new ConsoleReportStrategy().Render(stats);
        var csv = new CsvReportStrategy().Render(stats);

        // Assert
        Assert.Contains("Club: Tech \"Club\"", console);
        Assert.StartsWith("Club,Members", csv);
        Assert.Contains("\"Tech \"\"Club\"\"\",2,1,2,1", csv);
    }

    [Fact]
    public void Observer_CreateEvent_NotifiesEverySubscriber()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var service = new EventService(
            clubs,
            new InMemoryMemberRepository(),
            new InMemoryEventRepository());
        var first = new SpyObserver();
        var second = new SpyObserver();
        service.Subscribe(first);
        service.Subscribe(second);

        // Act
        service.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));

        // Assert
        Assert.Equal(1, first.Calls);
        Assert.Equal(1, second.Calls);
    }

    [Fact]
    public void Observer_Unsubscribe_StopsNotifications()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var service = new EventService(
            clubs,
            new InMemoryMemberRepository(),
            new InMemoryEventRepository());
        var observer = new SpyObserver();
        service.Subscribe(observer);
        service.Unsubscribe(observer);

        // Act
        service.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));

        // Assert
        Assert.Equal(0, observer.Calls);
    }
}
