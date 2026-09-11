using CampusBorrowing.Domain;
using CampusBorrowing.Application.Interfaces;

namespace CampusBorrowing.Infrastructure.Repositories;

public class InMemoryBorrowingRepository : IBorrowingRepository
{
    private readonly List<Borrowing> _borrowings = new();

    public Task<Borrowing?> GetBorrowingAsync(int id)
    {
        var borrowing = _borrowings.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(borrowing);
    }

    public Task<List<Borrowing>> GetActiveBorrowingByStudentAsync(int studentId)
    {
        var active = _borrowings
            .Where(b => b.StudentId == studentId && b.Status == BorrowingStatus.Active)
            .ToList();
        return Task.FromResult(active);
    }

    public Task AddBorrowingAsync(Borrowing borrowing)
    {
        _borrowings.Add(borrowing);
        return Task.CompletedTask;
    }
}
