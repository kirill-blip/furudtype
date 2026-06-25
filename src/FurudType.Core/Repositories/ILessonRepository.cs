using FurudType.Core.Models;

namespace FurudType.Core.Repositories;

public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync();
}
