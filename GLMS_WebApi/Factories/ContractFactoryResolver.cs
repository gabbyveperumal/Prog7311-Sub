using GLMS.API.Data;

namespace GLMS.API.Factories;

public interface IContractFactory
{
    Contract Create(string title, string clientName, decimal value, string currency);
}

public class ContractFactoryResolver : IContractFactory
{
    public Contract Create(string title, string clientName,
                           decimal value, string currency) => new()
                           {
                               Title = title,
                               ClientName = clientName,
                               Value = value,
                               Currency = currency,
                               Status = "Pending",
                               CreatedAt = DateTime.UtcNow
                           };
}