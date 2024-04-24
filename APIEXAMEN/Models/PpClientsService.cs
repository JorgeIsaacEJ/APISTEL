using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APISTEL.Models
{
    public partial class PpClientsService
    {
        /// <summary>
        /// ID del cliente/servicio
        /// </summary>
        public int PpcsId { get; set; }
        /// <summary>
        /// ID del cliente
        /// </summary>
        public short PpcId { get; set; }
        /// <summary>
        /// ID del servicio
        /// </summary>
        public short PpsId { get; set; }
        public short PpscId { get; set; }
        /// <summary>
        /// Estatus del cliente/servicio
        /// </summary>
        public short PpstId { get; set; }
        /// <summary>
        /// Monto de pago del cliente/servicio
        /// </summary>
        public decimal PpcsPay { get; set; }
        /// <summary>
        /// Fecha de pago del cliente/servicio
        /// </summary>
        public DateTime PpcsDatePay { get; set; }
        /// <summary>
        /// Fecha de creacion del cliente/servicio
        /// </summary>
        public DateTime PpcsDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del cliente/servicio
        /// </summary>
        public DateTime PpcsDateUpdate { get; set; }
    }

    public class get_PpClientsService
    {
        [Key]
        /// <summary>
        /// ID del cliente/servicio CALCULADO
        /// </summary>
        public int ppp_id { get; set; }
        /// <summary>
        /// ID del cliente/servicio *ppcs_id
        /// </summary>
        public int ppcs_id { get; set; }
        /// <summary>
        /// ID del cliente *ppc_id
        /// </summary>
        public short ppc_id { get; set; }
        /// <summary>
        /// Nombre de cliente *ppc_name
        /// </summary>
        public string ppc_name { get; set; }
        /// <summary>
        /// ID del servicio *pps_id
        /// </summary>
        public short pps_id { get; set; }
        /// <summary>
        /// Nombre del servicio *pps_name
        /// </summary>
        public string pps_name { get; set; }
        /// <summary>
        /// ID del esquema *ppsc_id
        /// </summary>
        public short ppsc_id { get; set; }
        /// <summary>
        /// Nombre del esquema *ppsc_name
        /// </summary>
        public string ppsc_name { get; set; }
        /// <summary>
        /// Estatus del cliente/servicio *ppst_id
        /// </summary>
        public short ppst_id { get; set; }
        /// <summary>
        /// Nombre del Status *ppst_name
        /// </summary>
        public string ppst_name { get; set; }
        /// <summary>
        /// ID del negocio/giro *ppb_id
        /// </summary>
        public short ppb_id { get; set; }
        /// <summary>
        /// Nombre del negocio/giro *ppb_name
        /// </summary>
        public string ppb_name { get; set; }
        /// <summary>
        /// Monto de pago del cliente/servicio *ppcs_pay
        /// </summary>
        public decimal ppcs_pay { get; set; }

        /// <summary>
        /// Monto de pago (ABONO) del cliente/servicio *ppcs_pays
        /// </summary>
        public decimal ppcs_pays { get; set; }
        /// <summary>
        /// Monto de pago (CAMPANIA) del cliente/servicio *ppcs_pays_campaign
        /// </summary>
        public decimal ppcs_pays_campaign { get; set; }
        /// <summary>
        /// Monto de pago (COMISION) del cliente/servicio *ppcs_pays_task
        /// </summary>
        public decimal ppcs_pays_task { get; set; }

        /// <summary>
        /// Fecha de pago del cliente/servicio *ppcs_date_pay
        /// </summary>
        public DateTime ppcs_date_pay { get; set; }
    }
}
