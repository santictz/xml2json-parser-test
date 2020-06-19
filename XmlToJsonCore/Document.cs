namespace XmlToJsonCore
{
    public class Document
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public Document(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}