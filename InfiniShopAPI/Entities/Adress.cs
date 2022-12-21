using API.Entities;

namespace InfiniShopAPI.Entities
{
    public class Adress
    {
        //PK Y FK
        public int AdressId { get; set; }
        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int? BranchMechanicsId { get; set; }
        public BranchMechanics? BranchMechanics { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }


        public string AdressName { get; set; }



    }
}
