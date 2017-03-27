﻿namespace MovieComparo.Config
{
    /// <summary>
    /// These are hard coded, but could be stored in the Web Config, Azure, a database, a configuration service, etc
    /// Having them here means that settings can be decrypted if required.
    /// </summary>
    public class Config : IConfig
    {
        public string ApiAddress
        {
            get { return "http://webjetapitest.azurewebsites.net/"; }
        }

        public string AccessToken
        {
            get { return "sjd1HfkjU83ksdsm3802k"; }
        }

        public int ApiMaxRetries
        {
            get { return 2; }
        }

        public int ApiRetryInterval
        {
            get { return 100; }
        }
    }
}
