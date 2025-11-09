
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
	public class modifPassword
	{
		[DataType(DataType.Password)]
		public string Actual { get; set; }
		[DataType(DataType.Password)]
		public string Nueva { get; set; }
	}
}