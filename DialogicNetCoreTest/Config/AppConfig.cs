namespace DialogicNetCoreTest.Config
{
    public class AppConfig//: IConfiguration
    {
        //public string this[string key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string ApplicationName { get; set; }
        public string Version { get; set; }

        public string UDHServiceAddres { get; set; }
        public string udhAPIPath { get; set; }
        public double TimerMiliSecond { get; set; }

        public string TextToPrint { get; set; }

        //public IEnumerable<IConfigurationSection> GetChildren()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public IChangeToken GetReloadToken()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public IConfigurationSection GetSection(string key)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
