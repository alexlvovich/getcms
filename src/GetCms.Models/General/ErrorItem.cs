namespace GetCms.Models.General
{
    public class ErrorItem
    {

        public ErrorItem(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
