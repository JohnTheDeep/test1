using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.DatabaseContext;

namespace WebApplication1.Filters
{
    public class RanksParameterFilter : IParameterFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RanksParameterFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("rank", StringComparison.InvariantCultureIgnoreCase))
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var ranksContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    parameter.Schema.Enum = ranksContext.ranks.Select(p => new OpenApiString(p.Name)).ToList<IOpenApiAny>();
                }
            }
        }
    }
}
