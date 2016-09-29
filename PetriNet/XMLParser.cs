using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace DUS
{
    public static class XMLParser
    {
        public static PetriNet Parse(string path)
        {
            string xmlFile;
            XDocument document;

            if (File.Exists(path))
            {
                xmlFile = File.ReadAllText(path);
            }
            else
            {
                throw new ArgumentException("Input file path is incorrect !");
            }

            PetriNet petriNet = new PetriNet();

            try
            {
                document = XDocument.Parse(xmlFile);
            
                var ID = document.Root.Element("id").Value;
                if (ID != null && ID != string.Empty)
                    petriNet.ID = int.Parse(ID);
                else
                    petriNet.ID = null;

                var X = document.Root.Element("x").Value;
                if (X != null && ID != string.Empty)
                    petriNet.X = int.Parse(X);
                else
                    petriNet.X = null;

                var Y = document.Root.Element("y").Value;
                if (Y != null && ID != string.Empty)
                    petriNet.Y = int.Parse(Y);
                else
                    petriNet.Y = null;

                petriNet.Label = document.Root.Element("label").Value;

                petriNet.Places = new List<Place>(
                    from item in document.Root.Element("subnet").Elements("place")
                    select new Place()
                    {
                        Label = item.Element("label").Value,
                        IsStatic = bool.Parse(item.Element("isStatic").Value),
                        X = int.Parse(item.Element("x").Value),
                        Y = int.Parse(item.Element("y").Value),
                        ID = int.Parse(item.Element("id").Value),
                        Tokens = int.Parse(item.Element("tokens").Value)
                    }
                    );

                petriNet.Transitions = new List<Transition>(

                    from item in document.Root.Element("subnet").Elements("transition")
                    select new Transition()
                    {
                        Label = item.Element("label").Value,
                        ID = int.Parse(item.Element("id").Value),
                        X = int.Parse(item.Element("x").Value),
                        Y = int.Parse(item.Element("y").Value)
                    }
                    );

                petriNet.Arcs = new List<Arc>(
                    from item in document.Root.Element("subnet").Elements("arc")
                    select new Arc()
                    {
                        DestinationID = int.Parse(item.Element("destinationId").Value),
                        SourceID = int.Parse(item.Element("sourceId").Value),
                        Multiplicity = int.Parse(item.Element("multiplicity").Value),
                        Type = item.Element("type").Value
                    }
                    );
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine("Exception of type XmlException occured in XmlParser.Parse()...");
                Console.WriteLine("Error message: {0}", ex.Message);
                return null;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Exception of type ArgumentException occured in XmlParser.Parse()...");
                Console.WriteLine("Error message: {0}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception of type {0} occured in XmlParser.Parse()...", ex.GetType().ToString());
                Console.WriteLine("Error message: {0}", ex.Message);
                return null;
            }
            return petriNet;
        }
    }
}
