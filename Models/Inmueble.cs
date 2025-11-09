
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models{    
    public class Inmueble
    {

        public int? inmuebleId { get; set; }
        
        [MaxLength(500)]
        public string? direccion { get; set; }

        [MaxLength(100)]
        public string? tipo { get; set; }
        
        [MaxLength(100)]
        public string? uso { get; set; }

        public int? ambientes { get; set; }

        public Decimal? latitud { get; set; }

        public Decimal? longuitud { get; set; }

        public int? superficie { get; set; }

        public Decimal? valor { get; set; }

        [MaxLength(2000)]
        public String? imagen{ get; set; }

        public int? propietarioId { get; set; }
        [ForeignKey(nameof(propietarioId))]

        public Boolean? disponible { get; set; }

        public Boolean? tieneContratoVigente { get; set; }

        [Display (Name = "Propietario")]    
        public Propietario? Duenio{get;set;}

        public Inmueble (){

        }
    }
}

