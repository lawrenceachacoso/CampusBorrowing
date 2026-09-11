using CampusBorrowing.Domain;
namespace CampusBorrowing.Application.Interfaces;
public interface IBorrowingRepository
{
    Task<Borrowing?> GetBorrowingAsync(int id);
    Task<List<Borrowing>> GetActiveBorrowingByStudentAsync(int studentId);
    Task AddBorrowingAsync(Borrowing borrowing);

}
