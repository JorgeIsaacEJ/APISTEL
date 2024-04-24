using Microsoft.EntityFrameworkCore;

namespace APISTEL.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //EXTRAS ** SI SE ACTUALIZA EL DBCONTEXT, SE DEBE MANTENER ESTA SECCION
        #region Modelos Extras
        public DbSet<get_PpClientsService> get_PpClientsService { get; set; } = null!;
        public DbSet<Login> Login { get; set; } = null!;
        public DbSet<UserPages> UserPages { get; set; } = null!;
        public DbSet<Access> Access { get; set; } = null!;
        public DbSet<PpCharsPie> CharsPie { get; set; } = null!;
        public DbSet<PpCharsLine> CharsLine { get; set; } = null!;
        public DbSet<Years> years { get; set; } = null!;
        public DbSet<Months> months { get; set; } = null!;
        #endregion

        //DBCONTEXT
        public virtual DbSet<PpBusiness> PpBusinesses { get; set; } = null!;
        public virtual DbSet<PpClient> PpClients { get; set; } = null!;
        public virtual DbSet<PpClientsService> PpClientsServices { get; set; } = null!;
        public virtual DbSet<PpClientsServicesHistoric> PpClientsServicesHistorics { get; set; } = null!;
        public virtual DbSet<PpPage> PpPages { get; set; } = null!;
        public virtual DbSet<PpRol> PpRols { get; set; } = null!;
        public virtual DbSet<PpScheme> PpSchemes { get; set; } = null!;
        public virtual DbSet<PpService> PpServices { get; set; } = null!;
        public virtual DbSet<PpStatus> PpStatuses { get; set; } = null!;
        public virtual DbSet<PpUser> PpUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;port=3306;uid=root;database=paperplaneapi", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<PpBusiness>(entity =>
            {
                entity.HasKey(e => e.PpbId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_business");

                entity.Property(e => e.PpbId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppb_id")
                    .HasComment("ID del negocio/giro");

                entity.Property(e => e.PpbDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppb_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del negocio/giro");

                entity.Property(e => e.PpbDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppb_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del negocio/giro");

                entity.Property(e => e.PpbName)
                    .HasMaxLength(100)
                    .HasColumnName("ppb_name")
                    .IsFixedLength()
                    .HasComment("Nombre del negocio/giro");
            });

            modelBuilder.Entity<PpClient>(entity =>
            {
                entity.HasKey(e => e.PpcId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_clients");

                entity.Property(e => e.PpcId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppc_id")
                    .HasComment("ID del cliente");

                entity.Property(e => e.PpbId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppb_id")
                    .HasComment("ID del negocio/giro");

                entity.Property(e => e.PpcDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppc_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del cliente");

                entity.Property(e => e.PpcDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppc_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del cliente");

                entity.Property(e => e.PpcName)
                    .HasMaxLength(100)
                    .HasColumnName("ppc_name")
                    .IsFixedLength()
                    .HasComment("Nombre de cliente");

                entity.Property(e => e.PpstId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppst_id")
                    .HasComment("Estatus del cliente");
            });

            modelBuilder.Entity<PpClientsService>(entity =>
            {
                entity.HasKey(e => e.PpcsId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_clients_services");

                entity.Property(e => e.PpcsId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ppcs_id")
                    .HasComment("ID del cliente/servicio");

                entity.Property(e => e.PpcId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppc_id")
                    .HasComment("ID del cliente");

                entity.Property(e => e.PpcsDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppcs_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del cliente/servicio");

                entity.Property(e => e.PpcsDatePay)
                    .HasColumnName("ppcs_date_pay")
                    .HasComment("Fecha de pago del cliente/servicio");

                entity.Property(e => e.PpcsDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppcs_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del cliente/servicio");

                entity.Property(e => e.PpcsPay)
                    .HasPrecision(10, 2)
                    .HasColumnName("ppcs_pay")
                    .HasComment("Monto de pago del cliente/servicio");

                entity.Property(e => e.PpsId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("pps_id")
                    .HasComment("ID del servicio");

                entity.Property(e => e.PpscId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppsc_id");

                entity.Property(e => e.PpstId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppst_id")
                    .HasComment("Estatus del cliente/servicio");
            });

            modelBuilder.Entity<PpClientsServicesHistoric>(entity =>
            {
                entity.HasKey(e => e.PpcshId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_clients_services_historic");

                entity.Property(e => e.PpcshId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("ppcsh_id")
                    .HasComment("ID del cliente/servicio historico");

                entity.Property(e => e.PpcId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppc_id")
                    .HasComment("ID del cliente historico");

                entity.Property(e => e.PpcsId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ppcs_id")
                    .HasComment("ID del cliente/servicio historico");

                entity.Property(e => e.PpcshChange)
                    .HasColumnType("text")
                    .HasColumnName("ppcsh_change")
                    .HasComment("Detelle del cambio cliente/servicio historico");

                entity.Property(e => e.PpcshDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppcsh_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del cliente/servicio historico");

                entity.Property(e => e.PpcshDatePay)
                    .HasColumnName("ppcsh_date_pay")
                    .HasComment("Fecha de pago del cliente/servicio historico");

                entity.Property(e => e.PpcshDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppcsh_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del cliente/servicio historico");

                entity.Property(e => e.PpcshPay)
                    .HasPrecision(10, 2)
                    .HasColumnName("ppcsh_pay")
                    .HasComment("Monto de pago del cliente/servicio historico");

                entity.Property(e => e.PpsId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("pps_id")
                    .HasComment("ID del servicio historico");

                entity.Property(e => e.PpscId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppsc_id")
                    .HasComment("ID del esquema historico");

                entity.Property(e => e.PpstId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppst_id")
                    .HasComment("Estatus del cliente/servicio historico");
            });

            modelBuilder.Entity<PpPage>(entity =>
            {
                entity.HasKey(e => e.PppId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_pages");

                entity.Property(e => e.PppId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppp_id")
                    .HasComment("ID de la Pagina");

                entity.Property(e => e.PppDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppp_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion de la Pagina");

                entity.Property(e => e.PppDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppp_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion de la Pagina");

                entity.Property(e => e.PppName)
                    .HasMaxLength(100)
                    .HasColumnName("ppp_name")
                    .IsFixedLength()
                    .HasComment("Nombre de la Pagina");

                entity.Property(e => e.PprIdSplit)
                    .HasMaxLength(100)
                    .HasColumnName("ppr_id_split")
                    .IsFixedLength()
                    .HasComment("IDs de los Roles asignados");
            });

            modelBuilder.Entity<PpRol>(entity =>
            {
                entity.HasKey(e => e.PprId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_rols");

                entity.Property(e => e.PprId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppr_id")
                    .HasComment("ID del Rol");

                entity.Property(e => e.PprName)
                    .HasMaxLength(100)
                    .HasColumnName("ppr_name")
                    .IsFixedLength()
                    .HasComment("Nombre del Rol");
            });

            modelBuilder.Entity<PpScheme>(entity =>
            {
                entity.HasKey(e => e.PpscId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_scheme");

                entity.Property(e => e.PpscId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppsc_id")
                    .HasComment("ID del esquema");

                entity.Property(e => e.PpscDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppsc_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del esquema");

                entity.Property(e => e.PpscDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppsc_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del esquema");

                entity.Property(e => e.PpscName)
                    .HasMaxLength(100)
                    .HasColumnName("ppsc_name")
                    .IsFixedLength()
                    .HasComment("Nombre del esquema");
            });

            modelBuilder.Entity<PpService>(entity =>
            {
                entity.HasKey(e => e.PpsId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_services");

                entity.Property(e => e.PpsId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("pps_id")
                    .HasComment("ID del servicio");

                entity.Property(e => e.PpsDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("pps_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del servicio");

                entity.Property(e => e.PpsDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("pps_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del servicio");

                entity.Property(e => e.PpsName)
                    .HasMaxLength(100)
                    .HasColumnName("pps_name")
                    .IsFixedLength()
                    .HasComment("Nombre del servicio");
            });

            modelBuilder.Entity<PpStatus>(entity =>
            {
                entity.HasKey(e => e.PpstId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_status");

                entity.Property(e => e.PpstId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppst_id")
                    .HasComment("ID de status");

                entity.Property(e => e.PpstDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppst_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del Status");

                entity.Property(e => e.PpstDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppst_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del Status");

                entity.Property(e => e.PpstName)
                    .HasMaxLength(100)
                    .HasColumnName("ppst_name")
                    .IsFixedLength()
                    .HasComment("Nombre del Status");
            });

            modelBuilder.Entity<PpUser>(entity =>
            {
                entity.HasKey(e => e.PpuId)
                    .HasName("PRIMARY");

                entity.ToTable("pp_users");

                entity.Property(e => e.PpuId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppu_id")
                    .HasComment("ID del Usuario");

                entity.Property(e => e.PpcId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppc_id")
                    .HasComment("ID del Cliente asociado");

                entity.Property(e => e.PprId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppr_id")
                    .HasComment("ID del Rol asignado");

                entity.Property(e => e.PpstId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ppst_id")
                    .HasComment("Estatus del Usuario");

                entity.Property(e => e.PpuDateCrete)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppu_date_crete")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de creacion del Usuario");

                entity.Property(e => e.PpuDateUpdate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ppu_date_update")
                    .HasDefaultValueSql("current_timestamp()")
                    .HasComment("Fecha de modificacion del Usuario");

                entity.Property(e => e.PpuEmail)
                    .HasMaxLength(100)
                    .HasColumnName("ppu_email")
                    .IsFixedLength()
                    .HasComment("Correo del Usuario");

                entity.Property(e => e.PpuName)
                    .HasMaxLength(100)
                    .HasColumnName("ppu_name")
                    .IsFixedLength()
                    .HasComment("Nombre del Usuario");

                entity.Property(e => e.PpuPass)
                    .HasMaxLength(250)
                    .HasColumnName("ppu_pass")
                    .IsFixedLength()
                    .HasComment("Nombre del Usuario");

                entity.Property(e => e.PpuToken)
                    .HasMaxLength(250)
                    .HasColumnName("ppu_token")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
