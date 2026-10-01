using AB.Ecommerce.Gros.Shared;
using System;
namespace AB.Ecommerce.Gros.Business
{
	public interface ICategoryRepository
	{

        Task<List<Category>> GetListAsync(string? filter = null, bool? isVisible = null,CategoryType? type=null);

        Task<int> CountAsync(string? filter = null, bool? isVisible = null, CategoryType? type = null);

        Task<List<Category>> GetListAsync(Guid[] ids);


        Task<Category?> GetByIdAsync(Guid id); 
		Task<Category> InsertAsync(Category client);
        Task<Category> UpdateAsync(Category client);
        Task DeleteAsync(Category client);

        Task<CategoryImage> GetImageAsync(Guid categoryId);


    }
}

