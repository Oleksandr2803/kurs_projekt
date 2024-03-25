using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Documents;

namespace Kurs_Proj.test
{
    public  class Networks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NetworkID { set; get; }

        public string Ip { get; set; }
        public bool? NetStatus { get; set; }
        public ICollection<Loh>? Lohs { get; set; } = new List<Loh>();

    }
}
