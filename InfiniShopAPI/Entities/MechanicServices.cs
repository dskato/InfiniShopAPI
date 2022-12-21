using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfiniShopAPI.Entities
{
    public class MechanicServices
    {
        // PK Y FK
        public int MechanicServicesId { get; set; }
        public int BranchMechanicsId { get; set; }
        public BranchMechanics BranchMechanics { get; set; }


        public string MechanicServicesName { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string TypeOfVehicle { get; set; }

       

    }
}
