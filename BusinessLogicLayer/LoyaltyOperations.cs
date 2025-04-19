using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    #region Loyalty

    public class LoyaltyDataOperations
    {
        private readonly AppDbContext _context;

        public LoyaltyDataOperations(AppDbContext context)
        {
            _context = context;
        }

        // Create a new LoyaltyAccount
        public void AddLoyaltyAccount(LoyaltyAccount loyaltyAccount)
        {
            _context.LoyaltyAccounts.Add(loyaltyAccount);
            _context.SaveChanges();
        }

        // Read all LoyaltyAccounts
        public List<LoyaltyAccount> GetAllLoyaltyAccounts()
        {
            return _context.LoyaltyAccounts.Where(l => l.IsActive).ToList();
        }

        // Read a LoyaltyAccount by ID
        public LoyaltyAccount GetLoyaltyAccountById(int loyaltyId)
        {
            return _context.LoyaltyAccounts.FirstOrDefault(l => l.LoyaltyID == loyaltyId && l.IsActive);
        }

        // Update a LoyaltyAccount by UserID
        public void UpdateLoyaltyAccountByUserId(int userId, int newPointsBalance, bool isActive)
        {
            var account = _context.LoyaltyAccounts.FirstOrDefault(l => l.UserID == userId);
            if (account != null)
            {
                account.PointsBalance = newPointsBalance;
                account.LastUpdated = DateTime.UtcNow;
                account.IsActive = isActive;
                _context.SaveChanges();
            }
        }

        // Mark a LoyaltyAccount as inactive
        public bool DeleteLoyaltyAccount(int loyaltyId)
        {
            var account = _context.LoyaltyAccounts.Find(loyaltyId);
            if (account != null)
            {
                account.IsActive = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
    #endregion
}