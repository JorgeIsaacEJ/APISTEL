using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpScheme
    {
        /// <summary>
        /// ID del esquema
        /// </summary>
        public short PpscId { get; set; }
        /// <summary>
        /// Nombre del esquema
        /// </summary>
        public string? PpscName { get; set; }
        /// <summary>
        /// Fecha de creacion del esquema
        /// </summary>
        public DateTime PpscDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del esquema
        /// </summary>
        public DateTime PpscDateUpdate { get; set; }
    }
}
