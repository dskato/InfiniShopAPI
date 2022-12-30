namespace InfiniShopAPI.DTOs
{
    public class RegisterBranchDTO
    {

        public int AppUserId { get; set; }

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
