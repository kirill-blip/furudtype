using FurudType.Core.Models;
using FurudType.Core.Repositories;

namespace FurudType.Storage.Repositories;

public class InMemoryLessonRepository : ILessonRepository
{
    private readonly List<Lesson> _lessons = [];
    private readonly object _lock = new();

    public InMemoryLessonRepository()
    {
        Exercise firstExercise = new Exercise() { Text = "fdfd fdfd fdfd fdfd", Title = "First" };
        Exercise secondExercise = new Exercise() { Text = "dkdk dkdk dkdk dkdk", Title = "Second" };
        Exercise fifthExercise = new Exercise() { Text = "Hello, World! It's the longest sentense in", Title = "Fourth" };

        var lesson = new Lesson()
        {
            Title = "First"
        };
        lesson.Exercises.AddRange([firstExercise, secondExercise, fifthExercise]);

        Exercise thirdExercise = new Exercise() { Text = "slsl slsl slsl slsl", Title = "Third" };
        Exercise fourthExercise = new Exercise() { Text = "a;a; a;a; a;a; a;a;", Title = "Fourth" };

        var secondLesson = new Lesson()
        {
            Title = "Second"
        };
        secondLesson.Exercises.AddRange([thirdExercise, fourthExercise]);

        _lessons.Add(lesson);
        _lessons.Add(secondLesson);
    }

    public Task<List<Lesson>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult(_lessons);
        }
    }
}
