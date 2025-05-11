using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MallManagmentSystem.Services
{
    public class StoreService : IStoreService
    {
        private readonly MallDBContext _context;

        public StoreService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await _context.Stores
                .Include(s => s.Mall)
                .Include(s => s.Contracts)
                .Include(s => s.Renters)
                .ToListAsync();
        }

        public async Task<Store> GetStoreByIdAsync(int id)
        {
            return await _context.Stores
                .Include(s => s.Mall)
                .Include(s => s.Contracts)
                .Include(s => s.Renters)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> CreateStoreAsync(Store store)
        {
            try
            {
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                return false;
            }
            catch (DbUpdateException)
            {
                // Handle other database update errors
                return false;
            }
        }

        public async Task<bool> UpdateStoreAsync(int id, Store store)
        {
            try
            {
                var existingStore = await _context.Stores.FindAsync(id);
                if (existingStore == null)
                    return false;

                _context.Entry(existingStore).CurrentValues.SetValues(store);
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(existingStore);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was deleted
                        return false;
                    }

                    // Update the original values with the database values
                    entry.OriginalValues.SetValues(databaseValues);
                    
                    // Try to save again
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

        public async Task<bool> DeleteStoreAsync(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                    return false;

                _context.Stores.Remove(store);
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(store);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was already deleted
                        return true;
                    }

                    // Store was modified by another user
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Store>> GetStoresByMallIdAsync(int mallId)
        {
            return await _context.Stores
                .Include(s => s.Mall)
                .Include(s => s.Contracts)
                .Include(s => s.Renters)
                .Where(s => s.MallId == mallId)
                .ToListAsync();
        }

        public async Task<List<Store>> GetStoresByRenterIdAsync(int renterId)
        {
            return await _context.Stores
                .Include(s => s.Mall)
                .Include(s => s.Contracts)
                .Include(s => s.Renters)
                .Where(s => s.Renters.Any(r => r.Id == renterId))
                .ToListAsync();
        }

        public async Task<bool> AssignRenterToStoreAsync(int storeId, int renterId)
        {
            try
            {
                var store = await _context.Stores
                    .Include(s => s.Renters)
                    .FirstOrDefaultAsync(s => s.Id == storeId);
                
                var renter = await _context.Renters.FindAsync(renterId);
                
                if (store == null || renter == null)
                    return false;

                store.Renters.Add(renter);
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(store);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was deleted
                        return false;
                    }

                    // Update the original values and try again
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

        public async Task<bool> RemoveRenterFromStoreAsync(int storeId)
        {
            try
            {
                var store = await _context.Stores
                    .Include(s => s.Renters)
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store == null)
                    return false;

                store.Renters.Clear();
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(store);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was deleted
                        return false;
                    }

                    // Update the original values and try again
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

        public async Task<bool> AddPenaltyToStoreAsync(int storeId, StorePenalty penalty)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                    return false;

                penalty.StoreId = storeId;
                await _context.StorePenalties.AddAsync(penalty);
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(store);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was deleted
                        return false;
                    }

                    // Update the original values and try again
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

        public async Task<bool> RemovePenaltyFromStoreAsync(int storeId, int penaltyId)
        {
            try
            {
                var penalty = await _context.StorePenalties
                    .FirstOrDefaultAsync(p => p.Id == penaltyId && p.StoreId == storeId);

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
                    // Handle concurrency conflict
                    var entry = _context.Entry(penalty);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Penalty was already deleted
                        return true;
                    }

                    // Penalty was modified by another user
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<StorePenalty>> GetPenaltiesByStoreIdAsync(int storeId)
        {
            return await _context.StorePenalties
                .Include(p => p.Store)
                .Include(p => p.Renter)
                .Where(p => p.StoreId == storeId)
                .ToListAsync();
        }

        public async Task<bool> UpdatePenaltyAsync(int storeId, int penaltyId, StorePenalty penalty)
        {
            try
            {
                var existingPenalty = await _context.StorePenalties
                    .FirstOrDefaultAsync(p => p.Id == penaltyId && p.StoreId == storeId);

                if (existingPenalty == null)
                    return false;

                _context.Entry(existingPenalty).CurrentValues.SetValues(penalty);
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(existingPenalty);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Penalty was deleted
                        return false;
                    }

                    // Update the original values and try again
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

        public async Task<bool> AddStoreToMallAsync(int storeId, int mallId)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                var mall = await _context.Malls.FindAsync(mallId);

                if (store == null || mall == null)
                    return false;

                store.MallId = mallId;
                
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflict
                    var entry = _context.Entry(store);
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    
                    if (databaseValues == null)
                    {
                        // Store was deleted
                        return false;
                    }

                    // Update the original values and try again
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
    }
}    