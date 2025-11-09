using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models{    


public class Pago{

    public int? pagoId  { get; set; }

    public int? nroPago { get; set; }
    public int? alquilerId { get; set; }
    [ForeignKey(nameof(alquilerId))]   
    
    public DateTime? fecha { get; set; }
    public Decimal? importe { get; set; }

    public Alquiler? alquiler{get;set;}

    public Pago(){
        
    }
}

}