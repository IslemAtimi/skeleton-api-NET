using System.Net.Sockets;
using System.Threading.Tasks;
using AB.Ecommerce.Gros.Application;
using AB.Ecommerce.Gros.Application.Dtos;
using AB.Ecommerce.Gros.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AB.Ecommerce.Gros;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase, IClientAppService
{

    private readonly IClientAppService ClientAppService;

    public ClientController(IClientAppService clientAppService)
    {
        ClientAppService = clientAppService;
    }

    

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ClientDto> RegisterAsync([FromForm] RegisterInput input)
    {
        var files = HttpContext.Request.Form.Files;
        if (files.Count()==input.Files?.Count())
        {
            foreach (var file in files)
            {
                var fileInput = input.Files.FirstOrDefault(f => f.Name == file.FileName);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileInput.Content = memoryStream.ToArray();
                    fileInput.MimeType = file.ContentType;

                } 
                
            }
        }
        return await ClientAppService.RegisterAsync(input);
    }


    [HttpGet]
    [Route("{id}/files")]
    public async Task<List<FileDto>> GetFilesAsync(Guid id)
    {
        return await ClientAppService.GetFilesAsync(id);
    }

    [HttpGet]
    [Route("{id}/files/{fileId}")]
    public async Task<FileDto> GetFileAsync(Guid id, int fileId)
    {
        return await ClientAppService.GetFileAsync(id, fileId);
    }

    [HttpGet]
    [Route("{id}/files/{fileId}/download")]
    public async Task<FileContent> DownloadFileAsync(Guid id, int fileId)
    {
        return await ClientAppService.DownloadFileAsync(id, fileId);
    }

    [HttpGet]
    [Route("")]
    public async Task<PagedResult<ClientDto>> GetListAsync([FromQuery] ClientGetListInput input)
    {
        return await ClientAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("with-password")]
    public async Task<PagedResult<ClienWithPasswordtDto>> GetListWithPasswordAsync([FromQuery] ClientGetListInput input)
    {
        return await ClientAppService.GetListWithPasswordAsync(input);
    }

    [HttpPut]
    [Route("{id}/set-lock/{locked}")]
    public async Task<ClientDto> SetClientLockAsync(Guid id, bool locked)
    {
        return await ClientAppService.SetClientLockAsync(id, locked);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ClientDto> GetAsync(Guid id)
    {
        return await ClientAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("{id}/with-password")]
    public async Task<ClienWithPasswordtDto> GetWithPasswordAsync(Guid id)
    {
        return await ClientAppService.GetWithPasswordAsync(id);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await ClientAppService.DeleteAsync(id);
    }

    [HttpPut]
    [Route("{id}/role/{roleId}")]
    public async Task<ClientDto> SetRoleAsync(Guid id, string roleId)
    {
        return await ClientAppService.SetRoleAsync(id, roleId);
    }
}

