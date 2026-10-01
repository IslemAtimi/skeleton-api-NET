using AB.Ecommerce.Gros.Application.Dtos;
using System;

namespace AB.Ecommerce.Gros.Application
{
    public interface IClientAppService
    {


        public Task<ClientDto> GetAsync(Guid id);
        public Task<ClienWithPasswordtDto> GetWithPasswordAsync(Guid id);
        public Task<PagedResult<ClientDto>> GetListAsync(ClientGetListInput input);
        public Task<PagedResult<ClienWithPasswordtDto>> GetListWithPasswordAsync(ClientGetListInput input);
        public Task<ClientDto> SetClientLockAsync(Guid id, bool locked);

        public Task<ClientDto> SetRoleAsync(Guid id, string roleId);


        public Task<ClientDto> RegisterAsync(RegisterInput input);

        public Task<List<FileDto>> GetFilesAsync(Guid id);
        public Task<FileDto> GetFileAsync(Guid id, int fileId);
        public Task<FileContent> DownloadFileAsync(Guid id, int fileId);


        public Task DeleteAsync(Guid id);


        /*
        public Task<ClientDto> AddFileAsync(Guid id, FileInput file);
        public Task DeleteFileAsync(Guid id, int fileId);*/

    }
}

