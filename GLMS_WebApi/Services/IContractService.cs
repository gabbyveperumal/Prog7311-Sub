using GLMS.API.Models;

namespace GLMS.API.Services;

public interface IContractService
{
    Task<IEnumerable<ContractDto>> GetFilteredAsync(
        string? status, string? clientName, int page, int size);
    Task<ContractDto?> GetByIdAsync(int id);
    Task<ContractDto> CreateAsync(CreateContractRequest request);
    Task<ContractDto?> UpdateStatusAsync(int id, string status);
}