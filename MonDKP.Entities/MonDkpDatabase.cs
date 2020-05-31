namespace MonDKP.Entities
{
    public class MonDkpDatabase
    {
        public DkpHistory DkpHistory { get; set; } = new DkpHistory();

        public LootHistory LootHistory { get; set; } = new LootHistory();

        public DkpTable DkpTable { get; set; } = new DkpTable();
    }
}