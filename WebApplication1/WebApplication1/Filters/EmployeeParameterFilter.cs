using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.DatabaseContext;

namespace WebApplication1.Filters
{
    public class EmployeeParameterFiler : IParameterFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EmployeeParameterFiler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("employee", StringComparison.InvariantCultureIgnoreCase))
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var ranksContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    parameter.Schema.Enum = ranksContext.employees.Select(p => new OpenApiString(p.FullName)).ToList<IOpenApiAny>();
                }
            }
        }
    }
}
