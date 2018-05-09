using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSerializer
{
    public class BooksGenerator
    {
        public static List<Book> Generate()
        {
            var book1 = new Book()
            {
                Author = "Löwy, Juval",
                Description = "COM &amp; .NET Component Services provides both traditional COM programmers and new .NET component developers with the information they need to begin developing applications that take full advantage of COM+ services. This book focuses on COM+ services, including support for transactions, queued components, events, concurrency management, and security",
                Genre = Genres.Computer,
                Id = "bk101",
                Isbn = "0-596-00103-7",
                PublishDate = new DateTime(2001, 09, 01),
                Publisher = "O'Reilly",
                RegistrationDate = new DateTime(2016, 01, 05),
                Title = "COM and .NET Component Services"
            };

            var book2 = new Book()
            {
                Author = "Ralls, Kim",
                Description = "A former architect battles corporate zombies, an evil sorceress, and her own childhood to become queen of the world.",
                Genre = Genres.Fantasy,
                Id = "bk102",
                PublishDate = new DateTime(2000, 12, 16),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2017, 01, 01),
                Title = "Midnight Rain"
            };

            var book3 = new Book()
            {
                Author = "Corets, Eva",
                Description = "After the collapse of a nanotechnology society in England, the young survivors lay the foundation for a new society.",
                Genre = Genres.Fantasy,
                Id = "bk103",
                PublishDate = new DateTime(2000, 11, 17),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2016, 01, 05),
                Title = "Maeve Ascendant"
            };

            var book4 = new Book()
            {
                Author = "Corets, Eva",
                Description = "In post-apocalypse England, the mysterious agent known only as Oberon helps to create a new life for the inhabitants of London. Sequel to Maeve Ascendant.",
                Genre = Genres.Fantasy,
                Id = "bk104",
                PublishDate = new DateTime(2001, 03, 10),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2012, 03, 01),
                Title = "Oberon's Legacy"
            };

            var book5 = new Book()
            {
                Author = "Corets, Eva",
                Description = "The two daughters of Maeve, half-sisters, battle one another for control of England. Sequel to Oberon's Legacy.",
                Genre = Genres.Fantasy,
                Id = "bk105",
                PublishDate = new DateTime(2001, 09, 10),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2016, 01, 05),
                Title = "The Sundered Grail"
            };

            var book6 = new Book()
            {
                Author = "Randall, Cynthia",
                Description = "When Carla meets Paul at an ornithology conference, tempers fly as feathers get ruffled.",
                Genre = Genres.Romance,
                Id = "bk106",
                PublishDate = new DateTime(2000, 09, 02),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2016, 01, 05),
                Title = "Lover Birds"
            };

            var book7 = new Book()
            {
                Author = "Thurman, Paula",
                Description = "A deep sea diver finds true love twenty thousand leagues beneath the sea.",
                Genre = Genres.Romance,
                Id = "bk107",
                PublishDate = new DateTime(2000, 11, 02),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2012, 01, 05),
                Title = "Splish Splash"
            };

            var book8 = new Book()
            {
                Author = "Knorr, Stefan",
                Description = "An anthology of horror stories about roaches, centipedes, scorpions  and other insects.",
                Genre = Genres.Horror,
                Id = "bk108",
                PublishDate = new DateTime(2000, 12, 06),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2011, 01, 05),
                Title = "Creepy Crawlies"
            };

            var book9 = new Book()
            {
                Author = "Kress, Peter",
                Description = "After an inadvertant trip through a Heisenberg Uncertainty Device, James Salway discovers the problems of being quantum.",
                Genre = Genres.ScienceFiction,
                Id = "bk109",
                PublishDate = new DateTime(2000, 11, 02),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2016, 01, 05),
                Title = "Paradox Lost"
            };

            var book10 = new Book()
            {
                Author = "O'Brien, Tim",
                Description = "Microsoft's .NET initiative is explored in detail in this deep programmer's reference.",
                Genre = Genres.Computer,
                Id = "bk110",
                PublishDate = new DateTime(2000, 12, 09),
                Publisher = "R &amp; D",
                RegistrationDate = new DateTime(2014, 10, 01),
                Title = "Microsoft .NET: The Programming Bible"
            };

            var book11 = new Book()
            {
                Author = "O'Brien, Tim",
                Description = "The Microsoft MSXML3 parser is covered in detail, with attention to XML DOM interfaces, XSLT processing, SAX and more.",
                Genre = Genres.Computer,
                Id = "bk111",
                PublishDate = new DateTime(2000, 12, 01),
                Publisher = "Microsoft",
                RegistrationDate = new DateTime(2010, 03, 15),
                Title = "MSXML3: A Comprehensive Guide"
            };

            var book12 = new Book()
            {
                Author = "Galos, Mike",
                Description = "Microsoft Visual Studio 7 is explored in depth, looking at how Visual Basic, Visual C++, C#, and ASP+ are integrated into a comprehensive development environment.",
                Genre = Genres.Computer,
                Id = "bk112",
                PublishDate = new DateTime(2001, 04, 16),
                Publisher = "Microsoft",
                RegistrationDate = new DateTime(2011, 04, 25),
                Title = "Visual Studio 7: A Comprehensive Guide"
            };

            var list = new List<Book>();
            list.Add(book1);
            list.Add(book2);
            list.Add(book3);
            list.Add(book4);
            list.Add(book5);
            list.Add(book6);
            list.Add(book7);
            list.Add(book8);
            list.Add(book9);
            list.Add(book10);
            list.Add(book11);
            list.Add(book12);

            return list;
        }
    }
}
