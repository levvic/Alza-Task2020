﻿using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AlzaApi.Helpers
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
        
        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                try
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Versioning API",
                Version = description.ApiVersion.ToString()
            };
            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }
            return info;
        }
    }
}
