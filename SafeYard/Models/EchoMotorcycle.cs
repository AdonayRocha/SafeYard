using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeYard.Models
{
    [Table("TB_ECHO_MOTORCYCLE")]
    public class EchoMotorcycle
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("QTD_MOTOS")]
        public int QtdMotos { get; set; }

        [Column("HORA")]
        public string Hora { get; set; } 

        [Column("DATA")]
        public DateTime Data { get; set; }

        public float HoraDecimal => float.Parse(Hora.Split(':')[0]) + float.Parse(Hora.Split(':')[1]) / 60f;
    }
}