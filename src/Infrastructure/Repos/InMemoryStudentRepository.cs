using CampusBorrowing.Domain;
using CampusBorrowing.Application.Interfaces;

namespace CampusBorrowing.Infrastructure.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new()
    {
        new Student(1, "Juan Dela Cruz"),
        new Student(2, "Maria Santos")
    };

    public Task<Student?> GetStudentByIdAsync(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(student);
    }
}
