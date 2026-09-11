using CampusBorrowing.Domain;
namespace CampusBorrowing.Application.Interfaces;
public interface IEquipmentRepository
{
    Task<Equipment?> GetEquipmentAsync(int id);
    Task<List<Equipment>> GetAllAsync();
}
