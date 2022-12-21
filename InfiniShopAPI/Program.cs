
using InfiniShopAPI.Utils;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CountriesUtils.AddCountriesProvincesCities();
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
