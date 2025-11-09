using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Inquilino
{
    public int? inquilinoId { get; set; }

    [MaxLength(100)]
    public string? apellido { get; set; }

    [MaxLength(100)]
    public string? nombre { get; set; }

    [MaxLength(100)]
    public string? dni { get; set; }

    [MaxLength(100)]
    public string? telefono { get; set; }

    [MaxLength(500)]
    public string? mail { get; set; }


    public Inquilino()
    {

    }

}