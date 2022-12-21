using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace InfiniShopAPI.DTOs
{
    public class RegisterMechanicDTO
    {
        [Required]
        public string email { set; get; }
        [Required]
        public string password { set; get; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }


        public string typeOfVehicle { get; set; }
        public string nameSucursal { get; set; }
        public string contactPhone { get; set; }
        public string webPage { get; set; }
        public string description { get; set; }

        
        public int cityId { get; set; }
        public string address { get; set; }

        public ServicesDTO[] serviceLs { get; set; }

    }
}
