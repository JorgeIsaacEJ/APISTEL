using System.ComponentModel.DataAnnotations;

namespace APISTEL.Models
{
    public class Users
    {
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string correo { get; set; }
        [Required]
        public string telefono { get; set; }
        [Required]
        public string direccion { get; set; }

    }
}
