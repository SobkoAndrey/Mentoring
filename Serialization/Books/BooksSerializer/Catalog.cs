using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer
{
    [XmlRoot(Namespace = "http://library.by/catalog", ElementName = "catalog")]
    public class Catalog
    {
        //[XmlAttribute(AttributeName = "xmlns")]
        //public string Namespace { get; set; }

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute(AttributeName = "date")]
        public string DateFormatted
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }

            set
            {
                Date = DateTime.Parse(value);
            }
        }

        [XmlArray("books"), XmlArrayItem("book")]
        public List<Book> Books { get; set; }
    }
}
