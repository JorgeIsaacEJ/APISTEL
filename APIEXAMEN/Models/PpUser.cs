using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APISTEL.Models
{
    public partial class PpUser
    {
        /// <summary>
        /// ID del Usuario
        /// </summary>
        public short PpuId { get; set; }
        /// <summary>
        /// ID del Rol asignado
        /// </summary>
        public short PprId { get; set; }
        /// <summary>
        /// ID del Cliente asociado
        /// </summary>
        public short PpcId { get; set; }
        /// <summary>
        /// Estatus del Usuario
        /// </summary>
        public short PpstId { get; set; }
        /// <summary>
        /// Nombre del Usuario
        /// </summary>
        public string PpuName { get; set; } = null!;
        /// <summary>
        /// Correo del Usuario
        /// </summary>
        public string PpuEmail { get; set; } = null!;
        /// <summary>
        /// Nombre del Usuario
        /// </summary>
        public string PpuPass { get; set; } = null!;
        public string? PpuToken { get; set; }
        /// <summary>
        /// Fecha de creacion del Usuario
        /// </summary>
        public DateTime PpuDateCrete { get; set; }
        /// <summary>
        /// Fecha de modificacion del Usuario
        /// </summary>
        public DateTime PpuDateUpdate { get; set; }
    }

    [Keyless]
    public class Access
    {
        public List<Login> login { get; set; }
        public List<UserPages> userPages { get; set; }
    }

    public class Login
    {
        [Key]
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPass { get; set; }
        public string? UserToken { get; set; }
        public DateTime? UserDateUpdate { get; set;}
    }

    [Keyless]
    public class UserPages
    {
        public string? userPages { get; set; }
        public string? pageName { get; set; }
        public string? pageIcon { get; set; }
    }
}
