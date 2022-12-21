using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniShopAPI.DTOs
{
    public class ServicesDTO
    {
        public string ServicesName { get; set; }
        public decimal Price { get; set; }
    }
}
