using System.ComponentModel.DataAnnotations;


namespace Api.Models
{
	public class CargarInmuVista
	{
		public string Inmueble { get; set; }
		public IFormFile imagen { get; set; }
	}
}