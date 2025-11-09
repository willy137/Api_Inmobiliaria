using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
    {
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly DataContext contexto;

        public PagoController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet("Alquiler/{id}")]
        public async Task<ActionResult<IList<Pago>>> Get(int id)
        {
            try
            {
                string mail = User.Identity?.Name;
                Propietario prop = await contexto.Propietario.SingleOrDefaultAsync(x => x.mail == mail);
                Alquiler alqui = await contexto.Alquiler
                .Include(a => a.inmueble)
                .FirstOrDefaultAsync(a => a.alquilerId == id && a.inmueble.propietarioId == prop.propietarioId);
                if (alqui==null)
                {
                    return BadRequest("No Existe Alquiler/Error Este Alquiler no es tuyo");
                }
                IList<Pago> pagos = await contexto.Pago.Include(p => p.alquiler)
                .Where(p => p.alquilerId == id && p.alquiler.inmueble.propietarioId == prop.propietarioId).ToListAsync();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }

        }

    }

    }