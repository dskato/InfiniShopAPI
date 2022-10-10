using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


//The best place to put the validations are in the DTO
//Add later more register data user :)
namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string email {set;get;}
        [Required]
        public string password {set;get;}
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}