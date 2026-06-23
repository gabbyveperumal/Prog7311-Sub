using GLMS.API.Data;
using GLMS.API.Models;
using GLMS.API.Repositories;
using GLMS_Project.Factories;
using GLMS_Project.Services.Currency;

namespace GLMS.API.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _repo;
    private readonly IContractFactory _factory;        
    private readonly CurrencyConversionService _currency;   

    public ContractService(
        IContractRepository repo,
        IContractFactory factory,
        CurrencyConversionService currency)
    {
        _repo = repo;
        _factory = factory;
        _currency = currency;
    }

    public async Task<IEnumerable<ContractDto>> GetFilteredAsync(
        string? status, string? clientName, int page, int size)
    {
        var contracts = await _repo.GetFilteredAsync(status, clientName, page, size);
        return contracts.Select(ToDto);
    }

    public async Task<ContractDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<ContractDto> CreateAsync(CreateContractRequest request)
    {
        var contract = _factory.Create(request.Title, request.ClientName,
                                       request.Value, request.Currency);

        contract.Value = await _currency.ConvertToBaseAsync(
            request.Value, request.Currency);

        var saved = await _repo.AddAsync(contract);
        return ToDto(saved);
    }

    public async Task<ContractDto?> UpdateStatusAsync(int id, string status)
    {
        var updated = await _repo.UpdateStatusAsync(id, status);
        return updated is null ? null : ToDto(updated);
    }

    private static ContractDto ToDto(Contract c) => new(
        c.Id, c.Title, c.ClientName, c.Value, c.Currency, c.Status, c.CreatedAt);
}