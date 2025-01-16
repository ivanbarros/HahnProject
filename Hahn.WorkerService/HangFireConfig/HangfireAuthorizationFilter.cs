using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.WorkerService.HangFireConfig;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Implement your authorization logic here
        // For example, allow only authenticated users
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity.IsAuthenticated;
    }
}
