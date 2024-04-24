using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpStatus
    {
        /// <summary>
        /// ID de status
        /// </summary>
        public short PpstId { get; set; }
        /// <summary>
        /// Nombre del Status
        /// </summary>
        public string? PpstName { get; set; }
        /// <summary>
        /// Fecha de creacion del Status
        /// </summary>
        public DateTime PpstDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del Status
        /// </summary>
        public DateTime PpstDateUpdate { get; set; }
    }
}
