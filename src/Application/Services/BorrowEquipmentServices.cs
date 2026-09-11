using CampusBorrowing.Domain;
using CampusBorrowing.Application.Interfaces;

namespace CampusBorrowing.Application.Services;

public class BorrowEquipmentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IBorrowingRepository _borrowingRepository;
    private const int MaxActiveBorrowings = 3;

    public BorrowEquipmentService(
        IStudentRepository studentRepository,
        IEquipmentRepository equipmentRepository,
        IBorrowingRepository borrowingRepository)
    {
        _studentRepository = studentRepository;
        _equipmentRepository = equipmentRepository;
        _borrowingRepository = borrowingRepository;
    }

    public async Task<BorrowEquipmentResult> BorrowAsync(int studentId, int equipmentId, DateTime expectedReturnDate)
    {
        var student = await _studentRepository.GetStudentByIdAsync(studentId);
        if (student is null)
            return BorrowEquipmentResult.Failed("Student does not exist.");

        if (!student.IsAllowedToBorrow)
            return BorrowEquipmentResult.Failed("Student is not currently allowed to borrow.");

        var equipment = await _equipmentRepository.GetEquipmentAsync(equipmentId);
        if (equipment is null)
            return BorrowEquipmentResult.Failed("Equipment does not exist.");

        if (!equipment.Availability)
            return BorrowEquipmentResult.Failed("Equipment is not available.");

        var activeBorrowings = await _borrowingRepository.GetActiveBorrowingByStudentAsync(studentId);
        if (activeBorrowings.Count >= MaxActiveBorrowings)
            return BorrowEquipmentResult.Failed("Student has reached the maximum number of active borrowings.");

        var borrowing = new Borrowing(
            id: new Random().Next(1000, 999999),
            studentId: studentId,
            equipmentId: equipmentId,
            borrowDate: DateTime.Now,
            returnDate: expectedReturnDate);

        equipment.Borrowed();
        await _borrowingRepository.AddBorrowingAsync(borrowing);

        return BorrowEquipmentResult.Succeeded(borrowing);
    }
}
