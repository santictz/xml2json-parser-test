using System;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Moravia.Homework
{
    public class Document
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            try
            {
                FileStream sourceStream = File.Open(sourceFileName, FileMode.Open);
                var reader = new StreamReader(sourceStream);
                string input = reader.ReadToEnd();
                var xdoc = XDocument.Parse(input);
                var doc = new Document
                {
                    Title = xdoc.Root.Element("title").Value,
                    Text = xdoc.Root.Element("text").Value
                };

                var serializedDoc = JsonConvert.SerializeObject(doc);

                var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(targetStream))
                {
                    sw.Write(serializedDoc);
                }
                //var sw = new StreamWriter(targetStream); -> Original method
                //sw.Write(serializedDoc);


                //File.WriteAllText(targetFileName, serializedDoc); -> Method 2

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}