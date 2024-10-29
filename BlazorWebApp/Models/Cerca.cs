using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Models
{
    public class Cerca
    {
        [Required(ErrorMessage = "Parola da cercare obbligatoria!")]
        public string parola { get; set; }
    }
}
