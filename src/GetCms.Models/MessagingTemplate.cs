using GetCms.Models.Enums.Messaging;

namespace GetCms.Models
{
    public class MessagingTemplate : Content
    {
        public int ContentId { get; set; }
        public string Subject { get; set; }
        public string Parameters { get; set; }

        public TemplateTypes TemplateType { get; set; }

        public TargetTypes Target { get; set; }
    }
}
