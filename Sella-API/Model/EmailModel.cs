namespace Sella_API.Model
{
    public class EmailModel
    {
        public string TO { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailModel(string to, string subject, string content)
        {
            TO = to;
            Subject = subject;
            Content = content;
        }
    }
}
