using Microsoft.Extensions.Configuration;

namespace SeleniumProject.Utilities
{
    public static class ConfigReader
    {
        private static readonly IConfigurationRoot _config;

        static ConfigReader()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string Browser => _config["Browser"] ?? "Chrome";
        public static string BaseUrl => _config["BaseUrl"] ?? "http://localhost:3000";
        public static int ImplicitWaitInSeconds => int.Parse(_config["ImplicitWaitInSeconds"] ?? "5");
        public static int ExplicitWaitInSeconds => int.Parse(_config["ExplicitWaitInSeconds"] ?? "10");
        public static string TestEmail => _config["TestAccount:Email"] ?? "";
        public static string TestPassword => _config["TestAccount:Password"] ?? "";
        public static string AdminEmail => _config["AdminAccount:Email"] ?? "";
        public static string AdminPassword => _config["AdminAccount:Password"] ?? "";
    }
}