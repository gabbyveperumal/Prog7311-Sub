using GLMS_Project.Data;
using GLMS_Project.Models;
using Microsoft.Extensions.Logging;

namespace GLMS_Project.Services
{
    public class ServiceRequestService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ServiceRequestService> _logger;

        public ServiceRequestService(AppDbContext context, ILogger<ServiceRequestService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<(bool Success, string Message)> CreateServiceRequestAsync(ServiceRequest request, string currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogWarning("Access Denied: Unauthenticated user attempted to create a service request.");
                return (false, "You must be logged in to create a service request.");
            }

            _logger.LogInformation("User {UserId} is attempting to create service request for Contract ID: {ContractId}", currentUserId, request.ContractId);

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == request.ContractId);

            if (contract == null)
            {
                _logger.LogWarning("Creation Failed: Contract ID {ContractId} does not exist.", request.ContractId);
                return (false, "Contract not found.");
            }

            if (contract.Status == ContractStatus.Expired || contract.EndDate < DateTime.Now)
            {
                _logger.LogWarning("Business Rule Violation: Contract ID {ContractId} is Expired/Inactive.", request.ContractId);
                return (false, "Cannot create a service request for an expired or inactive contract.");
            }

            try
            {
                request.Status = ServiceRequestStatus.Pending;
                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created Service Request ID: {RequestId} by User: {UserId}", request.Id, currentUserId);
                return (true, "Service request created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database Failure for Contract ID: {ContractId}", request.ContractId);
                return (false, $"An error occurred while saving: {ex.Message}");
            }
        }

        public async Task<bool> ProcessServiceRequestAsync(int contractId, string userRole)
        {
            if (userRole != "Admin" && userRole != "Manager")
            {
                _logger.LogWarning("Unauthorized workflow transition attempt with role: {Role}", userRole);
                return false; 
            }

            _logger.LogInformation("Processing workflow transition for Contract ID: {ContractId} by Role: {Role}", contractId, userRole);

            try
            {
                var contract = await _context.Contracts.FindAsync(contractId);

                if (contract == null || contract.Status == ContractStatus.Expired)
                {
                    _logger.LogWarning("Workflow Processing Failed or Contract is Expired for ID {ContractId}.", contractId);
                    return false;
                }

                ContractStatus oldStatus = contract.Status;
                contract.Status = ContractStatus.PendingReview;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical Exception occurred for Contract ID: {ContractId}", contractId);
                throw;
            }
        }

        public async Task<IEnumerable<ServiceRequest>> GetRequestsByContractIdAsync(int contractId)
        {
            return await _context.ServiceRequests
                .Where(sr => sr.ContractId == contractId)
                .ToListAsync();
        }
    }
}