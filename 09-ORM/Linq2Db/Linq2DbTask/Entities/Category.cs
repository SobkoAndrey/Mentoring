using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Column("CategoryID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public string CategoryName { get; set; }
        [Column]
        public string Description { get; set; }
        [Column]
        public byte[] Picture { get; set; }
    }
}
