namespace InfiniShopAPI.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }


        //REFERENCES
        public List<Province> Provinces { get; set; }
    }
}
