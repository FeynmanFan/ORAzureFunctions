using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctions
{
    public static class UnzipIncomingFile
    {
        [FunctionName("UnzipIncomingFile")]
        public async static Task Run(
            [BlobTrigger("incoming/{name}", Connection = "zipStorage")]Stream myBlob,
            [Blob("archive/{DateTime}/{name}", FileAccess.Write, Connection = "zipStorage")]
            Stream archiveBlob,
            string name, 
            ILogger log,
            Binder binder,
            [OrchestrationClient] DurableOrchestrationClient orchestrationClient)
        {
            myBlob.CopyTo(archiveBlob);

            ZipArchive archive = new ZipArchive(myBlob);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                var attributes = new Attribute[]
                {
                    new BlobAttribute($"extracted/{entry.Name}", FileAccess.Write),
                    new StorageAccountAttribute("zipStorage")
                };

                using (var extractedStream = await binder.BindAsync<Stream>(attributes))
                {
                    using (var entryStream = entry.Open())
                    {
                        entryStream.CopyTo(extractedStream);
                    }
                }
            }

            var workItems = archive.Entries.Select(x => new WorkItem { FileName = x.FullName, BlobPath = "extracted" });

            await orchestrationClient.StartNewAsync("ProcessFiles", workItems);
        }
    }
}
