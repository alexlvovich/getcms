using Microsoft.Extensions.Logging;

namespace GetCms.Services
{
    public abstract class BaseService
    {
        internal readonly ILogger _logger;

        public BaseService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger(this.GetType().Name);
        }
    }
}
