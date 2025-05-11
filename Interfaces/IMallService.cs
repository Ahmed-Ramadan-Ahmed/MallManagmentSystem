using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IMallService
    {
        Task<List<Mall>> GetAllMallsAsync();
        Task<Mall> GetMallByIdAsync(int id);
        Task<bool> CreateMallAsync(Mall mall);
        Task<bool> UpdateMallAsync(int id, Mall mall);
        Task<bool> DeleteMallAsync(int id);
        Task<bool> UpdateMallStatisticsAsync(int mallId);
    }
}
