using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(Catalog));

            // Сериализация
            var catalog = new Catalog() { Date = DateTime.Now, Books = BooksGenerator.Generate()};

            using (var stream = new FileStream(@"C:\myBooks.xml", FileMode.Create))
            {
                serializer.Serialize(stream, catalog);
            }

            Thread.Sleep(1000);

            // Десериализация
            var list = serializer.Deserialize(new FileStream(@"C:\myBooks.xml", FileMode.Open))as Catalog;
        }
    }
}
