namespace DurableFunctions
{
    public class WorkItem
    {
        public enum ProcessStateValues
        {
            Pending = 0,
            Success = 1, 
            Failure = -1
        }

        public string FileName { get; set; }

        public string BlobPath { get; set; }

        public ProcessStateValues ProcessState { get; set; }
    }
}