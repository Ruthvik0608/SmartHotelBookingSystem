using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class ReviewsRepository
    {
        private readonly AppDbContext _context;

        public ReviewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewID == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
        }

        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public double CalculateAverageRating(int hotelID)
        {
            var reviews = _context.Reviews.Where(r => r.HotelID == hotelID);
            if (reviews.Any())
            {
                return reviews.Average(r => r.Rating);
            }
            return 0;
        }

        public List<Review> GetMostRecentReviews()
        {
            return _context.Reviews.OrderByDescending(r => r.Timestamp).ToList();
        }

        public List<Review> GetReviewsByMonthAndDate(int month, int day)
        {
            return _context.Reviews.Where(r => r.Timestamp.Month == month && r.Timestamp.Day == day).ToList();
        }

        public void FlagInappropriateReview(int reviewID)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewID == reviewID);
            if (review != null)
            {
                review.IsFlagged = true; // Mark the review as flagged
                _context.SaveChanges();
            }
        }
    }
}