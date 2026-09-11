using CampusBorrowing.Domain;
using CampusBorrowing.Application.Interfaces;

namespace CampusBorrowing.Infrastructure.Repositories;

public class InMemoryEquipmentRepository : IEquipmentRepository
{
    private readonly List<Equipment> _equipment = new()
    {
        new Equipment(1, "Digital Multimeter"),
        new Equipment(2, "Oscilloscope"),
        new Equipment(3, "Soldering Iron")
    };

    public Task<Equipment?> GetEquipmentAsync(int id)
    {
        var equipment = _equipment.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(equipment);
    }

    public Task<List<Equipment>> GetAllAsync()
    {
        return Task.FromResult(_equipment);
    }
}
