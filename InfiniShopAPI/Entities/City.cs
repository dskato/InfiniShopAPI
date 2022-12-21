namespace InfiniShopAPI.Entities
{
    public class City
    {
        //PK Y FK
        public int CityId { get; set; }
        public Province Province { get; set; }
        public int ProvinceId { get; set; }

        public string CityName { get; set; }


        //REFERENCES
        public List<Adress> Adresses { get; set; }

    }
}
