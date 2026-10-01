using System;
namespace AB.Ecommerce.Gros.Business
{
	public interface IClientRepository
	{

        Task<List<Client>> GetListAsync(string? filter = null,
                            bool? isActive = null,
                            string? sorting = null,
                            int? skipCount = null,
                            int? maxResultCount = null);

        Task<int> CountAsync(string? filter = null, bool? isActive = null);

        Task<List<Client>> GetListAsync(Guid[] ids);

        Task<Client?> GetByIdAsync(Guid id); 
		Task<Client> InsertAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task DeleteAsync(Client client);

        Task<List<ClientFile>> GetFilesAsync(Guid clientId, int[]? filesIds = null, bool? withContent = false);


    }
}

