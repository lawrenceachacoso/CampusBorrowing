using CampusBorrowing.Domain;
using CampusBorrowing.Application.Interfaces;

namespace CampusBorrowing.Application.Services;

public class ReturnEquipmentService
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public ReturnEquipmentService(
        IBorrowingRepository borrowingRepository,
        IEquipmentRepository equipmentRepository)
    {
        _borrowingRepository = borrowingRepository;
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ReturnEquipmentResult> ReturnAsync(int borrowingId)
    {
        var borrowing = await _borrowingRepository.GetBorrowingAsync(borrowingId);
        if (borrowing is null)
            return ReturnEquipmentResult.Failed("Borrowing record not found.");

        if (borrowing.Status == BorrowingStatus.Returned)
            return ReturnEquipmentResult.Failed("This borrowing has already been returned.");

        var equipment = await _equipmentRepository.GetEquipmentAsync(borrowing.EquipmentId);
        if (equipment is null)
            return ReturnEquipmentResult.Failed("Associated equipment could not be found.");

        borrowing.Return();
        equipment.Available();

        return ReturnEquipmentResult.Succeeded(borrowing);
    }
}
