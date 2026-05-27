using FurudType.Core.Models;

namespace FurudType.Core.Repositories;

public interface ILessonRepository
{
    List<Lesson> GetAll();
}
