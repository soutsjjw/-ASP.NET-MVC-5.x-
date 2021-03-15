using MessageBoard.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace MessageBoard.Helpers
{
    public interface IConfigHelper
    {
        string Get(string key);

        int GetInteger(string key, int output = 0);

        float GetFloat(string key, float output = 0);

        bool GetBoolean(string key, bool output = false);
    }

    public class ConfigHelper : IConfigHelper
    {
        private readonly IConfiguration _config;

        public ConfigHelper(IConfiguration config)
        {
            _config = config;
        }

        public string Get(string key)
        {
            return _config.GetValue<string>(key);
        }

        public bool GetBoolean(string key, bool output = false)
        {
            try
            {
                return _config.GetValue<bool>(key);
            }
            catch
            {
                return output;
            }
        }

        public float GetFloat(string key, float output = 0)
        {
            try
            {
                return _config.GetValue<float>(key);
            }
            catch
            {
                return output;
            }
        }

        public int GetInteger(string key, int output = 0)
        {
            try
            {
                return _config.GetValue<int>(key);
            }
            catch
            {
                return output;
            }
        }
    }
}
