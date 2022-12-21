using CsvHelper;
using InfiniShopAPI.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InfiniShopAPI.Utils
{
    public class CountriesUtils
    {
        private static List<string> CountriesLs = new List<string> { "Ecuador" };
        private static List<string> EcuadorProvincesLs = new List<string> { "Azuay", "Bolívar", "Cañar", "Carchi", "Chimborazo", "Cotopaxi", "El Oro", "Esmeraldas", "Galápagos", "Guayas", "Imbabura", "Loja", "Los Ríos", "Manabí", "Morona Santiago", "Napo", "Orellana", "Pastaza", "Pichincha", "Santa Elena", "Santo Domingo de los Tsáchilas", "Sucumbíos", "Tungurahua", "Zamora Chinchipe" };

        // Ecuador Provinces
        private static Dictionary<int, string> CountriesM = new Dictionary<int, string>();
        private static Dictionary<int, string> EcuadorProvinces = new Dictionary<int, string>();
        private static Dictionary<int, CityHelperDTO> EcuadorProvincesCities = new Dictionary<int, CityHelperDTO>();

        // Instances 
        private static CityHelperDTO cityHelperDTO = new CityHelperDTO();
        private static int iter = 0;
        private static IConfiguration _configuration;

        //Config data 
        static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
        private static string connectionString = conf["ConnectionStrings:DefaultConnection"].ToString();

        public CountriesUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static void AddCountriesProvincesCities()
        {

            //Append into dict the country with the current ID
            for (int x = 0; x < CountriesLs.Count; x++)
            {
                CountriesM.Add(x, CountriesLs[x]);
            }

            //Append into dict the province with the current ID
            for (int x = 0; x < EcuadorProvincesLs.Count; x++)
            {
                EcuadorProvinces.Add(x, EcuadorProvincesLs[x]);
            }

            //Read csv
            using (var reader = new StreamReader("Files/ProvinciasCiudades.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var ProvCiud = new
                {
                    PROVINCIA = string.Empty,
                    CIUDAD = string.Empty
                };
                var records = csv.GetRecords(ProvCiud);

                foreach (var r in records)
                {
                    //Console.WriteLine("P: " + r.PROVINCIA + "  C: "+ r.CIUDAD);
                    foreach (KeyValuePair<int, string> kvPair in EcuadorProvinces)
                    {

                        //If current province in map<> is equal to province in csv add to EcuadorProvincesCities<>
                        if (kvPair.Value.Trim().ToLower().Equals(r.PROVINCIA.Trim().ToLower()))
                        {
                            cityHelperDTO = new CityHelperDTO();
                            cityHelperDTO.CityId = kvPair.Key;
                            cityHelperDTO.CityName = r.CIUDAD.Trim().ToLower();
                            EcuadorProvincesCities.Add(iter++, cityHelperDTO);
                        }
                    }

                }


                //Queries
                String queryCountries = " SET IDENTITY_INSERT dbo.Countries ON INSERT INTO dbo.Countries (CountryId,CountryName) VALUES (@CountryId,@CountryName)";
                String queryProvinces = "SET IDENTITY_INSERT dbo.Provinces ON INSERT INTO dbo.Provinces (ProvinceId,ProvinceName,CountryId) VALUES (@ProvinceId,@ProvinceName, @CountryId)";
                String queryCities = " INSERT INTO dbo.Cities (CityName, ProvinceId) VALUES (@CityName, @ProvinceId)";
                String queryCountryValidation = " SELECT CountryId FROM dbo.Countries  WHERE CountryName=@CountryName";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {

                        //Iterate over contries and add
                        //NOTE IF WE NEED TO ADD MORE COUNTRIES, WE NEED TO ADD THEY CORRESPODING PROVINCES
                        foreach (KeyValuePair<int, string> kvPair in CountriesM)
                        {
                            using (SqlCommand commandValidation = new SqlCommand(queryCountryValidation, connection)) {

                                Console.WriteLine("Paiss: "+kvPair.Value);
                                commandValidation.Parameters.AddWithValue("@CountryName", kvPair.Value);

                                try {
                                    int? id = (int)commandValidation.ExecuteScalar();
                                    Console.WriteLine("Country exists!");
                                    return;
                                }
                                catch (Exception ex) {
                                    Console.WriteLine("Country doesnt exists, adding ...");
                                }
                              

                            }

                            using (SqlCommand commandCountry = new SqlCommand(queryCountries, connection))
                            {
                                commandCountry.Parameters.AddWithValue("@CountryId", kvPair.Key);
                                commandCountry.Parameters.AddWithValue("@CountryName", kvPair.Value);
                                
                                int result = commandCountry.ExecuteNonQuery();
                                // Check Error
                                if (result < 0) { Console.WriteLine("Error inserting data into Database!"); }

                                //Iterate over Provinces
                                foreach (KeyValuePair<int, string> kvPairP in EcuadorProvinces)
                                {

                                    using (SqlCommand commandProvince = new SqlCommand(queryProvinces, connection))
                                    {

                                        commandProvince.Parameters.AddWithValue("@ProvinceId", kvPairP.Key);
                                        commandProvince.Parameters.AddWithValue("@ProvinceName", kvPairP.Value);
                                        commandProvince.Parameters.AddWithValue("@CountryId", kvPair.Key); // using country fk key
                                        int resultP = commandProvince.ExecuteNonQuery();
                                        // Check Error
                                        if (resultP < 0) { Console.WriteLine("Error inserting data into Database! - province"); }



                                    }
                                }

                            }

                        }
                        //Iterate over cities
                        foreach (KeyValuePair<int, CityHelperDTO> kvPairC in EcuadorProvincesCities)
                        {
                            using (SqlCommand commandCities = new SqlCommand(queryCities, connection))
                            {

                                //commandCities.Parameters.AddWithValue("@CityId", kvPairC.Key);
                                commandCities.Parameters.AddWithValue("@CityName", kvPairC.Value.CityName);
                                commandCities.Parameters.AddWithValue("@ProvinceId", kvPairC.Value.CityId); // using city fk key
                                int resultC = commandCities.ExecuteNonQuery();
                                // Check Error
                                if (resultC < 0) { Console.WriteLine("Error inserting data into Database! - city"); }


                            }
                        }

                    }
                    catch (IOException e)
                    {
                        
                        if (e.Source != null)
                            Console.WriteLine("IOException source: {0}", e.Source);
                        throw;
                    }




                    connection.Close();


                }

            }

        }

    }
}
