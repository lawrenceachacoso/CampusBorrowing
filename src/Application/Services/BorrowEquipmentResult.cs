using CampusBorrowing.Domain;

namespace CampusBorrowing.Application.Services;

public class BorrowEquipmentResult
{
    public bool Success { get; }
    public string? FailureReason { get; }
    public Borrowing? Borrowing { get; }

    private BorrowEquipmentResult(bool success, string? failureReason, Borrowing? borrowing)
    {
        Success = success;
        FailureReason = failureReason;
        Borrowing = borrowing;
    }

    public static BorrowEquipmentResult Succeeded(Borrowing borrowing) =>
        new(true, null, borrowing);

    public static BorrowEquipmentResult Failed(string reason) =>
        new(false, reason, null);
}
