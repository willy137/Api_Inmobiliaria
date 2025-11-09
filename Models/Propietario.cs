

using System.ComponentModel.DataAnnotations;
namespace Api.Models;

public class Propietario
{
    public int? propietarioId { get; set; }
    
    [MaxLength(100)]
    public string? dni { get; set; }

    [MaxLength(100)]
    public string? apellido { get; set; }
    
    [MaxLength(100)]
    public string? nombre { get; set; }
    
    [MaxLength(100)]
    public string? telefono { get; set; }
    
    [MaxLength(200)]
    public string? mail { get; set; }
    
    [MaxLength(2000)]
    public string? password { get; set;}


    public Propietario()
    {

    }
}
