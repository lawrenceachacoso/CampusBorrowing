using CampusBorrowing.Domain;
namespace CampusBorrowing.Application.Interfaces;
public interface IEquipmentRepository
{
    Task<IEquipmentRepository> GetEquipmentAsync(int id);
    Task<List<Equipment>> GetAllAsync();
}
