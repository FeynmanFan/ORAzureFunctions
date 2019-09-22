namespace DurableFunctions
{
    using Microsoft.Azure.WebJobs;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ProcessFiles
    {
        [FunctionName("ProcessFiles")]
        public async static Task<List<WorkItem>> Run(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
           var workItems = context.GetInput<List<WorkItem>>();

            var tasks = new List<Task<WorkItem>>();

            foreach (var workItem in workItems)
            {
                tasks.Add(context.CallActivityAsync<WorkItem>("ProcessFile", workItem));
            }

            await Task.WhenAll(tasks);

            return tasks.Select(x => x.Result).ToList();
        }
    }
}
