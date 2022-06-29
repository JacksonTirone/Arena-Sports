using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class ReservaChurrasqueira
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ReservaChurrasqueiraId { get; set; }
        public long ReservaId { get; set; }
        public int ChurrasqueiraId { get; set; }
        public int ChurrasqueiraPacoteId { get; set; }
        public int Turno { get; set; }
        public float Valor { get; set; }

        [ForeignKey("ReservaId")]
        public Reserva Reserva { get; set; }
        
        [ForeignKey("ChurrasqueiraId")]
        public Churrasqueira Churrasqueira { get; set; }

        [ForeignKey("ChurrasqueiraPacoteId")]
        public ChurrasqueiraPacote Pacote { get; set; }
    }
}
