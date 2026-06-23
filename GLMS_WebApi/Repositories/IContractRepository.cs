using GLMS.API.Models;
using GLMS_Project.Models;
namespace GLMS.API.Repositories
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetFilteredAsync(
            string? status, string? clientName, int page, int size);
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract> AddAsync(Contract contract);
        Task<Contract?> UpdateStatusAsync(int id, string status);
    }
}