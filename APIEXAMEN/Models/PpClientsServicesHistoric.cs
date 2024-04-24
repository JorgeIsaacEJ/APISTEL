using System;
using System.Collections.Generic;

namespace APISTEL.Models
{
    public partial class PpClientsServicesHistoric
    {
        /// <summary>
        /// ID del cliente/servicio historico
        /// </summary>
        public long PpcshId { get; set; }
        /// <summary>
        /// ID del cliente/servicio historico
        /// </summary>
        public int PpcsId { get; set; }
        /// <summary>
        /// ID del cliente historico
        /// </summary>
        public short PpcId { get; set; }
        /// <summary>
        /// ID del servicio historico
        /// </summary>
        public short PpsId { get; set; }
        /// <summary>
        /// ID del esquema historico
        /// </summary>
        public short PpscId { get; set; }
        /// <summary>
        /// Estatus del cliente/servicio historico
        /// </summary>
        public short PpstId { get; set; }
        /// <summary>
        /// Monto de pago del cliente/servicio historico
        /// </summary>
        public decimal PpcshPay { get; set; }
        /// <summary>
        /// Fecha de pago del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDatePay { get; set; }
        /// <summary>
        /// Detelle del cambio cliente/servicio historico
        /// </summary>
        public string PpcshChange { get; set; } = null!;
        /// <summary>
        /// Fecha de creacion del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDateUpdate { get; set; }
    }

    public class PpClientsServicesHistoricDetaill
    {
        /// <summary>
        /// ID del cliente/servicio historico
        /// </summary>
        public long PpcshId { get; set; }
        /// <summary>
        /// ID del cliente/servicio historico
        /// </summary>
        public int PpcsId { get; set; }
        /// <summary>
        /// ID del cliente historico
        /// </summary>
        public short PpcId { get; set; }
        /// <summary>
        /// ID del servicio historico
        /// </summary>
        public short PpsId { get; set; }
        /// <summary>
        /// ID del esquema historico
        /// </summary>
        public short PpscId { get; set; }
        /// <summary>
        /// Estatus del cliente/servicio historico
        /// </summary>
        public short PpstId { get; set; }
        /// <summary>
        /// Monto de pago del cliente/servicio historico
        /// </summary>
        public decimal PpcshPay { get; set; }
        /// <summary>
        /// Fecha de pago del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDatePay { get; set; }
        /// <summary>
        /// Detelle del cambio cliente/servicio historico
        /// </summary>
        public List<string> PpcshChange { get; set; }
        /// <summary>
        /// Fecha de creacion del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del cliente/servicio historico
        /// </summary>
        public DateTime PpcshDateUpdate { get; set; }
    }
}
