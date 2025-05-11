using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MallManagmentSystem.Services
{
    public class MallService : IMallService
    {
        private readonly MallDBContext _context;

        public MallService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<List<Mall>> GetAllMallsAsync()
        {
            return await _context.Malls
                .Include(m => m.Stores)
                .Include(m => m.Employees)
                .ToListAsync();
        }

        public async Task<Mall> GetMallByIdAsync(int id)
        {
            return await _context.Malls
                .Include(m => m.Stores)
                .Include(m => m.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> CreateMallAsync(Mall mall)
        {
            try
            {
                // Initialize collections
                mall.Stores = new List<Store>();
                mall.Employees = new List<Employee>();

                await _context.Malls.AddAsync(mall);
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

        public async Task<bool> UpdateMallAsync(int id, Mall mall)
        {
            try
            {
                var existingMall = await _context.Malls.FindAsync(id);
                if (existingMall == null)
                    return false;

                _context.Entry(existingMall).CurrentValues.SetValues(mall);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingMall);
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

        public async Task<bool> DeleteMallAsync(int id)
        {
            try
            {
                var mall = await _context.Malls
                    .Include(m => m.Stores)
                    .Include(m => m.Employees)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (mall == null)
                    return false;

                // Check if mall has active stores or employees
                if (mall.Stores.Any() || mall.Employees.Any())
                    return false;

                _context.Malls.Remove(mall);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(mall);
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

        public async Task<bool> UpdateMallStatisticsAsync(int mallId)
        {
            try
            {
                var mall = await _context.Malls
                    .Include(m => m.Stores)
                    .Include(m => m.Employees)
                    .FirstOrDefaultAsync(m => m.Id == mallId);

                if (mall == null)
                    return false;

                // Update statistics
                mall.NumberOfStores = mall.Stores.Count;
                mall.NumberOfActiveStores = mall.Stores.Count(s => s.IsActive == true);
                mall.NumberOfEmployees = mall.Employees.Count;

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(mall);
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
    }
}
