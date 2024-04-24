using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpPage
    {
        /// <summary>
        /// ID de la Pagina
        /// </summary>
        public short PppId { get; set; }
        /// <summary>
        /// IDs de los Roles asignados
        /// </summary>
        public string PprIdSplit { get; set; } = null!;
        /// <summary>
        /// Nombre de la Pagina
        /// </summary>
        public string PppName { get; set; } = null!;
        /// <summary>
        /// Fecha de creacion de la Pagina
        /// </summary>
        public DateTime PppDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion de la Pagina
        /// </summary>
        public DateTime PppDateUpdate { get; set; }
    }
}
