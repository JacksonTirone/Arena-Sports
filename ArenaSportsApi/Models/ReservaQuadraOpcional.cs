using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class ReservaQuadraOpcional
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ReservaQuadraOpcionalId { get; set; }
        public long ReservaQuadraId { get; set; }
        public int QuadraItemOpcionalId { get; set; }
        public float Valor { get; set; }

        [ForeignKey("ReservaQuadraId")]
        public ReservaQuadra ReservaQuadra { get; set; }

        [ForeignKey("QuadraItemOpcionalId")]
        public QuadraItemOpcional QuadraItemOpcional { get; set; }
    }
}
