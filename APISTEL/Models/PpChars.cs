using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace APISTEL.Models
{
    [Keyless]
    public class PpCharsLine
    {
        /// <summary>
        /// Nombre
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Estructura PIE
        /// </summary>
        [NotMapped]
        public List<PpCharsPie> series { get; set; }
    }
    [Keyless]
    public class PpCharsPie
    {
        /// <summary>
        /// Nombre
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Valor
        /// </summary>
        public int value { get; set; }
    }
    #region HELPERS
    [Keyless]
    public class Years
    {
        /// <summary>
        /// Anios
        /// </summary>
        public int year { get; set; }
    }
    [Keyless]
    public class Months
    {
        public int Enero { get; set; }
        public int Febrero { get; set; }
        public int Marzo { get; set; }
        public int Abril { get; set; }
        public int Mayo { get; set; }
        public int Junio { get; set; }
        public int Julio { get; set; }
        public int Agosto { get; set; }
        public int Septiembre { get; set; }
        public int Noviembre { get; set; }
        public int Diciembre { get; set; }
    }
    #endregion
}
