using WebApplication1.Interfaces;

namespace WebApplication1.Configuartion
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string DEFAULT_CONNECTION_STRING { get; set; }
    }
}
