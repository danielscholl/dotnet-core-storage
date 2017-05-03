using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace dotnet_core_storage
{
    public class Config
    {
        private string _accountKey;
        private string _accountName;

        // Singleton Pattern
        public static readonly Config Secrets = new Config();
        public string Account { get; private set; }
        public string Key { get; private set; }


        public Config()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Config>();
            var configuration = builder.Build();
            Key = configuration["ACCOUNT_KEY"];
            Account = configuration["ACCOUNT_NAME"];
        }
    }
}