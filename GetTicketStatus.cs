using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestFunctionAppDamatan
{
    public static class GetTicketStatus
    {
        [FunctionName("GetTicketStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string ticketnumber = req.Query["ticketnumber"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            ticketnumber = ticketnumber ?? data?.ticketnumber;

            var dummyTicketStatuses = new List<string>();
            dummyTicketStatuses.Add("NEW");
            dummyTicketStatuses.Add("OPEN");
            dummyTicketStatuses.Add("RESOLVED");
            dummyTicketStatuses.Add("NEED ADDITIONAL INFO FROM USER");

            var status = dummyTicketStatuses[new Random().Next(4)];

            return ticketnumber != null
                ? (ActionResult)new OkObjectResult($"{ticketnumber}: {status}")
                : new BadRequestObjectResult("Please pass a ticket number on the query string or in the request body");
        }
    }
}
