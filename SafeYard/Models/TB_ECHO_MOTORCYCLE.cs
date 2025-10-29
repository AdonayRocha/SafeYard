using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeYard.Models
{
    [Table("TB_ECHO_MOTORCYCLE")]
    public class TB_ECHO_MOTORCYCLE
    {
        [Key]
        public int ID { get; set; }
        public int QTD_MOTOS { get; set; }
        public string HORA { get; set; } 
        public DateTime DATA { get; set; } 
    }
}