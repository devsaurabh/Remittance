using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remitter.Business.Estimation;
using Remitter.Business.Exchange;
using Remitter.Business.Location;
using Remitter.Business.Payments;
using Remitter.Business.Taxes;
using Remitter.Core;
using Remitter.Core.Cache;
using Remitter.Data.External.Providers;
using Remitter.Data.Repository;
using RestSharp;
using System;

namespace Remitter.Api.Framework
{
    public static class ServiceRegistration
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            #region Register Helpers

            services.AddScoped<IRestClient, RestClient>();
            services.AddSingleton<IRestApiHelper, RestApiHelper>();

            #endregion

            #region Register Providers

            bool isProviderEnabled = configuration.GetValue<bool>("ProviderEnabled");
            if(isProviderEnabled)
            {
                services.AddScoped<IRemittanceProvider, ThirdPartyProvider>();
            }
            else
            {
                services.AddScoped<IRemittanceProvider, MockRemittanceProvider>();
            }

            bool isCacheEnabled = configuration.GetValue<bool>("CacheEnabled");
            if(isCacheEnabled)
            {
                // replace this line with actual cache
                //services.AddScoped<ICacheManager, NoOpCacheManager>();
            }
            else
            {
                services.AddScoped<ICacheManager, NoOpCacheManager>();
            }

            #endregion

            #region Register Repositories

            services.AddScoped<IFeeRepository, MockFeeRepository>();
            services.AddScoped<IMarkupRepository, MockMarkupRepository>();
            services.AddScoped<ITaxRepository, MockTaxRepository>();
            services.AddScoped<IEstimationRepository, CachedEstimationRepository>();

            #endregion

            #region Register Services

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEstimationService, EstimationService>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPaymentService, MockPaymentService>();
            services.AddScoped<ITaxAndFeeService, TaxAndFeeService>();

            #endregion
        }
    }
}
