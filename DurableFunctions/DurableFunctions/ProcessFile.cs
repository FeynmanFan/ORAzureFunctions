using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using static DurableFunctions.WorkItem;

namespace DurableFunctions
{
    public static class ProcessFile
    {
        [FunctionName("ProcessFile")]
        public static WorkItem Run(
            [ActivityTrigger] DurableActivityContext context)
        {
            // do stuff to actually process file
            // maybe succeed, maybe fail
            // for now, just return failure because we're not actually doing anything

            var workItem = context.GetInput<WorkItem>();

            workItem.ProcessState = ProcessStateValues.Failure;

            return workItem;
        }
    }
}