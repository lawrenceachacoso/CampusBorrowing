using CampusBorrowing.Domain;

namespace CampusBorrowing.Application.Services;

public class ReturnEquipmentResult
{
    public bool Success { get; }
    public string? FailureReason { get; }
    public Borrowing? Borrowing { get; }

    private ReturnEquipmentResult(bool success, string? failureReason, Borrowing? borrowing)
    {
        Success = success;
        FailureReason = failureReason;
        Borrowing = borrowing;
    }

    public static ReturnEquipmentResult Succeeded(Borrowing borrowing) =>
        new(true, null, borrowing);

    public static ReturnEquipmentResult Failed(string reason) =>
        new(false, reason, null);
}