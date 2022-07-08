using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SummaryReport.Startup))]

namespace SummaryReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
