
Ipify.GetMyIpAddress MicroService Library
----------------------------------------------
The Ipify.GetMyIpAddress is a Microservice that allows you to retrieve your external IPv4 and IPv6 Address, it leverages ipify webservices.

Things to keep in mind
----------------------------------------------
You must wrap the calls in error handling, the http call will throw errors on non success HTTP status codes. 

Logging
--------
The library offers logging capabilities for debugging and troubleshooting. Debug level will offer application workflow and Trace will offer both application workflow and variable information. 

Configuration Format (Appsettings.json)
----------------------------------------
Use the Ipify.GetMyIpAddress MicroService Settings format below within your appsettings Json

```
{
  ...
  "IpServiceSettings": {
    "TimeOutInSeconds": 30
  },
  ...
}
```

Example of how to use the Ipify.GetMyIpAddress MicroService Library
---------------------------------------
The Ipify.GetMyIpAddress MicroService Library can be injected into your project by leveraging the extension method while passing in the configuration.
This example shows the configuration being pulled from the appsettings.json, but it can be pulled from any other configuration.
As long as it is an Action\<T\>

```
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            //The only line you need to Add the Ipify.GetMyIpAddress to your project.
            services.AddIpService(Configuration.GetSection("IpServiceSettings").Get<IpServiceSettings>());        
            ...
        }
```
The Ipify.GetMyIpAddress MicroService Library is now in your DI Container and is fully configured and ready to be injected into your application and controllers
Here is an example of a controller utilizing the Ipify.GetMyIpAddress MicroService library after injecting it with DI.

```
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Ipify.GetMyIpAddress;

namespace IpServiceDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IpService _ipService; 

        public IndexModel(ILogger<IndexModel> logger, IpService ipService)
        {
            _ipService = ipService 
            _logger = logger;
        }

        public async void OnGet()
        {
            
        }
    }
}
```