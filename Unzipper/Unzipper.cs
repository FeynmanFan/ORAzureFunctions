using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Unzipper
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run(
            [BlobTrigger("incoming/{name}", Connection = "zipStorage")]Stream myBlob, 
            [Blob("archive/{DateTime}/{name}", FileAccess.Write, Connection ="zipStorage")]Stream archiveBlob,
            string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            myBlob.CopyTo(archiveBlob);

            var currentDirectory = Directory.GetCurrentDirectory();

            var extracted = Path.Combine(currentDirectory, "extracted");

            Directory.Delete(extracted);
            Directory.CreateDirectory(extracted);

            ZipArchive archive = new ZipArchive(myBlob);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                entry.ExtractToFile(Path.Combine(extracted, entry.FullName));
            }
        }
    }
}
