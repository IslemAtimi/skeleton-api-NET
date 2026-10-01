using System;
using System.Net.NetworkInformation;
using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AB.Ecommerce.Gros.Data
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetQuery()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            var category = await GetQuery().Include(c=>c.Image).SingleOrDefaultAsync(c=>c.Id==id);
            return category;
        }


        public async Task<Category> InsertAsync(Category category)
        {
            var entity = _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

         

        public async Task<List<Category>> GetListAsync(string? filter = null, bool? isVisible = null, CategoryType? type=null)
        {
            var query = _context.Categories.AsQueryable();
            query = ApplyFilter(query, filter, isVisible,type );
            query = query.OrderBy(e => e.Label);
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(string? filter = null, bool ? isVisible = null, CategoryType? type = null)
        {
            var query = _context.Categories.AsQueryable();
            query = ApplyFilter(query, filter, isVisible,type);
            return await query.CountAsync();
        }

        public async Task<List<Category>> GetListAsync(Guid[] ids)
        {
            var query = _context.Categories.AsQueryable();
            query = query.Where(p => ids.Contains(p.Id));
            return await query.ToListAsync();
        }

        private IQueryable<Category> ApplyFilter(IQueryable<Category> query, string? filter = null, bool? isVisible = null,CategoryType? type=null)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var search = filter.ToLower();
                query = query.Where(c => c.Label.ToLower().Contains(search));
            }
            
            if (isVisible.HasValue)
            {
                query = query.Where(c => c.Visible == isVisible.Value);
            }
            return query;
        }

         

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryImage> GetImageAsync(Guid categoryId)
        {
            var query = _context.CategoryImages.AsQueryable();
            query = query.Where(f => f.CategoryId == categoryId);
            return await query.FirstAsync();
        }
    }
}

