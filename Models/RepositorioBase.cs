

namespace Api.Models
{
	public abstract class RepositorioBase
	{
		protected readonly IConfiguration configuracion;
		protected readonly string connectionString;

		protected RepositorioBase(IConfiguration configuration)
		{
			this.configuracion = configuration;
			connectionString = configuration["ConnectionStrings:MySql"];
		}
	}
}