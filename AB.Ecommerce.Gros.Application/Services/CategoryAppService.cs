using System;
using AB.Ecommerce.Gros.Business;
using AutoMapper;
using static System.Net.Mime.MediaTypeNames;

namespace AB.Ecommerce.Gros.Application
{
    public class CategoryAppService : BaseAppService, ICategoryAppService
    {

        private readonly ICategoryRepository _categoryRepository;


        public CategoryAppService( ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        


        public async Task<CategoryDto?> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                return await MapCategoryAsync(category);
            }
            return null;
        }


       
        public async Task<List<CategoryDto>> GetListAsync(CategoriesGetListInput input)
        {
            var categories = await _categoryRepository.GetListAsync(input.Filter, input.Visible , input.CategoryType);
            return await MapCategoryAsync(categories);
        }

        public async Task<CategoryDto?> CreateAsync(CategoryCreateOrUpdateDto input)
        {
            var categoryId = Guid.NewGuid();
            var image = new CategoryImage(categoryId, input.Image.MimeType, input.Image.Content);
            var category = new Category(categoryId, input.Label, input.Visible,input.Description,input.Histoire, input.Type ,DateTime.UtcNow, image);
            category = await _categoryRepository.InsertAsync(category);
            return await MapCategoryAsync(category);
        }

        public async Task<CategoryDto?> UpdateAsync(Guid id, CategoryCreateOrUpdateDto input)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
           
            if (category != null)
            {
                var image = new CategoryImage(category.Id, input.Image.MimeType, input.Image.Content);

                category.Image = image;
                category.Label = input.Label;
                category.Visible = input.Visible;

                category = await _categoryRepository.UpdateAsync(category);
                return await MapCategoryAsync(category);
            }
            return null;
        } 
        

        public async Task DeleteAsync(Guid id)
        {
            var categorie = await _categoryRepository.GetByIdAsync(id);
            if (categorie != null)
            {
               await _categoryRepository.DeleteAsync(categorie);
            }
        }

        public async Task<FileContent> GetImageAsync(Guid id)
        {
            var image = await _categoryRepository.GetImageAsync(id);
            return new FileContent
            {
                Content = image.Content,
                MimeType = image.MimeType
            };
        }

        private async Task<CategoryDto> MapCategoryAsync(Category category)
        {
            var count = 0;
            var dto = Mapper.Map<Category, CategoryDto>(category);
            dto.SetProductCount(count);
            return dto;
        }

        private async Task<List<CategoryDto>> MapCategoryAsync(List<Category> categories)
        {
            var dtos = new List<CategoryDto>();
            foreach (var category in categories)
            {
                dtos.Add(await MapCategoryAsync(category));
            }
            return dtos; 
        }
    }
    
}

