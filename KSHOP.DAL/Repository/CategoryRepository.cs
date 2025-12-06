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

        public Category Create(Category request)
        {
            _context.Add(request);
            _context.SaveChanges();
            return request;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Translations).ToList();
        }
    }
}
