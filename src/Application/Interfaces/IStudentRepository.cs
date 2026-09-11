using CampusBorrowing.Domain;
namespace CampusBorrowing.Application.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetStudentByIdAsync(int id);

}
