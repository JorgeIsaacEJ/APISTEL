using APISTEL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APISTEL.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaperController : Controller
    {
        #region DBContext
        private readonly ApplicationDbContext _applicationDbContext;
        public PaperController(ApplicationDbContext applicationDbContext) { _applicationDbContext = applicationDbContext; }
        #endregion

        #region LOGIN
        /// <summary>
        /// Login and pages access
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        //[HttpGet("{token}")]
        [HttpGet]
        public async Task<IActionResult> GetLogin(string mail, string pass)
        {
            try
            {
                List<Login> logins = new();
                List<UserPages> userPages = new();
                Access access = new Access();
                string UserPass_enc = Utils.Functions.Encrypt("APITOK", pass, false);
                string token = Utils.Functions.Encrypt("ENCcipher", UserPass_enc, true);
                var login = await _applicationDbContext.Set<Login>()//LOGIN
                                     .FromSqlInterpolated($"CALL get_login  ({mail}, {UserPass_enc}, {token})").AsNoTracking().ToListAsync();
                var pages = await _applicationDbContext.Set<UserPages>()//PAGES ACCESS
                                     .FromSqlInterpolated($"CALL validate_token  ({token})").AsNoTracking().ToListAsync();
                foreach (var log in login){ logins.Add(log); }
                foreach (var pag in pages){ userPages.Add(pag); }
                access.login = logins;
                access.userPages = userPages;

                return Ok(access);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Users
        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="ppUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUser(PpUser ppUser, string token)
        {
            try
            {
                var validaToken = ValdiateToken(token);
                if (validaToken.Result)
                {
                    string UserPass_enc = Utils.Functions.Encrypt("APITOK", ppUser.PpuPass, false);
                    var login = await _applicationDbContext.Set<string>()//CREATE USER
                                         .FromSqlInterpolated($"CALL new_user  ({ppUser.PprId}, {ppUser.PpcId}, {ppUser.PpstId}, {ppUser.PpuName}, {ppUser.PpuEmail}, {UserPass_enc})").AsNoTracking().ToListAsync();
                    return Ok(login);
                }
                else
                {
                    return BadRequest("Token Invalido");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region ClientsService
        /// <summary>
        /// Consulta de clientes y servicios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClientsServices()
        {
            try
            {
                get_PpClientsService get_PpClientsService = new get_PpClientsService();
                var services = await _applicationDbContext.Set<get_PpClientsService>()
                                     .FromSqlInterpolated($"CALL get_clients")
                                     .ToListAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de un cliente y servicios
        /// </summary>
        /// <param name="PpcsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClientServices(int PpcsId)
        {
            try
            {
                get_PpClientsService get_PpClientsService = new get_PpClientsService();
                var services = await _applicationDbContext.Set<get_PpClientsService>()
                                     .FromSqlInterpolated($"CALL get_client ({PpcsId})")
                                     .ToListAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Crea un nuevo cliente y servicios
        /// </summary>
        /// <param name="ppClientsService"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewClientsServices(PpClientsService ppClientsService)
        {
            try
            {
                using (_applicationDbContext)
                {
                    Utils.Functions functions = new Utils.Functions();
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Nuevo registro
                    _applicationDbContext.PpClientsServices.Add(ppClientsService);
                    await _applicationDbContext.SaveChangesAsync();
                    PpClientsServicesHistoric ppClientsServicesHistoric = new PpClientsServicesHistoric();
                    ppClientsServicesHistoric.PpcsId = ppClientsService.PpcsId; //ID del cliente/servicio historico
                    ppClientsServicesHistoric.PpcId = ppClientsService.PpcId; //ID del cliente historico
                    ppClientsServicesHistoric.PpsId = ppClientsService.PpsId; //ID del servicio historico
                    ppClientsServicesHistoric.PpscId = ppClientsService.PpscId; //ID del esquema historico
                    ppClientsServicesHistoric.PpstId = ppClientsService.PpstId; //Estatus del cliente/servicio historico
                    ppClientsServicesHistoric.PpcshPay = ppClientsService.PpcsPay; //Monto de pago del cliente/servicio historico
                    ppClientsServicesHistoric.PpcshDatePay = ppClientsService.PpcsDatePay; //Fecha de pago del cliente/servicio historico
                    ppClientsServicesHistoric.PpcshDateCrete = DateTime.Now; //Fecha de creacion del cliente/servicio historico
                    ppClientsServicesHistoric.PpcshChange = "Se crea nuevo registro"; //Nuevo registro creado
                    //Historico de cambios
                    _applicationDbContext.PpClientsServicesHistorics.Add(ppClientsServicesHistoric);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppClientsServicesHistoric);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza Clientes Servicios
        /// </summary>
        /// <param name="ppClientsService"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateClientsServices(PpClientsService ppClientsService)
        {
            try
            {
                using (_applicationDbContext)
                {
                    Utils.Functions functions = new Utils.Functions();
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //No-Tracking query using AsNoTracking() extention method
                    var services = await _applicationDbContext.PpClientsServices.AsNoTracking().ToListAsync();
                    var clientsServices_Old = services.Where(x => x.PpcsId == ppClientsService.PpcsId);
                    //Mensaje con los cambios
                    var clientServices = functions.SetHistoricClientsServices(ppClientsService, clientsServices_Old.FirstOrDefault());
                    //Actualiza valores nuevos
                    _applicationDbContext.PpClientsServices.Update(ppClientsService);
                    await _applicationDbContext.SaveChangesAsync();
                    //Historico de cambios
                    _applicationDbContext.PpClientsServicesHistorics.Add(clientServices);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(clientServices);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta el Historial de cambios por Cliente
        /// </summary>
        /// <param name="PpcsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClientsServicesHistoric(int PpcId)
        {
            try
            {
                using (_applicationDbContext)
                {
                    Utils.Functions functions = new Utils.Functions();
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //No-Tracking query using AsNoTracking() extention method
                    var services = await _applicationDbContext.PpClientsServicesHistorics.AsNoTracking().ToListAsync();
                    var clientsServices = services.Where(x => x.PpcsId == PpcId);
                    //Mensaje con los cambios
                    List<PpClientsServicesHistoric> ppClientsServicesHistorics = clientsServices.ToList();
                    var clientServices = functions.GetHistoricClientsServices(ppClientsServicesHistorics);
                    //Detalle con los cambios
                    return Ok(clientServices);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Catalogs
        /// <summary>
        /// Consulta de negocios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusiness()
        {
            try
            {
                var businesses = await _applicationDbContext.PpBusinesses.AsNoTracking().ToListAsync();
                return Ok(businesses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Nuevo negocio
        /// </summary>
        /// <param name="ppBusiness"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewBusiness(PpBusiness ppBusiness)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Agrego el registro nuevo
                    _applicationDbContext.PpBusinesses.Add(ppBusiness);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppBusiness);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza valores negocio
        /// </summary>
        /// <param name="ppBusiness"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateBusiness(PpBusiness ppBusiness)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _applicationDbContext.PpBusinesses.Update(ppBusiness);
                    await _applicationDbContext.SaveChangesAsync();
                    return Ok(ppBusiness);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consilta de clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                var clients = await _applicationDbContext.PpClients.AsNoTracking().ToListAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Nuevo cliente
        /// </summary>
        /// <param name="ppClient"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewClients(PpClient ppClient)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Agrego el registro nuevo
                    _applicationDbContext.PpClients.Add(ppClient);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppClient);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza valores
        /// </summary>
        /// <param name="ppClient"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateClients(PpClient ppClient)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _applicationDbContext.PpClients.Update(ppClient);
                    await _applicationDbContext.SaveChangesAsync();
                    return Ok(ppClient);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de esquemas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSchemes()
        {
            try
            {
                var schemes = await _applicationDbContext.PpSchemes.AsNoTracking().ToListAsync();
                return Ok(schemes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Nuevo esquema
        /// </summary>
        /// <param name="ppScheme"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewSchemes(PpScheme ppScheme)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Agrego el registro nuevo
                    _applicationDbContext.PpSchemes.Add(ppScheme);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppScheme);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza valores
        /// </summary>
        /// <param name="ppScheme"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateSchemes(PpScheme ppScheme)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _applicationDbContext.PpSchemes.Update(ppScheme);
                    await _applicationDbContext.SaveChangesAsync();
                    return Ok(ppScheme);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de servicios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var services = await _applicationDbContext.PpServices.AsNoTracking().ToListAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Nuevo servicio
        /// </summary>
        /// <param name="ppService"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewServices(PpService ppService)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Agrego el registro nuevo
                    _applicationDbContext.PpServices.Add(ppService);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppService);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza valores
        /// </summary>
        /// <param name="ppService"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateServices(PpService ppService)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _applicationDbContext.PpServices.Update(ppService);
                    await _applicationDbContext.SaveChangesAsync();
                    return Ok(ppService);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de Status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var statuses = await _applicationDbContext.PpStatuses.AsNoTracking().ToListAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Nuevo status
        /// </summary>
        /// <param name="ppStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> NewStatus(PpStatus ppStatus)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //Agrego el registro nuevo
                    _applicationDbContext.PpStatuses.Add(ppStatus);
                    await _applicationDbContext.SaveChangesAsync();
                    //Detalle con los cambios
                    return Ok(ppStatus);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Actualiza valores
        /// </summary>
        /// <param name="ppStatus"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateStatus(PpStatus ppStatus)
        {
            try
            {
                using (_applicationDbContext)
                {
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _applicationDbContext.PpStatuses.Update(ppStatus);
                    await _applicationDbContext.SaveChangesAsync();
                    return Ok(ppStatus);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region ValidaToken
        /// <summary>
        /// Valida el token y accesos al portal
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> ValdiateToken(string token)
        {
            try
            {
                using (_applicationDbContext)
                {
                    var pages = await _applicationDbContext.Set<UserPages>()
                                         .FromSqlInterpolated($"CALL validate_token  ({token})").AsNoTracking().ToListAsync();
                    if (pages.Count() > 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Chars
        /// <summary>
        /// Consulta de clientes y servicios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetServicesChar()
        {
            try
            {
                var services = await _applicationDbContext.Set<PpCharsPie>()
                                     .FromSqlInterpolated($"CALL get_services_char")
                                     .ToListAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de ventas anuales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSalesChar()
        {
            try
            {
                using (_applicationDbContext)
                {
                    List<PpCharsLine> ppCharsLineList = new List<PpCharsLine>();
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //No-Tracking query using AsNoTracking() extention method
                    var services = await _applicationDbContext.Set<Years>()
                                        .FromSqlInterpolated($"SELECT DISTINCT YEAR(ppcsp_date_crete) AS 'year' FROM paperplaneapi.pp_clients_services_pay;").AsNoTracking()
                                        .ToListAsync();
                    foreach ( var service in services )
                    {
                        List<PpCharsPie> ppCharsPie = new List<PpCharsPie>();
                        PpCharsLine ppCharsLine = new PpCharsLine();
                        var charservices = await _applicationDbContext.Set<PpCharsPie>()
                                                .FromSqlInterpolated($"CALL get_sales_char ({service.year})")
                                                .ToListAsync();
                        ppCharsLine.name = service.year.ToString();
                        foreach (var values in charservices)
                        {
                            PpCharsPie charsPie = new PpCharsPie();
                            charsPie.name = values.name;
                            charsPie.value = values.value;
                            ppCharsPie.Add(charsPie);
                        }
                        ppCharsLine.series = ppCharsPie;
                        ppCharsLineList.Add(ppCharsLine);
                    }
                    //Detalle
                    return Ok(ppCharsLineList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Consulta de objetivos mensuales del anio en curso
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoalsChar()
        {
            try
            {
                using (_applicationDbContext)
                {
                    List<PpCharsLine> ppCharsLineList = new List<PpCharsLine>();
                    //Change tracking behavior at the context instance level
                    _applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    //No-Tracking query using AsNoTracking() extention method
                    var services = await _applicationDbContext.Set<Years>()
                                        .FromSqlInterpolated($"SELECT DISTINCT YEAR(ppcsp_date_crete) AS 'year' FROM paperplaneapi.pp_clients_services_pay;").AsNoTracking()
                                        .ToListAsync();
                    foreach (var service in services)
                    {
                        List<PpCharsPie> ppCharsPie = new List<PpCharsPie>();
                        PpCharsLine ppCharsLine = new PpCharsLine();
                        var charservices = await _applicationDbContext.Set<PpCharsPie>()
                                                .FromSqlInterpolated($"CALL get_sales_char ({service.year})")
                                                .ToListAsync();
                        ppCharsLine.name = service.year.ToString();
                        foreach (var values in charservices)
                        {
                            PpCharsPie charsPie = new PpCharsPie();
                            charsPie.name = values.name;
                            charsPie.value = values.value;
                            ppCharsPie.Add(charsPie);
                        }
                        ppCharsLine.series = ppCharsPie;
                        ppCharsLineList.Add(ppCharsLine);
                    }
                    //Detalle
                    return Ok(ppCharsLineList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
