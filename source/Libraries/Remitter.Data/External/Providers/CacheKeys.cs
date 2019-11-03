using System;

namespace Remitter.Data.External.Providers
{
    /// <summary>
    ///  This can be more generic or can be read from the config section.
    ///  Putting here for the sake of simplicity
    /// </summary>
    public class CacheKeys
    {
        public static readonly string COUNTRY_LIST = "provider-clist";
        public static readonly TimeSpan COUNTRY_LIST_TTL = TimeSpan.FromMinutes(60);

        public static readonly string EXCHANGE_RATE = "exchange-rate";
        public static readonly TimeSpan EXCHANGE_RATE_TTL = TimeSpan.FromMinutes(15);
    }
}
