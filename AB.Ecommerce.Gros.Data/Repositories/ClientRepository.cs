using System;
using System.Net.NetworkInformation;
using AB.Ecommerce.Gros.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AB.Ecommerce.Gros.Data
{
    public class ClientRepository : IClientRepository
    {

        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }


        public async Task<Client> InsertAsync(Client client)
        {
            var entity = _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            _context.Update(client);
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task DeleteAsync(Client client)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ClientFile>> GetFilesAsync(Guid clientId, int[]? filesIds = null, bool? withContent = false)
        {
            var query = _context.ClientFiles.AsQueryable();
            query = query.Where(f => f.ClientId == clientId);
            if (filesIds != null)
            {
                query = query.Where(f => filesIds.Contains(f.Id));
            }
            if (withContent.HasValue && withContent.Value == false)
            {
                var items = await query.Select(f => new
                {
                    f.Id,
                    f.ClientId,
                    f.FileType,
                    f.Name
                }).ToListAsync();
                var result = items.Select(f => new ClientFile(f.Id, f.ClientId, f.FileType, f.Name, null, null));
                return result.ToList();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<List<Client>> GetListAsync(Guid[] ids)
        {
            var query = _context.Clients.AsQueryable();
            query = query.Where(c => ids.Contains(c.Id));
            return await query.ToListAsync();

        }

        public async Task<List<Client>> GetListAsync(string? filter = null, bool? isActive = null, string? sorting = null, int? skipCount = null, int? maxResultCount = null)
        {
            var query = _context.Clients.AsQueryable();
            query = ApplyFilter(query, filter, isActive);
            query = ApplySorting(query, sorting);

            if (skipCount.HasValue && maxResultCount.HasValue)
            {
                query = query.Skip(skipCount.Value).Take(maxResultCount.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(string? filter = null, bool ? isActive = null)
        {
            var query = _context.Clients.AsQueryable();
            query = ApplyFilter(query, filter, isActive); 

            return await query.CountAsync();
        }

        private IQueryable<Client> ApplyFilter(IQueryable<Client> query, string? filter = null, bool? isActive = null)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var search = filter.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(search)
                                        || c.Lastname.ToLower().Contains(search)
                                        || c.Firstname.ToLower().Contains(search)
                                        || c.Email.ToLower().Contains(search)
                                        || c.PhoneNumber.ToLower().Contains(search)
                                        || c.Address.ToLower().Contains(search));
            }
            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }
            return query;
        }

        private IQueryable<Client> ApplySorting(IQueryable<Client> query, string? sorting = null)
        {
            if (!string.IsNullOrEmpty(sorting))
            {
                var property = sorting.Split(" ").First();
                var dir = sorting.Split(" ").Last();
                switch (property.ToLower())
                {
                    case "email":
                        query = dir == "desc" ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email);
                        break;
                    case "address":
                        query = dir == "desc" ? query.OrderByDescending(e => e.Address) : query.OrderBy(e => e.Address);
                        break; 
                    case "firstname":
                        query = dir == "desc" ? query.OrderByDescending(e => e.Firstname) : query.OrderBy(e => e.Firstname);
                        break;
                    case "lastname":
                        query = dir == "desc" ? query.OrderByDescending(e => e.Lastname) : query.OrderBy(e => e.Lastname);
                        break;
                    case "phonenumber":
                        query = dir == "desc" ? query.OrderByDescending(e => e.PhoneNumber) : query.OrderBy(e => e.PhoneNumber);
                        break;
                    default:
                        query = dir == "desc" ? query.OrderByDescending(e => e.Lastname) : query.OrderBy(e => e.Lastname);
                        break;
                }
            } else
            {
                query = query.OrderBy(e => e.Lastname);
            }
            return query;
        }
    }
}

