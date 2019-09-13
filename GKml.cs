using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GenerateKml.Utils
{
    public class GKml
    {

        const string HREF_ICON = "http://maps.google.com/mapfiles/kml/paddle/wht-blank.png";
        string _nameDoc = string.Empty;

        List<PlacemarkClassModel> _PlacemarkList = null;

        public GKml(string nameDoc, List<PlacemarkClassModel> PlacemarkList)
        {
            _nameDoc = nameDoc;
            _PlacemarkList = PlacemarkList;

        }
        public XmlDocument startKml()
        {
            // Create documenrt xml and declare UTF-8
            var Document = new XmlDocument();
            var declaration = Document.CreateXmlDeclaration("1.0", "UTF-8", null);
            Document.AppendChild(declaration);

            //Create element KML
            var ElementKml = Document.CreateElement("kml");
            ElementKml.SetAttribute("xmlns", "http://www.opengis.net/kml/2.2");

            //Create element Document
            var ElementDoc = Document.CreateElement("Document");
            ElementKml.AppendChild(ElementDoc);

            //Create Style Elements
            var ElementStyle = Document.CreateElement("Style");
            var ElementIconStyle = Document.CreateElement("IconStyle");
            var ElementIcon = Document.CreateElement("Icon");
            var ElementHref = Document.CreateElement("href");

            //Values style
            ElementStyle.SetAttribute("id", "myStyle");
            ElementHref.InnerText = HREF_ICON;

            ElementIcon.AppendChild(ElementHref);
            ElementIconStyle.AppendChild(ElementIcon);
            ElementStyle.AppendChild(ElementIconStyle);

            ElementDoc.AppendChild(ElementStyle);

            //Conditionals PlacemarkList or Route

            if (_PlacemarkList != null)
            {
                foreach (var item in _PlacemarkList)
                {
                    //Create Placemark, name, description point cordinates
                    var Placemark = Document.CreateElement("Placemark");
                    var Name = Document.CreateElement("name");
                    var Description = Document.CreateElement("description");
                    var Point = Document.CreateElement("Point");
                    var coordinates = Document.CreateElement("coordinates");
                    var styleUrl = Document.CreateElement("styleUrl");

                    //Values Elements
                    styleUrl.InnerText = "#myStyle";
                    Name.InnerText = item.name;
                    Description.InnerText = item.description;
                    coordinates.InnerText = item.point.coordinates;

                    //Add elements
                    Placemark.AppendChild(styleUrl);
                    Placemark.AppendChild(Name);
                    Placemark.AppendChild(Description);
                    Point.AppendChild(coordinates);
                    Placemark.AppendChild(Point);

                    ElementDoc.AppendChild(Placemark);
                }
            }

            Document.AppendChild(ElementKml);
            string xmlString = string.Empty;

            //Probable 
            using (var wr = new myStringWriter(Encoding.UTF8))
            {
                Document.Save(wr);
                xmlString = wr.ToString();
            }
            return Document;
        }
    }
    
    public class Point
    {
        public string longitude { private get;  set; }
        public string latitude { private get; set; }
        public string coordinates
        {
            get
            {
                return string.Format("{0},{1},0", longitude.Replace(',', '.'), latitude.Replace(',', '.'));
            }
        }
    }

    public class PlacemarkClassModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public Point point { get; set; }
    }

    public class myStringWriter: StringWriter
    {
        public override Encoding Encoding { get; }

        public myStringWriter(Encoding encoding)
        {
            Encoding = encoding;
        }
    }
}
