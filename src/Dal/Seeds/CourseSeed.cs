using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Seeds;

public static class CourseSeed
{
    public static void SeedCourse(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
                    .HasData(
                        new Course
                        {
                            Id = 1,
                            Name = "C#",
                            Description =
                                "C# is a general-purpose, multi-paradigm programming language encompassing static typing, strong typing, lexically scoped, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines."
                        },
                        new Course
                        {
                            Id = 2,
                            Name = "TypeScript",
                            Description =
                                "TypeScript is a strict syntactical superset of JavaScript and adds optional static typing to the language. It is designed for the development of large applications and transcompiles to JavaScript."
                        },
                        new Course
                        {
                            Id = 3,
                            Name = "JavaScript",
                            Description =
                                "JavaScript, often abbreviated as JS, is a programming language that conforms to the ECMAScript specification. JavaScript is high-level, often just-in-time compiled, and multi-paradigm. It has curly-bracket syntax, dynamic typing, prototype-based object-orientation, and first-class functions."
                        },
                        new Course
                        {
                            Id = 4,
                            Name = "Java",
                            Description =
                                "Java is a general-purpose programming language that is class-based, object-oriented, and designed to have as few implementation dependencies as possible."
                        },
                        new Course
                        {
                            Id = 5,
                            Name = "Kotlin",
                            Description =
                                "Kotlin is a cross-platform, statically typed, general-purpose programming language with type inference. It is designed to interoperate fully with Java."
                        },
                        new Course
                        {
                            Id = 6,
                            Name = "Swift",
                            Description =
                                "Swift is a powerful and intuitive programming language for macOS, iOS, watchOS, and tvOS. Writing Swift code is interactive and fun, the syntax is concise yet expressive, and Swift includes modern features developers love."
                        },
                        new Course
                        {
                            Id = 7,
                            Name = "Rust",
                            Description =
                                "Rust is a multi-paradigm programming language designed for performance and safety, especially safe concurrency. Rust is syntactically similar to C++."
                        },
                        new Course
                        {
                            Id = 8,
                            Name = "Python",
                            Description =
                                "Python is an interpreted, high-level, general-purpose programming language. Created by  "
                        },
                        new Course
                        {
                            Id = 9,
                            Name = "Go",
                            Description =
                                "Go is an open-source programming language that makes it easy to build simple, reliable, and efficient software."
                        },
                        new Course
                        {
                            Id = 10,
                            Name = "Scala",
                            Description =
                                "Scala is a strong statically typed general-purpose programming language which supports both object-oriented programming and functional programming."
                        }
                    );
    }
}