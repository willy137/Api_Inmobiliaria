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
    public class AlquilerController : ControllerBase
    {
        private readonly DataContext contexto;

        public AlquilerController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alquiler>> Get(int id)
        {
            try
            {           
                string mail = User.Identity?.Name;
                Propietario prop = await contexto.Propietario.SingleOrDefaultAsync(x => x.mail == mail);

                DateTime hoy = DateTime.Now;

                Inmueble inmu = await contexto.Inmueble.FirstOrDefaultAsync(i => i.inmuebleId==id);
                if (inmu.propietarioId!=prop.propietarioId)
                {
                    return BadRequest("Este Contrato Pertenece A un Inmueble Que no es Suyo");
                }
                Alquiler alqui = await contexto.Alquiler.Include(a => a.inquilino)
                .Include(a => a.inmueble)
                .ThenInclude(i => i.Duenio)
                .FirstOrDefaultAsync(a => a.inmuebleId == id && hoy >= a.fechaInicio && hoy <= a.fechaFin && a.inmueble.propietarioId == prop.propietarioId);
                if (alqui==null )
                {
                    return Ok("No tiene contrato este Inmueble");
                }
                return Ok(alqui);
            }
            catch (Exception ex)
            {
                return BadRequest(new {erro=ex.Message});
            }
        }
    
    
    }

    }