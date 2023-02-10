using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TestApp.Models {
	public class Suscriptor {
		[Key]
		public int SuscriptorId { get; set; }
		[Required]
		public string? Nombre { get; set; }
		[Required]
		public string? Apellido { get; set; }
		[Required]
		public string? NumeroDocumento { get; set; }
		[Required]
		public string? TipoDocumento { get; set; }
		[Required]
		public string? Direccion { get; set; }
		public string? Email { get; set; }
		[Required]
		public string? NombreUsuario { get; set;}
		[Required]
		public string? PasswordUsuario { get; set; }

	}
}