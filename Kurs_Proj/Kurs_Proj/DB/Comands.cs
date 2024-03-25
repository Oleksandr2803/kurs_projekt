using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Proj.test
{
    public class Comands
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ComandID { get; set; }
        public string ComandName { get; set; }
        public string? Type { get; set; }
        public ICollection<Loh>? Lohs { get; set; } = new List<Loh>();
    }
}
