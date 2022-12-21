using API.Entities;

namespace InfiniShopAPI.Entities
{
    public class BranchMechanics
    {
        //PK Y FK
        public int BranchMechanicsId { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

       


        public string Name { get; set; }

        public string ContactPhone { get; set; }
        public string? WebPage { get; set; }
        public string Description { get; set; }

        public string typeOfVehicle { get; set; }


        // REFERENCES
        public List<MechanicServices> MechanicServices { get; set; }
        public List<Adress> Adresses { get; set; }



     

    }
}
