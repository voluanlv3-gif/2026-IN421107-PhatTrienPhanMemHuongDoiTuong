```mermaid
classDiagram
    %% Định nghĩa các Class và thuộc tính/mối quan hệ mẫu %%

    class User {
        -int id
        -string fullName
        -string email
        +login() bool
        +logout() void
    }

    class Student {
        -string studentId
        -string major
        +enrollCourse(Course course) void
    }

    class Teacher {
        -string teacherId
        -string department
        +createCourse() Course
    }

    class Course {
        -int courseId
        -string title
        -string description
        +addLesson(Lesson lesson) void
    }

    class Lesson {
        -int lessonId
        -string title
        -string content
    }

    %% Kế thừa (Inheritance)
    User <|-- Student
    User <|-- Teacher

    %% Quan hệ phụ thuộc / kết hợp (Association / Aggregation)
    Teacher "1" --> "*" Course : Giảng dạy
    Course "1" *-- "*" Lesson : Chứa các bài học
    Student "*" --> "*" Course : Đăng ký học
```