using API.Entities;

namespace InfiniShopAPI.Entities
{
    public class Comment
    {
        //PK Y FK
        public int CommentId { get; set; }
        public int BranchId { get; set; }
        public int AppUserId { get; set; }

        public string Commentary { get; set; }
    }
}
