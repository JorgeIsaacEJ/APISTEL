using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpClient
    {
        /// <summary>
        /// ID del cliente
        /// </summary>
        public short PpcId { get; set; }
        /// <summary>
        /// ID del negocio/giro
        /// </summary>
        public short? PpbId { get; set; }
        /// <summary>
        /// Nombre de cliente
        /// </summary>
        public string? PpcName { get; set; }
        /// <summary>
        /// Estatus del cliente
        /// </summary>
        public short? PpstId { get; set; }
        /// <summary>
        /// Fecha de creacion del cliente
        /// </summary>
        public DateTime PpcDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del cliente
        /// </summary>
        public DateTime PpcDateUpdate { get; set; }
    }
}
