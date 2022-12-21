using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InfiniShopAPI.Services
{
    public class PrefillDBData
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;


        public PrefillDBData(DataContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public void insertCountries() {

            String query = "INSERT INTO dbo.SMS_PW (CountryId,CountryName,ProvinceId) VALUES (@CountryId,@CountryName,@ProvinceId)";

            SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryId", "abc");
                command.Parameters.AddWithValue("@CountryName", "abc");
                command.Parameters.AddWithValue("@ProvinceId", "abc");

                connection.Open();
                int result = command.ExecuteNonQuery();

                // Check Error
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }

        private async Task<bool> CountryExists(string country)
        {
            return await _context.Countries.AnyAsync(x => x.CountryName == country.ToLower());
        }
        private async Task<bool> ProviceExists(string province)
        {
            return await _context.Provinces.AnyAsync(x => x.ProvinceName == province.ToLower());
        }
        private async Task<bool> CityExists(string city)
        {
            return await _context.Cities.AnyAsync(x => x.CityName == city.ToLower());
        }

    }
}
