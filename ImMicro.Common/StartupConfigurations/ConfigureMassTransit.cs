﻿using System;
using ImMicro.Common.Constans;
using ImMicro.Common.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Common.StartupConfigurations
{
    /// <summary>
    /// Distributed cache configuration extension
    /// </summary>
    public static class ConfigureMassTransit
    {
        /// <summary>
        /// Add Mass transit configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOption>(configuration.GetSection(AppConstants.RabbitMqSettingsOptionName));

            var rabbitMqConfig = new RabbitMqOption();
            configuration.GetSection(AppConstants.RabbitMqSettingsOptionName).Bind(rabbitMqConfig);
            
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqConfig.RabbitMqUri),hst =>
                    {
                        hst.Username(rabbitMqConfig.UserName);
                        hst.Password(rabbitMqConfig.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            
            return services;
        }
    }
}