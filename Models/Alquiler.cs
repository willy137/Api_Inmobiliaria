
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models{    


public class Alquiler{

    public int? alquilerId { get; set; }

    public DateTime? fechaInicio { get; set; }
    public DateTime? fechaFin { get; set; }

    public Decimal? montoAlquiler { get; set; }

    public Boolean? estado{ get; set; }
    public int? inmuebleId { get; set; } 
    [ForeignKey(nameof(inmuebleId))]   
    public int? inquilinoId { get; set; }
    [ForeignKey(nameof(inquilinoId))]

    public Inquilino? inquilino { get; set; }

    public Inmueble? inmueble{get;set;}

    public Alquiler(){
        
    }
}

}