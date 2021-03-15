using MessageBoard.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace MessageBoard.Helpers
{
    public interface IConfigHelper
    {
        public string Separator { get; }

        string Get(string key);

        int GetInteger(string key, int output = 0);

        float GetFloat(string key, float output = 0);

        bool GetBoolean(string key, bool output = false);
    }

    public class ConfigHelper : IConfigHelper
    {
        private readonly IConfiguration _config;
        public string Separator => ".";

        public ConfigHelper(IConfiguration config)
        {
            _config = config;
        }

        public string Get(string key)
        {
            if (key.IndexOf(Separator) > -1)
            {
                return GetSection(key).GetValue<string>(lastKey);
            }
            else
            {
                return _config.GetValue<string>(key);
            }
        }

        public bool GetBoolean(string key, bool output = false)
        {
            try
            {
                if (key.IndexOf(Separator) > -1)
                {
                    return GetSection(key).GetValue<bool>(lastKey);
                }
                else
                {
                    return _config.GetValue<bool>(key);
                }
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
                if (key.IndexOf(Separator) > -1)
                {
                    return GetSection(key).GetValue<float>(lastKey);
                }
                else
                {
                    return _config.GetValue<float>(key);
                }
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
                if (key.IndexOf(Separator) > -1)
                {
                    return GetSection(key).GetValue<int>(lastKey);
                }
                else
                {
                    return _config.GetValue<int>(key);
                }
            }
            catch
            {
                return output;
            }
        }

        private string lastKey;

        private IConfigurationSection GetSection(string key)
        {
            string[] sections = key.Split(Separator.ToCharArray()[0]);

            IConfigurationSection _section = _config.GetSection(sections[0]);

            if (sections.Length > 2)
            {
                for (int i = 1; i < sections.Length - 1; i++)
                {
                    _section = _section.GetSection(sections[i]);
                }
            }

            lastKey = sections[sections.Length - 1];

            return _section;
        }
    }
}
