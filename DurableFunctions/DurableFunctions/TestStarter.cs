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

namespace DurableFunctions
{
    public static class TestStarter
    {
        [FunctionName("TestStarter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [OrchestrationClient] DurableOrchestrationClient orchestrationClient)
        {
            var workItems = new List<WorkItem>
            {
                new WorkItem {FileName="test1.xls", BlobPath ="extracted"},
                new WorkItem {FileName="test2.xls", BlobPath ="extracted"},
                new WorkItem {FileName="test3.xls", BlobPath ="extracted"}
            };

            await orchestrationClient.StartNewAsync("ProcessFiles", workItems);

            return new OkObjectResult("Function executed");
        }
    }
}
