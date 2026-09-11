using CampusBorrowing.Application.Services;
using CampusBorrowing.Infrastructure.Repositories;

var studentRepository = new InMemoryStudentRepository();
var equipmentRepository = new InMemoryEquipmentRepository();
var borrowingRepository = new InMemoryBorrowingRepository();

var borrowService = new BorrowEquipmentService(studentRepository, equipmentRepository, borrowingRepository);
var returnService = new ReturnEquipmentService(borrowingRepository, equipmentRepository);

Console.WriteLine("=== Campus Equipment Borrowing System Demo ===\n");

Console.WriteLine("Attempt 1: Student 1 borrows Equipment 1 (should succeed)");
var result1 = await borrowService.BorrowAsync(studentId: 1, equipmentId: 1, expectedReturnDate: DateTime.Now.AddDays(7));
PrintBorrowResult(result1);

Console.WriteLine("\nAttempt 2: Student 2 tries to borrow Equipment 1 again (should fail - unavailable)");
var result2 = await borrowService.BorrowAsync(studentId: 2, equipmentId: 1, expectedReturnDate: DateTime.Now.AddDays(7));
PrintBorrowResult(result2);

Console.WriteLine("\nAttempt 3: Student 1 tries to borrow Equipment 999 (should fail - does not exist)");
var result3 = await borrowService.BorrowAsync(studentId: 1, equipmentId: 999, expectedReturnDate: DateTime.Now.AddDays(7));
PrintBorrowResult(result3);

Console.WriteLine($"\nAttempt 4: Return borrowing {result1.Borrowing!.Id} (should succeed)");
var result4 = await returnService.ReturnAsync(result1.Borrowing.Id);
PrintReturnResult(result4);

Console.WriteLine($"\nAttempt 5: Try returning borrowing {result1.Borrowing.Id} again (should fail - already returned)");
var result5 = await returnService.ReturnAsync(result1.Borrowing.Id);
PrintReturnResult(result5);

static void PrintBorrowResult(BorrowEquipmentResult result)
{
    if (result.Success)
        Console.WriteLine($"  SUCCESS - Borrowing ID: {result.Borrowing!.Id}, Status: {result.Borrowing.Status}");
    else
        Console.WriteLine($"  FAILED - Reason: {result.FailureReason}");
}

static void PrintReturnResult(ReturnEquipmentResult result)
{
    if (result.Success)
        Console.WriteLine($"  SUCCESS - Borrowing ID: {result.Borrowing!.Id}, Status: {result.Borrowing.Status}");
    else
        Console.WriteLine($"  FAILED - Reason: {result.FailureReason}");
}
