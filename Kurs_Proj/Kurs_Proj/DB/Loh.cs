using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Kurs_Proj.test
{
    public class Loh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        public string Date { set; get; }

        public long? NetworkID { set; get; }       
        public Networks? Network { set; get; }

        public long? ComandID { set; get; }
        public Comands? Comand { get; set; }

        public bool? StatusResponseComand { set; get; }
    }
}
