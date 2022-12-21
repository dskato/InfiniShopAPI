namespace InfiniShopAPI.Entities
{
    public class Province
    {
        //PK Y FK
        public int ProvinceId { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }


        public string ProvinceName { get; set; }

        //REFERENCES
        public List<City> Cities { get; set; }
    }
}
