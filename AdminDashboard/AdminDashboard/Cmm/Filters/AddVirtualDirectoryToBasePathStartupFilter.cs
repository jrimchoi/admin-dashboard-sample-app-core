using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace AdminDashboard.Cmm.Filters
{
    public class AddVirtualDirectoryToBasePathStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                // Sets the root-relative application path to '<ServerRoot>/app'
                builder.UsePathBase("/dashboard");
                next(builder);
            };
        }
    }
}
