using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERCOWeb.Models.ViewModels
{
    public class FormViewModel
    {
        [Display(Name= "Nombre")]
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Apellido")]
        [Required]
        public string Apellido { get; set; }
        [Display(Name = "Telefono")]
        [Required]
        public string Telefono { get; set; }
        [Display(Name = "Localidad")]
        [Required]
        public string Localidad { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email{ get; set; }
        [Display(Name = "Mensaje")]
        [Required]
        public string Mensaje { get; set; }


    }
}
