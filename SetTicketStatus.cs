using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestFunctionAppDamatan
{
    public static class SetTicketStatus
    {
        [FunctionName("SetTicketStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string ticketnumber = req.Query["ticketnumber"];
            string ticketstatus = req.Query["ticketstatus"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            ticketnumber = ticketnumber ?? data?.ticketnumber;
            ticketstatus = ticketstatus ?? data?.ticketstatus;

            return ticketnumber != null && ticketstatus != null
                ? (ActionResult)new OkObjectResult($"{ticketnumber} status successful set to {ticketstatus}")
                : new BadRequestObjectResult("Please pass ticket number and a ticket status");
        }
    }
}
