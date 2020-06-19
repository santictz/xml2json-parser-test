using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlToJsonCore
{
    internal class Program
    {
        private static DirectoryInfo xmlSourcePath;
        private static DirectoryInfo jsonTargetPath;

        public static void Main(string[] args)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(args[0]) || string.IsNullOrWhiteSpace(args[1]))
                    throw new ArgumentException("Invalid arguments provided. Please check the paths provided.");
                xmlSourcePath = new DirectoryInfo(args[0]);
                jsonTargetPath = new DirectoryInfo(args[1]);
                if (!xmlSourcePath.Exists || !jsonTargetPath.Exists)
                    throw new DirectoryNotFoundException("Cannot found provided directories");
                FileInfo[] xmlFiles = xmlSourcePath.GetFiles("*.xml", SearchOption.AllDirectories);
                if (xmlFiles.Length == 0)
                    throw new ArgumentException("Cannot found xml files to work with");
                else
                    Console.WriteLine($"Found {xmlFiles.Length} xml files to work");
                List<Task> processFilesTasks = new List<Task>();
                foreach (FileInfo xml in xmlFiles)
                    processFilesTasks.Add(ConvertToJson(xml));
                Task.WaitAll(processFilesTasks.ToArray());
                Console.WriteLine($"All {xmlFiles.Length} xml files were successfuly converted to json");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught: {e.Message}");
                Console.WriteLine("Program execution will stop. Press any key to exit");
                Console.ReadKey();
                return;
            }
        }

        private static Task ConvertToJson(FileInfo file)
        {
            try
            {
                Task conversionTask = Task.Run(() =>
                {
                    string jsonFile = Path.Combine(jsonTargetPath.FullName, file.Name.Replace(file.Extension, ".json"));
                    using StreamReader r = new StreamReader(file.Open(FileMode.Open));
                    XDocument xDoc = XDocument.Load(r);
                    Document doc = new Document(title: xDoc.Root.Element("title").Value, text: xDoc.Root.Element("text").Value);
                    string serializedDoc = JsonConvert.SerializeObject(doc, Newtonsoft.Json.Formatting.Indented);
                    using StreamWriter w = new StreamWriter(File.Create(jsonFile));
                    w.Write(serializedDoc);
                }
                );
                return conversionTask;
            }
            catch (XmlException xe)
            {
                Console.WriteLine($"XML Exception while trying to process file {file.FullName} - {xe.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught: {e.Message}");
                return null;
            }
        }
    }
}