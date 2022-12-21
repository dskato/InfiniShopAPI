using InfiniShopAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
    
        public string PhoneNumber { get; set; } = String.Empty;
        [Required]
        public string Email { get; set; }

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        [Required]
        public string Role { get; set; } //Normal user, Mechanical user


        //REFERENCES
        public List<BranchMechanics> BranchMechanics { get; set; }
        public List<Adress> Adresses { get; set; }


    }
}