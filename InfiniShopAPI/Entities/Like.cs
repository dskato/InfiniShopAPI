using API.Entities;

namespace InfiniShopAPI.Entities
{
    public class Like
    {
        //PK Y FK
        public int LikeId { get; set; }
        public int BranchId { get; set; }

        public int AppUserId { get; set; }

       
    }
}
