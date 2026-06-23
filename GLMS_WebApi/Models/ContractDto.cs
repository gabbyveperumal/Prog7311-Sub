namespace GLMS.API.Models;

public record ContractDto(
    int Id,
    string Title,
    string ClientName,
    decimal Value,
    string Currency,
    string Status,
    DateTime CreatedAt
);

public record CreateContractRequest(
    string Title,
    string ClientName,
    decimal Value,
    string Currency          // "USD", "ZAR"
);

public record UpdateStatusRequest(
    string Status             // "Approved / Declined"
);