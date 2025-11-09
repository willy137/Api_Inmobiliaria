using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
    {
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmuebleController : ControllerBase {
        private readonly DataContext contexto;

        private readonly IWebHostEnvironment environment;

		public InmuebleController(DataContext contexto,IWebHostEnvironment environment)
		{
            this.contexto = contexto;
            this.environment = environment;
		}

        [HttpGet]
        public async Task<ActionResult<IList<Inmueble>>> Get()
        {
            try
            {
                string mail = User.Identity?.Name;
                Propietario prop=await contexto.Propietario.SingleOrDefaultAsync(x => x.mail == mail);
                int id = (int)prop.propietarioId;
                IList<Inmueble> inmus = await contexto.Inmueble.Where(x => x.propietarioId == id).ToListAsync();
                if (inmus.Count==0 || inmus==null)
                { 
                    return BadRequest("No tiene Inmuebles");
                }
                return Ok(inmus);
            }catch(Exception ex)
            {
                return BadRequest(new {erro=ex.Message});
            }

        }



        [HttpPost("actualizar")]
        public async Task<ActionResult<Inmueble>> actualizarInmu([FromBody] Inmueble inmu)
        {
            try{       
                Inmueble inmueble = await contexto.Inmueble.SingleOrDefaultAsync(x => x.inmuebleId == inmu.inmuebleId);
                inmueble.disponible = inmu.disponible;
                await contexto.SaveChangesAsync();
                return Ok(inmueble);
            }catch(Exception ex)
            {
                return BadRequest(new {erro=ex.Message});
            }
        }

        [HttpPost("cargar")]
        public async Task<ActionResult<Inmueble>> cargarInmu([FromForm] CargarInmuVista inmu)
        {
            try
            {
            var inmueble = JsonConvert.DeserializeObject<Inmueble>(inmu.Inmueble);
            var nbreRnd = Guid.NewGuid();
            if (inmu.imagen != null)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "inmu_" + nbreRnd + Path.GetExtension(inmu.imagen.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                inmueble.imagen = Path.Combine("/Uploads", fileName);
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    inmu.imagen.CopyTo(stream);
                }
            }
            if (inmueble.imagen == null || inmueble.imagen.Equals(""))
            {
                inmueble.imagen = "sin avatar";
            }
            string mail = User.Identity?.Name;
            Propietario prop = await contexto.Propietario.SingleOrDefaultAsync(x => x.mail == mail);
            inmueble.propietarioId = prop.propietarioId;
            contexto.Inmueble.Add(inmueble);
            await contexto.SaveChangesAsync();
            return Ok(inmueble);
            }catch(Exception ex)
            {
                return BadRequest(new {erro=ex.Message});
            }

        }
        

        [HttpGet("ContratoVigente")]
        public async Task<ActionResult<IList<Inmueble>>> ContratoVigente()
        {
            try
            {
                DateTime hoy = DateTime.Now;
                string mail = User.Identity?.Name;
                Propietario prop = await contexto.Propietario.SingleOrDefaultAsync(x => x.mail == mail);
                int id = (int)prop.propietarioId;
                IList<Inmueble> inmus= await contexto.Inmueble.Where(x=> x.propietarioId==id)
                .Where(x=> contexto.Alquiler.Any(a=> a.inmuebleId==x.inmuebleId && hoy>=a.fechaInicio && hoy<=a.fechaFin)).ToListAsync();
                return Ok(inmus);
            }catch(Exception ex)
            {
                return BadRequest(new {erro=ex.Message});
            }
        }
    }
 }