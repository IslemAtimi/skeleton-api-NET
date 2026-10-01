using System;

namespace AB.Ecommerce.Gros.Application
{
    public interface ICategoryAppService
    {

        public Task<CategoryDto> GetAsync(Guid id);
        public Task<List<CategoryDto>> GetListAsync(CategoriesGetListInput input);
        public Task<CategoryDto?> CreateAsync(CategoryCreateOrUpdateDto input);
        public Task<CategoryDto?> UpdateAsync(Guid id, CategoryCreateOrUpdateDto input);
        public Task DeleteAsync(Guid id);

        public Task<FileContent> GetImageAsync(Guid id);

    }


}

