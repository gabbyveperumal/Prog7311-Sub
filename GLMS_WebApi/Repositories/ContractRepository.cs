using GLMS.API.Data;
using GLMS.API.Models;
namespace GLMS.API.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using GLMS.API.Data;
public class ContractRepository : IContractRepository
    {
        private readonly AppDbContext _db;

        public ContractRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Contract>> GetFilteredAsync(
            string? status, string? clientName, int page, int size)
        {
            var query = _db.Contracts.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(clientName))
                query = query.Where(c => c.ClientName.Contains(clientName));

            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id) =>
            await _db.Contracts.FindAsync(id);

        public async Task<Contract> AddAsync(Contract contract)
        {
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract?> UpdateStatusAsync(int id, string status)
        {
            var contract = await _db.Contracts.FindAsync(id);
            if (contract is null) return null;
            contract.Status = status;
            await _db.SaveChangesAsync();
            return contract;
        }
    }
