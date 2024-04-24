using APISTEL.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace APISTEL.DB
{
    public class DBAccess
    {
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns></returns>
        public List<Users> getExcelFile()
        {
            List<Users> users = new List<Users>();
            using (var reader = new StreamReader("DB/Users.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    Users user = new Users();
                    user.id = Convert.ToInt32(record.id);
                    user.nombre = record.nombre;
                    user.correo = record.correo;
                    user.telefono = record.telefono;
                    user.direccion = record.direccion;
                    users.Add(user);
                }
            }
            return users;
        }
        /// <summary>
        /// Consulta un registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Users getExcelFile(int id)
        {
            Users users = null;
            using (var reader = new StreamReader("DB/Users.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records.Where(x => Convert.ToInt32(x.id) == id))
                {
                    Users users2 = new Users();
                    users2.id = Convert.ToInt32(record.id);
                    users2.nombre = record.nombre;
                    users2.correo = record.correo;
                    users2.telefono = record.telefono;
                    users2.direccion = record.direccion;
                    users = users2;
                }
            }
            return users;
        }
        /// <summary>
        /// Nuevo registro
        /// </summary>
        /// <param name="user"></param>
        public void postExcelFile(Users user)
        {
            var all = getExcelFile();
            user.id = all.MaxBy(x => x.id).id + 1;
            List<Users> users = new() { user };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
            using (var writer = new StreamWriter("DB/Users.csv", true))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(users);
            }
        }
        /// <summary>
        /// Actualiza registro
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void updateExcelFile(Users user)
        {
            List<Users> users = getExcelFile();
            foreach (var record in users.Where(x => Convert.ToInt32(x.id) == user.id))
            {
                record.id = user.id;
                record.nombre = user.nombre;
                record.correo = user.correo;
                record.telefono = user.telefono;
                record.direccion = user.direccion;
            }
            using (var writer = new StreamWriter("DB/Users.csv", false))
            using (var csvwrite = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvwrite.WriteRecords(users);
            }
        }
        /// <summary>
        /// Elimina registro
        /// </summary>
        /// <param name="id"></param>
        public void deleteExcelFile(int id)
        {
            List<Users> users = getExcelFile();
            users.RemoveAll(x=> x.id == id);
            using (var writer = new StreamWriter("DB/Users.csv", false))
            using (var csvwrite = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvwrite.WriteRecords(users);
            }
        }
    }
}
