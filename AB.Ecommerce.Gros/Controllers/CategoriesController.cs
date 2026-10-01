using AB.Ecommerce.Gros.Application;
using AB.Ecommerce.Gros.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace AB.Ecommerce.Gros;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase, ICategoryAppService
{

    private readonly ICategoryAppService CategoryAppService;

    public CategoryController(ICategoryAppService categoryAppService)
    {
        CategoryAppService = categoryAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<CategoryDto?> GetAsync(Guid id)
    {
        return await CategoryAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("")]
    public async Task<List<CategoryDto>> GetListAsync([FromQuery] CategoriesGetListInput input)
    {
        return await CategoryAppService.GetListAsync(input);
    }

    [HttpPost]
    [Route("")]
    public async Task<CategoryDto> CreateAsync([FromForm] CategoryCreateOrUpdateDto input)
    {
        var imageFile = HttpContext.Request.Form.Files.FirstOrDefault();
        if (imageFile != null)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                input.Image.Content = memoryStream.ToArray();
                input.Image.MimeType = imageFile.ContentType;

            }
        }
        return await CategoryAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<CategoryDto?> UpdateAsync(Guid id, [FromForm] CategoryCreateOrUpdateDto input)
    {
        var imageFile = HttpContext.Request.Form.Files.FirstOrDefault();
        if (imageFile!=null)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                input.Image.Content = memoryStream.ToArray();
                input.Image.MimeType = imageFile.ContentType;

            }
        }
        return await CategoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task DeleteAsync(Guid id)
    {
        await CategoryAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}/image")]
    public async Task<FileContent> GetImageAsync(Guid id)
    {
        return await CategoryAppService.GetImageAsync(id);
    }
     
}

