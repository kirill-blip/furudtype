using FurudType.Core.Models;
using FurudType.Core.Repositories;

namespace FurudType.Storage;

public class InMemoryLessonRepository : ILessonRepository
{
    private readonly List<Lesson> _lessons = [];

    public InMemoryLessonRepository()
    {
        Exercise firstExercise = new Exercise() { Text = "fdfd fdfd fdfd fdfd", Title = "First" };
        Exercise secondExercise = new Exercise() { Text = "dkdk dkdk dkdk dkdk", Title = "Second" };
        
        var lesson = new Lesson()
        {
            Title = "First"
        };
        lesson.Exercises.AddRange([firstExercise, secondExercise]);

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

    public List<Lesson> GetAll()
    {
        return _lessons;
    }
}
