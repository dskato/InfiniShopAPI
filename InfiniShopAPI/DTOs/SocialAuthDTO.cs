using System.ComponentModel.DataAnnotations;

namespace InfiniShopAPI.DTOs
{
    public class SocialAuthDTO
    {
        [Required]
        public string email { set; get; }
        [Required]
        public string token { set; get; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }

        [Required]
        public string provider { get; set; }
    }
}
