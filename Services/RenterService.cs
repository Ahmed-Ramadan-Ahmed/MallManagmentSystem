using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MallManagmentSystem.Services
{
    public class RenterService : IRenterService
    {
        private readonly MallDBContext _context;

        public RenterService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<List<Renter>> GetAllRentersAsync()
        {
            return await _context.Renters
                .Include(r => r.Stores)
                .ToListAsync();
        }

        public async Task<Renter> GetRenterByIdAsync(int id)
        {
            return await _context.Renters
                .Include(r => r.Stores)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> CreateRenterAsync(Renter renter)
        {
            try
            {
                renter.CreatedAt = DateTime.UtcNow;
                renter.UpdatedAt = DateTime.UtcNow;
                await _context.Renters.AddAsync(renter);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRenterAsync(int id, Renter renter)
        {
            try
            {
                var existingRenter = await _context.Renters.FindAsync(id);
                if (existingRenter == null)
                    return false;

                renter.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingRenter).CurrentValues.SetValues(renter);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingRenter);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRenterAsync(int id)
        {
            try
            {
                var renter = await _context.Renters
                    .Include(r => r.Stores)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (renter == null)
                    return false;

                //// Check if renter has active stores, contracts, or penalties
                //if (renter.Stores.Any() || renter.Contracts.Any() || renter.Penalties.Any())
                //    return false;

                _context.Renters.Remove(renter);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(renter);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Store>> GetStoresByRenterIdAsync(int renterId)
        {
            return await _context.Stores
                .Include(s => s.Mall)
                .Include(s => s.Contracts)
                .Where(s => s.Renters.Any(r => r.Id == renterId))
                .ToListAsync();
        }

        public async Task<bool> AddStoreToRenterAsync(int renterId, int storeId)
        {
            try
            {
                var renter = await _context.Renters
                    .Include(r => r.Stores)
                    .FirstOrDefaultAsync(r => r.Id == renterId);

                var store = await _context.Stores.FindAsync(storeId);

                if (renter == null || store == null)
                    return false;

                renter.Stores.Add(store);
                renter.UpdatedAt = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(renter);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveStoreFromRenterAsync(int renterId, int storeId)
        {
            try
            {
                var renter = await _context.Renters
                    .Include(r => r.Stores)
                    .FirstOrDefaultAsync(r => r.Id == renterId);

                var store = await _context.Stores.FindAsync(storeId);

                if (renter == null || store == null)
                    return false;

                renter.Stores.Remove(store);
                renter.UpdatedAt = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(renter);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddRentContractAsync(int renterId, StoreRentContract contract)
        {
            try
            {
                var renter = await _context.Renters.FindAsync(renterId);
                if (renter == null)
                    return false;

                contract.RenterId = renterId;
                contract.StartDate = DateTime.UtcNow;
                contract.UpdatedAt = DateTime.UtcNow;

                await _context.StoreRentContracts.AddAsync(contract);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(contract);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRentContractAsync(int renterId, int contractId, StoreRentContract contract)
        {
            try
            {
                var existingContract = await _context.StoreRentContracts
                    .FirstOrDefaultAsync(c => c.Id == contractId && c.RenterId == renterId);

                if (existingContract == null)
                    return false;

                contract.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingContract).CurrentValues.SetValues(contract);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingContract);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<StoreRentContract>> GetRentContractsByRenterIdAsync(int renterId)
        {
            return await _context.StoreRentContracts
                .Include(c => c.Renter)
                .Include(c => c.Store)
                .Where(c => c.RenterId == renterId)
                .ToListAsync();
        }

        public async Task<bool> AddStorePenaltyAsync(int renterId, int storeId, StorePenalty penalty)
        {
            try
            {
                var renter = await _context.Renters.FindAsync(renterId);
                var store = await _context.Stores.FindAsync(storeId);

                if (renter == null || store == null)
                    return false;

                penalty.RenterId = renterId;
                penalty.StoreId = storeId;
                penalty.CreatedAt = DateTime.UtcNow;
                penalty.UpdatedAt = DateTime.UtcNow;

                await _context.StorePenalties.AddAsync(penalty);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(penalty);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveStorePenaltyAsync(int renterId, int storeId, int penaltyId)
        {
            try
            {
                var penalty = await _context.StorePenalties
                    .FirstOrDefaultAsync(p => p.Id == penaltyId && p.RenterId == renterId && p.StoreId == storeId);

                if (penalty == null)
                    return false;

                _context.StorePenalties.Remove(penalty);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(penalty);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<StorePenalty>> GetStorePenaltiesByRenterIdAsync(int renterId)
        {
            return await _context.StorePenalties
                .Include(p => p.Renter)
                .Include(p => p.Store)
                .Where(p => p.RenterId == renterId)
                .ToListAsync();
        }

        public async Task<bool> AddRenterPaymentAsync(int renterId, RentInvoice invoice)
        {
            try
            {
                var renter = await _context.Renters.FindAsync(renterId);
                if (renter == null)
                    return false;

                invoice.RenterId = renterId;
                invoice.InvoiceDate = DateTime.UtcNow;

                await _context.RentInvoices.AddAsync(invoice);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(invoice);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<RentInvoice>> GetRenterInvoicesByRenterIdAsync(int renterId, DateTime startDate, DateTime endDate)
        {
            return await _context.RentInvoices
                .Include(p => p.Renter)
                .Where(p => p.RenterId == renterId && p.DueDate >= startDate && p.DueDate <= endDate)
                .ToListAsync();
        }
    }
} 