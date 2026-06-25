using System.Text.Json;

using FurudType.Core.Models;
using FurudType.Core.Repositories;

namespace FurudType.Storage.Repositories;

public class JsonLessonRepository : ILessonRepository
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly StorageSettings _storageSettings;

    public JsonLessonRepository(StorageSettings storageSettings)
    {
        _storageSettings = storageSettings;
    }

    public async Task<List<Lesson>> GetAllAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            List<Lesson> lessons = [];

            foreach(string file in Directory.EnumerateFiles(_storageSettings.DataPath, "*.json"))
            {
                string data = await File.ReadAllTextAsync(file);
                Lesson? lesson = JsonSerializer.Deserialize<Lesson>(data);
                if (lesson is not null)
                {
                    lessons.Add(lesson);
                }
            }

            return await Task.FromResult(lessons);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
