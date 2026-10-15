# Phân tích vi phạm SOLID trong thiết kế ban đầu

Các vị trí dưới đây tham chiếu tới `Ch02.Refactor/ClubManager.cs` ở commit nền
`main` (trước khi refactor). File God class đã được loại bỏ khỏi phiên bản hiện
tại; có thể xem lại bằng `git show main:Ch02.Refactor/ClubManager.cs`.

| Vị trí ban đầu | Nguyên tắc | Vi phạm và hậu quả |
|---|---|---|
| `ClubManager.cs:35-64`, `CreateClub` | SRP | Một phương thức vừa xác thực tên, tạo/lưu CLB, ghi log và gửi thông báo. Thay đổi quy tắc nghiệp vụ, lưu trữ hay kênh gửi đều buộc sửa cùng một class; khó kiểm thử từng trách nhiệm riêng. |
| `ClubManager.cs:52-64`, `CreateClub` | OCP | Các nhánh kiểm tra Gmail/Yahoo quyết định cách gửi. Thêm SMS hoặc một nhà cung cấp khác đòi sửa logic hiện có, tăng nguy cơ ảnh hưởng các nhánh đã chạy ổn. |
| `ClubManager.cs:30-32`, `48`, `79`, `94`, `111` | DIP | Class cấp nghiệp vụ tự giữ `List<T>` để lưu dữ liệu, gọi `File.AppendAllText` và `Console.WriteLine`. Nó phụ thuộc trực tiếp vào chi tiết lưu trữ/hiển thị nên khó thay database, logger hoặc dùng fake trong unit test. |
| `ClubManager.cs:67-84`, `AddMemberToClub` | SRP / OCP | Thêm thành viên đồng thời kiểm tra sức chứa, lưu, ghi file và gửi nhiều thông báo trực tiếp. Mỗi thay đổi hạ tầng hoặc chính sách thông báo làm phình và sửa phương thức nghiệp vụ. |
| `ClubManager.cs:88-99`, `CreateEvent` | SRP / OCP | Tạo sự kiện, ghi log và gửi email cho tất cả thành viên nằm trong cùng một method. `ClubManager` phải biết cả người nhận lẫn cách thông báo; thêm observer mới phải chỉnh code tạo sự kiện. |
| `ClubManager.cs:103-112`, `PrintClubReport` | SRP / OCP | Thống kê, in ra console và ghi log được trộn lẫn. Đổi định dạng hoặc thêm CSV buộc sửa logic báo cáo thay vì thêm một cách trình bày độc lập. |
| Toàn bộ `ClubManager` | ISP / DIP | Một class gom thao tác CLB, thành viên, sự kiện và báo cáo, không có hợp đồng nhỏ để bên gọi chỉ phụ thuộc phần họ cần. Thay thế hoặc kiểm thử một mảng buộc kéo theo cả class. |

## Cách refactor

- `Club.Create` giữ quy tắc xác thực tên; `Event` giữ quy tắc đăng ký và check-in.
- `ClubService`, `EventService` và `ReportService` chia các trách nhiệm nghiệp vụ.
- `IClubRepository`, `IMemberRepository`, `IEventRepository` tách hợp đồng lưu trữ; các service nhận chúng qua constructor.
- `IAppLogger` và `INotificationService` đảo chiều phụ thuộc khỏi file, console và nhà cung cấp cụ thể.
- `NotificationServiceFactory` chọn implementation theo `NotificationChannel`.
- `LoggingClubRepository` bọc `IClubRepository` để thêm log mà không sửa kho dữ liệu.
- `IReportStrategy` cho phép thêm định dạng báo cáo; `IEventObserver` cho phép thêm người nghe mà không sửa logic phát sự kiện.
- `Program.cs` là composition root, nơi các implementation được khởi tạo và ghép với nhau.
