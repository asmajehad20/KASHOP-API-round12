using KSHOP.DAL.Data;
using KSHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<TUser> _userManager;
        public CategoryRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category request)
        {
            try
            {
                await _context.AddAsync(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
            
            return request;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Translations).Include(c => c.User).ToListAsync();
           
        }

        public async Task<Category?> FindbyIdAsync(int id)
        {
            return await _context.Categories.Include(c=>c.Translations).
                FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
