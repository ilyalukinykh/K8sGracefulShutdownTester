using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace K8sGracefulShutdownTester
{

    public class ConnectionCloseMiddleware : IMiddleware
    {
        private readonly IHostApplicationLifetime _lifetime;

        public ConnectionCloseMiddleware(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            checkAndSet(context);
            await next.Invoke(context);
        }

        private void checkAndSet(HttpContext context)
        {
            if (_lifetime.ApplicationStopping.IsCancellationRequested)
                context.Response.Headers["Connection"] = "Close";
        }
    }
}
