using System.Collections.Generic;
using System.Linq;
using SIFIET.Models;

namespace SIFIET.Models.ViewModels
{
    /// <summary>
    /// Modelo de vista que para asignar permisos de acceso a los roles
    /// </summary>
    public class RoleMethodsViewModel
    {
        /// <summary>
        /// Objeto Context para interactuar con la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();
        
        /// <summary>
        /// Rol al que se le van a asignar los permisos
        /// </summary>
        public aspnet_Roles aspnet_Role { get; private set; }       
        
        /// <summary>
        /// Metodos que se van a agregar al rol
        /// </summary>
        public Dictionary<Method, bool> MethodChecklist  { get; private set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="aspnet_Role">Rol</param>
        /// <param name="Method">Lista de metodos</param>
        public RoleMethodsViewModel(aspnet_Roles aspnet_Role, IEnumerable<Method> Method) 
        {
            this.aspnet_Role = aspnet_Role;
            MethodChecklist = new Dictionary<Method, bool>();
            var aspnetList = db.RoleMethods.Where(s => s.IdRole == aspnet_Role.RoleId);
            bool bandera = true;
            foreach (var auxmethod in Method)
            {

                foreach (var methodslist in aspnetList)
                {
                    if (auxmethod.Id == methodslist.IdAction)
                    {
                        MethodChecklist.Add(auxmethod, true);
                        bandera = false;
                        break;
                    }

                }
                
                if (bandera) 
                {
                    MethodChecklist.Add(auxmethod, false);
                }

                bandera = true;


                
            }
        }
    }
}