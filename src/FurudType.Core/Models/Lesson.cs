namespace FurudType.Core.Models;

public class Lesson
{
    public required string Title { get; init; }

    public List<Exercise> Exercises { get; private set; } = [];
}
