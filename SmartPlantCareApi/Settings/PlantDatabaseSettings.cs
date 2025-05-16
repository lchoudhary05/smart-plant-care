namespace SmartPlantCareApi.Settings
{
    public class PlantDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PlantCollectionName { get; set; } = null!;
    }
}
