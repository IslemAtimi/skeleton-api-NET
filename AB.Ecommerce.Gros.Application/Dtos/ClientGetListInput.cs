using System.ComponentModel.DataAnnotations;

namespace AB.Ecommerce.Gros.Application
{
    public class ClientGetListInput: PagedRequest
    {
        public string? Filter { get; set; }
        public string? Sorting { get; set; }
        public bool? IsActive { get; set; }
    }
}

