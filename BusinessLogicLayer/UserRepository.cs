using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserAccount> ValidateUser(string email, string password)
        {
            try
            {
                return await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user.");
                throw;
            }
        }

        public async Task AddUsers(UserAccount newUser)
        {
            try
            {
                await _context.UserAccounts.AddAsync(newUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user.");
                throw;
            }
        }

        public async Task<List<UserAccount>> GetAllUsers()
        {
            try
            {
                return await _context.UserAccounts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users.");
                throw;
            }
        }

        public async Task<List<UserAccount>> GetUsersByName(string userName)
        {
            try
            {
                return await _context.UserAccounts.Where(a => a.Name.Contains(userName)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users by name.");
                throw;
            }
        }

        public async Task UpdateUser(int userId, string newName, string newEmail, string newContact)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user != null)
                {
                    user.Name = newName;
                    user.Email = newEmail;
                    user.ContactNumber = newContact;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User updated successfully.");
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user.");
                throw;
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user != null)
                {
                    _context.UserAccounts.Remove(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User deleted successfully.");
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                throw;
            }
        }

        public async Task<UserAccount> GetUserByEmail(string email)
        {
            try
            {
                return await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email.");
                throw;
            }
        }

        public async Task<int> GetTotalUser()
        {
            try
            {
                return await _context.UserAccounts.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total users count.");
                throw;
            }
        }

        public async Task<List<UserAccount>> GetUsersByRole(string role)
        {
            try
            {
                return await _context.UserAccounts.Where(u => u.Role == role).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users by role.");
                throw;
            }
        }
    }
}
