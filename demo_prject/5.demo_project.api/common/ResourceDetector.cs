using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Resources;
using System.Collections.Generic;

namespace demo_project.api
{
    public class ResourceDetector : IResourceDetector
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ResourceDetector(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public Resource Detect()
        {
            return ResourceBuilder.CreateEmpty()
                .AddService(serviceName: this.webHostEnvironment.ApplicationName)
                .AddAttributes(new Dictionary<string, object> { ["host.environment"] = this.webHostEnvironment.EnvironmentName })
                .Build();
        }
    }
}
