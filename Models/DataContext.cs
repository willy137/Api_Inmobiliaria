using Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Api.Models
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Propietario> Propietario { get; set; }
		public DbSet<Inquilino> Inquilino { get; set; }
		public DbSet<Inmueble> Inmueble { get; set; }
		public DbSet<Alquiler> Alquiler { get; set; }
        public DbSet<Pago> Pago { get; set; }

	}
}