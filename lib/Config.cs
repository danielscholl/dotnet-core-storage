using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Training
{
    public class Config
    {
        // Singleton Pattern
        public static readonly Config Data = new Config();


        public string Account { get; private set; }
        public string Key { get; private set; }
        

        public string GetConnectionString()
        {
            return $"DefaultEndpointsProtocol=https;AccountName={Config.Data.Account};AccountKey={Config.Data.Key}";
        }


        public Config()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Config>();
            var configuration = builder.Build();
            Key = configuration["ACCOUNT_KEY"];
            Account = configuration["ACCOUNT_NAME"];
        }
    }
}