using System.Collections.Generic;
using System.Linq;

namespace SIFIET.Models.ViewModels
{
    /// <summary>
    /// Vista de modelos para agregar roles a un usuario
    /// </summary>
    public class UsersInRolesViewModel
    {
        /// <summary>
        /// Objeto Context para interactuar con la base de datos
        /// </summary>
        private DbSIEPISContext db = new DbSIEPISContext();
        /// <summary>
        /// Usuario al que se le van a asignar los roles
        /// </summary>
        public aspnet_Users aspnet_User { get; private set; }
       
        /// <summary>
        /// Lista de roles que se le van a asignar al usuario
        /// </summary>
        public Dictionary<aspnet_Roles, bool> aspnet_RolesChecklist  { get; private set; }

       /// <summary>
       /// Constructor de la clase
       /// </summary>
       /// <param name="aspnet_User">Usuario</param>
       /// <param name="aspnet_Roles">Lista de roles</param>
        public UsersInRolesViewModel(aspnet_Users aspnet_User, IEnumerable<aspnet_Roles> aspnet_Roles) 
        {
            this.aspnet_User = aspnet_User;
            aspnet_RolesChecklist = new Dictionary<aspnet_Roles, bool>();
            var aspnetList = db.aspnet_UsersInRoles.Where(s => s.UserId == aspnet_User.UserId);
            bool bandera = true;
            foreach (var asproles in aspnet_Roles)
            {

                foreach (var aspnetUsersInRolese in aspnetList)
                {
                    if (asproles.RoleId == aspnetUsersInRolese.RoleId)
                    {
                        aspnet_RolesChecklist.Add(asproles, true);
                        bandera = false;
                        break;
                    }

                }
                
                if (bandera) 
                {
                    aspnet_RolesChecklist.Add(asproles, false);
                }

                bandera = true;


                
            }
        }
    }
}