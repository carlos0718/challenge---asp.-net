using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Models {
    public class Suscripcion {
        [Key]
        public int IdAsociacion { get; set; }

        #region Para realcion FK
        public int SuscriptorId { get; set; }
        public virtual Suscriptor? Suscriptor { get; set; }
        #endregion

        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }
        public string? MotivoFin { get; set; }

        public Suscripcion() {

        }
        public Suscripcion(int suscriptorId) {
                
            SuscriptorId = suscriptorId;
            FechaAlta = DateTime.Today;
            MotivoFin = "";
        }
    }
}
