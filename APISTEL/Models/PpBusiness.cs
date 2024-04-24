using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpBusiness
    {
        /// <summary>
        /// ID del negocio/giro
        /// </summary>
        public short PpbId { get; set; }
        /// <summary>
        /// Nombre del negocio/giro
        /// </summary>
        public string? PpbName { get; set; }
        /// <summary>
        /// Fecha de creacion del negocio/giro
        /// </summary>
        public DateTime PpbDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del negocio/giro
        /// </summary>
        public DateTime PpbDateUpdate { get; set; }
    }
}
