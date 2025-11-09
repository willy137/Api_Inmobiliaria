
using System.ComponentModel.DataAnnotations;


namespace Api.Models
{
	public class LoginVista
	{
		[DataType(DataType.EmailAddress)]
		public string Usuario { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}