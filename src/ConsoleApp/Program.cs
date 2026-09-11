using CampusBorrowing.Application.Services;
using CampusBorrowing.Infrastructure.Repositories;

var studentRepository = new InMemoryStudentRepository();
var equipmentRepository = new InMemoryEquipmentRepository();
var borrowingRepository = new InMemoryBorrowingRepository();

var borrowService = new BorrowEquipmentService(studentRepository, equipmentRepository, borrowingRepository);

Console.WriteLine("=== Campus Equipment Borrowing System Demo ===\n");

// Success case: valid student, valid available equipment
Console.WriteLine("Attempt 1: Student 1 borrows Equipment 1 (should succeed)");
var result1 = await borrowService.BorrowAsync(studentId: 1, equipmentId: 1, expectedReturnDate: DateTime.Now.AddDays(7));
PrintResult(result1);

// Failure case: equipment already borrowed (Equipment 1 is now unavailable)
Console.WriteLine("\nAttempt 2: Student 2 tries to borrow Equipment 1 again (should fail - unavailable)");
var result2 = await borrowService.BorrowAsync(studentId: 2, equipmentId: 1, expectedReturnDate: DateTime.Now.AddDays(7));
PrintResult(result2);

// Failure case: equipment does not exist
Console.WriteLine("\nAttempt 3: Student 1 tries to borrow Equipment 999 (should fail - does not exist)");
var result3 = await borrowService.BorrowAsync(studentId: 1, equipmentId: 999, expectedReturnDate: DateTime.Now.AddDays(7));
PrintResult(result3);

static void PrintResult(BorrowEquipmentResult result)
{
    if (result.Success)
    {
        Console.WriteLine($"  SUCCESS - Borrowing ID: {result.Borrowing!.Id}, Status: {result.Borrowing.Status}");
    }
    else
    {
        Console.WriteLine($"  FAILED - Reason: {result.FailureReason}");
    }
}
