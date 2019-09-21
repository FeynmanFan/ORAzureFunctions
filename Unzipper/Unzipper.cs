using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Unzipper
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public async static Task Run(
            [BlobTrigger("incoming/{name}", Connection = "zipStorage")]Stream myBlob, 
            [Blob("archive/{DateTime}/{name}", FileAccess.Write, Connection ="zipStorage")]Stream archiveBlob,
            string name, ILogger log, Binder binder)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

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
        }
    }
}
