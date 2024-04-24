using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpService
    {
        /// <summary>
        /// ID del servicio
        /// </summary>
        public short PpsId { get; set; }
        /// <summary>
        /// Nombre del servicio
        /// </summary>
        public string? PpsName { get; set; }
        /// <summary>
        /// Fecha de creacion del servicio
        /// </summary>
        public DateTime PpsDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del servicio
        /// </summary>
        public DateTime PpsDateUpdate { get; set; }
    }
}
