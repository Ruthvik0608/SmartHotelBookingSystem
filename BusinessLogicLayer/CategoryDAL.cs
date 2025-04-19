using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class CategoryDAL
    {
        private readonly AppDbContext _context;

        public CategoryDAL(AppDbContext context)
        {
            _context = context;
        }

        public void AddCategory(CategoryModel newCategory)
        {
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
        }

        public List<CategoryModel> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public List<CategoryModel> GetCategoriesByName(string categoryName)
        {
            var categories = _context.Categories.Where(c => c.Category == categoryName).ToList();
            foreach (var res in categories)
            {
                Console.WriteLine(res.ToString());
            }
            return categories;
        }

        public void UpdateCategory(int categoryId, string newCategory, DateTime newActivateDate, DateTime? newDeactivateDate, string newRemarks)
        {
            var category = _context.Categories.Find(categoryId);
            if (category != null)
            {
                category.Category = newCategory;
                category.CatActivateDate = newActivateDate;
                category.CatDeactivateDate = newDeactivateDate;
                category.Remarks = newRemarks;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }

        public void DeleteCategory(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }
    }
}