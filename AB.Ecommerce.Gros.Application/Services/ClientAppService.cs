using System;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using AB.Ecommerce.Gros.Application.Dtos;
using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AB.Ecommerce.Gros.Application
{
    public class ClientAppService : BaseAppService, IClientAppService
    {

        private readonly IClientRepository _clientRepository;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<ClientAppService> _logger;


        public ClientAppService(ILogger<ClientAppService> logger,
            IClientRepository clientRepository, UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _clientRepository = clientRepository;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

       

        public async Task<ClientDto> RegisterAsync(RegisterInput input)
        {
            input.Validate();

            var id = Guid.NewGuid();
            var user = new IdentityUser {
                Id = id.ToString(),
                UserName = input.Username,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(Roles.GetDefault());
                if (role==null)
                {
                    throw new BusinessException(BusinessErrors.RoleDoesntExist);
                }
                await _userManager.AddToRoleAsync(user, role.Name);

                var client = new Client(id, input.Name, input.Firstname, input.Lastname,input.Password, input.PhoneNumber, input.Email, input.Address, DateTime.UtcNow);
                var files = input.Files?.Select(f => new ClientFile(
                        id,
                        f.MediaType,
                        f.Name,
                        f.MimeType,
                        f.Content
                )).ToList();
                client.Files = files;
                client = await _clientRepository.InsertAsync(client);
                await SetClientLockAsync(client.Id, false);
                return Mapper.Map<Client, ClientDto>(client);
            } else
            {
                throw new BusinessException("Unable to register user");
            } 
               
        }

        public async Task<List<FileDto>> GetFilesAsync(Guid id)
        {
            var files = await _clientRepository.GetFilesAsync(id);
            return Mapper.Map<List<ClientFile>,List<FileDto>>(files);
        }

        public async Task<FileDto> GetFileAsync(Guid id, int fileId)
        {
            var file = (await _clientRepository.GetFilesAsync(id, new[] { fileId })).SingleOrDefault();
            return Mapper.Map<ClientFile, FileDto>(file);
        }

        public async Task<FileContent> DownloadFileAsync(Guid id, int fileId)
        {
            var files = await _clientRepository.GetFilesAsync(id, new[] { fileId }, true);
            var file = files.Single();
            return new FileContent
            {
                Content = file.Content,
                MimeType = file.MimeType
            };
        }

        public async Task<PagedResult<ClientDto>> GetListAsync(ClientGetListInput input)
        {
            var clients = await _clientRepository.GetListAsync(input.Filter, input.IsActive, input.Sorting, input.SkipCount, input.MaxResultCount);
            var totalCount = await _clientRepository.CountAsync(input.Filter, input.IsActive);
            return new PagedResult<ClientDto>
            {
                Items = await MapToClientDtoAsync<ClientDto>(clients),
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<ClienWithPasswordtDto>> GetListWithPasswordAsync(ClientGetListInput input)
        {
            var clients = await _clientRepository.GetListAsync(input.Filter, input.IsActive, input.Sorting, input.SkipCount, input.MaxResultCount);
            var totalCount = await _clientRepository.CountAsync(input.Filter, input.IsActive);
            return new PagedResult<ClienWithPasswordtDto>
            {
                Items = await MapToClientDtoAsync<ClienWithPasswordtDto>(clients),
                TotalCount = totalCount
            };
        }

        public async Task<ClientDto> SetClientLockAsync(Guid id, bool locked)
        { 
            var client = await _clientRepository.GetByIdAsync(id);
            if (client!=null)
            {
                if (locked)
                {
                    client.Invalidate();
                } else
                {
                    client.Validate();
                } 
                await _clientRepository.UpdateAsync(client);
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.SetLockoutEnabledAsync(user, locked);
                return await MapToClientDtoAsync<ClientDto>(client);
            }
            return null;
        }

        public async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return await MapToClientDtoAsync<ClientDto>(client);

        }
        public async Task<ClienWithPasswordtDto> GetWithPasswordAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            return await MapToClientDtoAsync<ClienWithPasswordtDto>(client);

        }

        public async Task DeleteAsync(Guid id)
        {
            
            var client = await _clientRepository.GetByIdAsync(id);
            if (client != null)
            {
                await _clientRepository.DeleteAsync(client);
            }   
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user!=null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<ClientDto> SetRoleAsync(Guid id, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if  (role==null)
            {
                throw new BusinessException("Impossible de trouver le rôle");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                throw new BusinessException("Une erreur s'est produite en essayant de changer le rôle");
            }
            await _userManager.AddToRoleAsync(user, role.Name);
            var client = await _clientRepository.GetByIdAsync(id);
            return await MapToClientDtoAsync<ClientDto>(client);

        }

         

        private async Task<T> MapToClientDtoAsync<T>(Client client) where T: ClientDto
        {
            var dto = Mapper.Map<Client, T>(client);
            var allRoles = _roleManager.Roles.ToList();
            var user = await _userManager.FindByIdAsync(client.Id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.SingleOrDefault();
            if (!string.IsNullOrEmpty(roleName))
            {
                var role = allRoles.Where(r=>r.Name==roleName).SingleOrDefault();
                var claims = await _roleManager.GetClaimsAsync(role);
                var claim = claims.FirstOrDefault(x => x.Type == CustomClaims.ConstraintsEnabled);
                var constrainsEnabled = claim != null ? bool.Parse(claim.Value) : false;
                
            }
            return dto;

        }

        private async Task<List<T>> MapToClientDtoAsync<T>(List<Client> clients) where T : ClientDto
        {
            var dtos = new List<T>();
            var allRoles = _roleManager.Roles.ToList();
            var isConstraintsEnabled = new Dictionary<string, bool>();
            foreach (var role in allRoles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var claim = claims.FirstOrDefault(x => x.Type == CustomClaims.ConstraintsEnabled);
                var constrainsEnabled = claim != null ? bool.Parse(claim.Value) : false;
                isConstraintsEnabled[role.Id] = constrainsEnabled;
            }


            foreach (var client in clients)
            {
                var dto = Mapper.Map<Client, T>(client);
                var user = await _userManager.FindByIdAsync(client.Id.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.SingleOrDefault();
                if (!string.IsNullOrEmpty(roleName))
                {
                    var role = allRoles.Where(r => r.Name == roleName).SingleOrDefault();
                    
                }
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}

