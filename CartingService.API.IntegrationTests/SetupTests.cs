namespace CartingService.API.IntegrationTests
{
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            File.Delete("./cart.db");
        }
    }
}