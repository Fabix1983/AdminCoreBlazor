using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JSON.CONVERT;

namespace JSON
{
    public static class ServExtension
    {
        public static JsonSerializerOptions AddJsonOptions(this IServiceCollection services)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new DateConvert());
            services.AddSingleton(options);
            return options;
        }
    }
}
