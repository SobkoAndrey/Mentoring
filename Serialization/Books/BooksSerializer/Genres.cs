using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer
{
    public enum Genres
    {
        [XmlEnum(Name = "Computer")]
        Computer,
        [XmlEnum(Name = "Fantasy")]
        Fantasy,
        [XmlEnum(Name = "Romance")]
        Romance,
        [XmlEnum(Name = "Horror")]
        Horror,
        [XmlEnum(Name = "Science Fiction")]
        ScienceFiction
    }
}
