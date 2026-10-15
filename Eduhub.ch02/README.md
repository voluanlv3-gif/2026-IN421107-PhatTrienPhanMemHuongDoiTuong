# Starter Ch02 — Refactoring EduHub với SOLID và Design Patterns

Bài thực hành Ch02 refactor quản lý câu lạc bộ EduHub theo SOLID và bốn design pattern. Phân tích các vi phạm trong thiết kế ban đầu xem tại [`docs/solid-analysis.md`](docs/solid-analysis.md).

## Cấu trúc

- `Domain/`: quy tắc tạo câu lạc bộ, số lượng thành viên, đăng ký và check-in.
- `Services/`: điều phối nghiệp vụ câu lạc bộ, sự kiện và báo cáo.
- `Repositories/`: interface kho dữ liệu, triển khai trong bộ nhớ và decorator ghi log.
- `Notifications/`: Factory tạo dịch vụ email, SMS hoặc Slack.
- `Patterns/Observers/`: observer nhận thông báo khi tạo sự kiện.
- `Patterns/Strategies/`: định dạng báo cáo console và CSV.
- `Logging/`: abstraction và triển khai log ra console hoặc file.
- `Ch02.Refactor.Tests/`: kiểm thử domain và design pattern theo Arrange-Act-Assert.

`Program.cs` là composition root: nơi lắp ghép các triển khai cụ thể; các service nhận phụ thuộc qua interface và constructor.

## Chạy và kiểm thử

```bash
dotnet build Ch02.Refactor.sln
dotnet test Ch02.Refactor.sln
dotnet run --project Ch02.Refactor
```

## Git

Các thay đổi được thực hiện trên nhánh `feature/solid-refactor` và commit theo Conventional Commits. Trước khi mở Pull Request, cập nhật nhánh từ `main`; không push nếu chưa được yêu cầu.
