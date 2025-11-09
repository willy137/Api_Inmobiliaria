using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

    namespace Api.Controllers
    {
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietarioController : ControllerBase{
        private readonly IConfiguration config;

        private readonly DataContext context;

        public PropietarioController(IConfiguration config, DataContext context)
        {
            this.config = config;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Propietario>> Get()
        {
            try
            {
                string mail = User.Identity?.Name;
                Propietario prop = context.Propietario.FirstOrDefault(p => p.mail == mail);
                return prop;
            }catch(Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginVista log)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: log.Password,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                Propietario prop = context.Propietario.FirstOrDefault(p => p.mail == log.Usuario);
                if (prop == null || prop.password != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var secreto = config["TokenAuthentication:SecretKey"];
                    if (string.IsNullOrEmpty(secreto))
                        throw new Exception("Falta configurar TokenAuthentication:Secret");
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secreto));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, prop.mail),
                        new Claim("FullName", prop.nombre + " " + prop.apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };
                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /*[HttpPost("create")]
        public async Task<ActionResult<Propietario>> agregarProp([FromBody] Propietario propNuevo)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: propNuevo.password,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                propNuevo.password = hashed;
                
                context.Propietario.Add(propNuevo);
                await context.SaveChangesAsync();

                return propNuevo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
        

        [HttpPost("mod")]
        public async Task<ActionResult<Propietario>> modPerfil([FromBody]Propietario propNuevo)
        {
            try
            {
                string mail = User.Identity?.Name;
                Propietario prop = context.Propietario.FirstOrDefault(p => p.mail == mail);
                if (prop==null)
                {
                    return BadRequest("Error no Encotramos al propietario");
                }
                else
                {
                    prop.nombre = propNuevo.nombre;
                    prop.apellido = propNuevo.apellido;
                    prop.dni = propNuevo.dni;
                    prop.telefono = propNuevo.telefono;
                    prop.mail = propNuevo.mail;
                    await context.SaveChangesAsync();
                    return prop;
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("modC")]
        public async Task<IActionResult> modifPassword([FromForm] modifPassword passw)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: passw.Actual,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                string mail = User.Identity?.Name;
                Propietario prop = context.Propietario.FirstOrDefault(p => p.mail == mail);
                if (prop == null || prop.password != hashed)
                {
                    return BadRequest("Contraseña incorrecta");
                }else
                {
                    string hashedNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: passw.Nueva,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    if (prop.password.Equals(hashedNueva))
                    {
                        return BadRequest("Contraseña Actual y Nueva Iguales");
                    }else
                    {
                        prop.password = hashedNueva;
                        await context.SaveChangesAsync();
                        return Ok("Contraseña modificada correctamente.");   
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
    }
}
