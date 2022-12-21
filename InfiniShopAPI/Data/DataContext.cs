using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using InfiniShopAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
             
        }
    
    //Create a table for the list of users
        public DbSet<AppUser> Users { get; set; }

        //--
        public DbSet<BranchMechanics> BranchMechanics { get; set; }
        public DbSet<MechanicServices> MechanicServices { get; set; }
        public DbSet<GlobalAutomotiveServices> GlobalAutomotiveServices { get; set; }
        public DbSet<GlobalMotorcycleServices>  GlobalMotorcycleServices { get; set; }

        //--
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Adress> Adresses { get; set; }

        //--
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }


    }
}