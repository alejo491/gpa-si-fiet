using System;
using System.Collections.Generic;
using SIFIET.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIFIET.Controllers
{
    /// <summary>
    /// La clase se encarga de acceso a la BD y realiza las consultas, para enviar encuentas
    /// </summary>
    public static class SendSurveysDbController
    {
        private static DbSIEPISContext db = new DbSIEPISContext();
        
        #region Obtener Email y Nombre de un Egresado por Programa y Fecha
        /// <summary>
        /// Consulta, que obtiene el email y nombre de los egresados de un programa, inferiores o iguales a la fecha ingresada
        /// </summary>
        /// <param name="programa">Programa al que pertenece un Egresado</param>
        /// <param name="fecha">Fecha Maxima de Busqueda</param>
        /// <returns>Dicitionary(email,nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosPrograma(string programa, DateTime fecha)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios = db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate <= fecha);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var idusuario = listadoEstudio.IdUser;
                var usuario = db.aspnet_Membership.Find(idusuario);
                var nombre = db.Users.Find(idusuario);
                if (nombre != null)
                {
                    string nombreCompleto = nombre.FirstNames + " " + nombre.LastNames;
                    listado.Add(usuario.Email, nombreCompleto);
                }
                else
                {
                    listado.Add(usuario.Email, "");
                }
            }

            return listado;
        }
        #endregion

        #region Obtener Email y Nombre de un Egresado por Programa y Entre Fechas
        /// <summary>
        /// Consulta, que obtiene el email y nombre de los egresados de un programa, Entre 2 fechas ingresadas
        /// </summary>
        /// <param name="programa">Programa al que pertenece un Egresado</param>
        /// <param name="fecha">Fecha Maxima de Busqueda</param>
        /// <returns>Dicitionary(email,nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosPrograma(string programa, DateTime fechaDesde, DateTime fechaHasta)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios =
                db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate >= fechaDesde && s.EndDate < fechaHasta);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var idusuario = listadoEstudio.IdUser;
                var usuario = db.aspnet_Membership.Find(idusuario);
                var nombre = db.Users.Find(idusuario);
                if (nombre != null)
                {
                    string nombreCompleto = nombre.FirstNames + " " + nombre.LastNames;
                    listado.Add(usuario.Email, nombreCompleto);
                }
                else
                {
                    listado.Add(usuario.Email, "");
                }
            }

            return listado;
        }
        #endregion

        #region Obtener Email y Nombre de un Egresado por Programa
        /// <summary>
        /// Consulta, que obtiene el email y nombre de los egresados de un programa, inferiores o iguales a la fecha del sistema
        /// </summary>
        /// <param name="programa">Programa al que pertenece un egresado</param>
        /// <returns>Dictionary(email,nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosPrograma(string programa)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios = db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate <= DateTime.Now);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var idusuario = listadoEstudio.IdUser;
                var usuario = db.aspnet_Membership.Find(idusuario);
                var nombre = db.Users.Find(idusuario);
                if (nombre != null)
                {
                    string nombreCompleto = nombre.FirstNames + " " + nombre.LastNames;
                    listado.Add(usuario.Email, nombreCompleto);
                }
                else
                {
                    listado.Add(usuario.Email, "");
                }
            }

            return listado;
        }
        #endregion

        #region Obtener Email y Nombre de un Jefe de Egresados por Programa y Fecha
        /// <summary>
        /// Consulta que obtiene un Jefe de un Egresado, Por Programa y Fecha Despues de Grado
        /// </summary>
        /// <param name="programa">Programa al que pertenecio un Egresado</param>
        /// <param name="fecha">Fecha desde donde se desea buscar</param>
        /// <returns>Dictionary(fecha, nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosJefe(string programa, DateTime fecha)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios = db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate >= fecha);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var aux = listadoEstudio;
                var experiencia = db.Experiences.Where(e => e.IdUser == aux.IdUser);
                foreach (var experience in experiencia)
                {
                    var exp = experience;
                    var expBoss = db.ExperiencesBosses.Where(ex => ex.IdExperiences == exp.Id);
                    foreach (var experiencesBoss in expBoss)
                    {
                        var boss = db.Bosses.Find(experiencesBoss.IdBoss);
                        listado.Add(boss.Email, boss.Name);
                    }
                }
                
            }

            return listado;
        }
        #endregion

        #region Obtener Email y Nombre de un Jefe de Egresados por Programa y Entre Fechas
        /// <summary>
        /// Consulta que obtiene un Jefe de un Egresado, Por Programa y Especificando una Fecha entre los Egresados
        /// </summary>
        /// <param name="programa">Programa al que pertenecio un Egresado</param>
        /// <param name="fechaDesde">Fecha Desde donde se desea buscar</param>
        /// <param name="fechaHasta">Fecha Hasta donde se desea buscar</param>
        /// <returns>Dictionary(fecha, nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosJefe(string programa, DateTime fechaDesde, DateTime fechaHasta)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios = db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate >= fechaDesde && s.EndDate<=fechaHasta);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var aux = listadoEstudio;
                var experiencia = db.Experiences.Where(e => e.IdUser == aux.IdUser);
                foreach (var experience in experiencia)
                {
                    var exp = experience;
                    var expBoss = db.ExperiencesBosses.Where(ex => ex.IdExperiences == exp.Id);
                    foreach (var experiencesBoss in expBoss)
                    {
                        var boss = db.Bosses.Find(experiencesBoss.IdBoss);
                        listado.Add(boss.Email, boss.Name);
                    }
                }

            }
            return listado;
        }
        #endregion
        
        #region Obtener Email y Nombre de un Jefe de Egresado Por Programa
        /// <summary>
        /// Consulta, que obtiene el Email y Nombre de un Egresado de un Programa que tenga Jefe
        /// </summary>
        /// <param name="programa">Programa al que pertenecion el Egresado</param>
        /// <returns>Dictionary(email,nombre)</returns>
        public static Dictionary<String, String> ListarEgresadosJefe(string programa)
        {
            var listado = new Dictionary<string, string>();
            var listadoEstudios = db.Studies.Where(s => s.Grade == programa).Where(s => s.EndDate >= DateTime.Now);
            foreach (var listadoEstudio in listadoEstudios)
            {
                var aux = listadoEstudio;
                var experiencia = db.Experiences.Where(e => e.IdUser == aux.IdUser);
                foreach (var experience in experiencia)
                {
                    var exp = experience;
                    var expBoss = db.ExperiencesBosses.Where(ex => ex.IdExperiences == exp.Id);
                    foreach (var experiencesBoss in expBoss)
                    {
                        var boss = db.Bosses.Find(experiencesBoss.IdBoss);
                        listado.Add(boss.Email, boss.Name);
                    }
                }

            }

            return listado;
        }
        #endregion
    }
}
