using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ClientServices.Providers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using SIFIET.Controllers;
using SIFIET.Models;
using FluentSecurity;
using FluentSecurity.Policy;

namespace SIFIET
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Nombre de ruta
                "{controller}/{action}/{id}", // URL con parámetros
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Valores predeterminados de parámetro
            );

        }

        protected void Application_Start()
        {

           // Desactivar rapidamente seguridad de la apliación
           
          #region Cargar controladores
            var assembly = new AssemblyHelper();
            var result = assembly.GetControllerNames();
            assembly.CreateAdmin();
            #endregion
          #region Reglas de seguridad
                //Reglas de configuración
                SecurityConfigurator.Configure(configuration =>
                {

                    //Se le dice a la aplicacion de donde sacar el estado del usuario y los roles que tiene
                    configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);
                    configuration.GetRolesFrom(Roles.GetRolesForUser);

                    //Reglas de configruacion para controladores

                    #region Reglas del Home Controller
                    //Configuracion para el controlador de Home
                    configuration.For<HomeController>().Ignore();
                    configuration.For<VerifyController>().Ignore();
                    configuration.For<AccountController>().Ignore();
                    configuration.For<UserController>(x => x.Create()).Ignore();
                    configuration.For<VerifyController>().Ignore();

                    #endregion

                    //****************************************************************************************************//
                    //*********************¡AQUI VAN TODAS LAS REGLAS QUE SE VAN A AGREGAR! *******************************//
                    //****************************************************************************************************//

                    #region Reglas de configuracion por cada controlador

                    //************Configuracion para el controlador de encuestas.********************

                    #region Reglas del controlador Surveys
                    //Se obtienen los roles que tienen acceso al controlador Surveys y a los metodos de este controlador
                    string[] rolesSurveysIndex = assembly.GetRolesMethods("SurveysController", "Index");
                    var rolesSurveysCreate = assembly.GetRolesMethods("SurveysController", "Create");
                    var rolesSurveysEdit = assembly.GetRolesMethods("SurveysController", "Edit");
                    var rolesSurveysDelete = assembly.GetRolesMethods("SurveysController", "Delete");
                    var rolesSurveysDeleteConfirmed = assembly.GetRolesMethods("SurveysController", "DeleteConfirmed");
                    var rolesSurveysDetails = assembly.GetRolesMethods("SurveysController", "Details");
                    var rolesSurveysCreateR = assembly.GetRolesMethods("SurveysController", "CreateReport");


                    //Se configuran las reglas de acceso para el controlador surveys.
                    configuration.For<SurveysController>(x => x.Index())
                                 .RequireAnyRole(rolesSurveysIndex);

                    configuration.For<SurveysController>(x => x.Create())
                                 .RequireAnyRole(rolesSurveysCreate);

                    configuration.For<SurveysController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesSurveysEdit);

                    configuration.For<SurveysController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesSurveysDelete);

                    configuration.For<SurveysController>(x => x.DeleteConfirmed(default(Guid)))
                                  .RequireAnyRole(rolesSurveysDeleteConfirmed);

                    configuration.For<SurveysController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesSurveysDetails);

                    configuration.For<SurveysController>(x => x.CreateReport(default(Guid)))
                                  .RequireAnyRole(rolesSurveysCreateR);

                    //configuration.For<SurveysController>(x => x.CreateReport(default(Guid))).Ignore();

                    #endregion

                    #region Reglas del controlador Post
                    ////Se obtienen los roles que tienen acceso al controlador Post y a los métodos de este controlador.
                    //REGLAS ADMINISTRADOR
                    string[] rolesPostIndex = assembly.GetRolesMethods("PostController", "Index");
                    var rolesPostDetailsAdmin = assembly.GetRolesMethods("PostController", "DetailsAdmin");
                    var rolesPostCreateAdmin = assembly.GetRolesMethods("PostController", "CreateAdmin");
                    var rolesPostEditAdmin = assembly.GetRolesMethods("PostController", "EditAdmin");
                    var rolesPostSearch = assembly.GetRolesMethods("PostController", "Search");
                   // var rolesPostAutorizarPost = assembly.GetRolesMethods("PostController", "AutorizarPost");
                    //var rolesPostPostPrincipal = assembly.GetRolesMethods("PostController", "PostPrincipal");
                    //var rolesPostCambiarEstadoPost = assembly.GetRolesMethods("PostController", "CambiarEstadoPost");

                    ////REGLAS USUARIO
                    string[] rolesPostUserIndex = assembly.GetRolesMethods("PostController", "UserIndex");
                    var rolesPostDetails = assembly.GetRolesMethods("PostController", "Details");
                    var rolesPostCreate = assembly.GetRolesMethods("PostController", "Create");
                    var rolesPostEdit = assembly.GetRolesMethods("PostController", "Edit");
                    var rolesPostUserSearch = assembly.GetRolesMethods("PostController", "UserSearch");
                    var rolesPostUserFilter = assembly.GetRolesMethods("PostController", "UserFilter");

                    //// Se configuran las reglas de acceso para el controlador Post.
                    ////REGLAS ADMINISTRADOR

                    configuration.For<PostController>().Ignore();

                    configuration.For<PostController>(x => x.Index(default(int)))
                                 .RequireAnyRole(rolesPostIndex);

                    configuration.For<PostController>(x => x.DetailsAdmin(default(Guid)))
                                  .RequireAnyRole(rolesPostDetailsAdmin);

                    configuration.For<PostController>(x => x.CreateAdmin())
                                     .RequireAnyRole(rolesPostCreateAdmin);

                    configuration.For<PostController>(x => x.EditAdmin(default(Guid)))
                                     .RequireAnyRole(rolesPostEditAdmin);

                    configuration.For<PostController>(x => x.Search(default(string), default(int)))
                                         .RequireAnyRole(rolesPostSearch);

                    //configuration.For<PostController>(x => x.AutorizarPost(default(Guid), default(int)))
                    //.RequireAnyRole(rolesPostAutorizarPost);

                    //configuration.For<PostController>(x => x.PostPrincipal(default(Guid), default(int)))
                    //.RequireAnyRole(rolesPostPostPrincipal);

                    //configuration.For<PostController>(x => x.CambiarEstadoPost(default(Guid), default(int)))
                    // .RequireAnyRole(rolesPostCambiarEstadoPost);

                    //// REGLAS USUARIO
                    configuration.For<PostController>(x => x.UserIndex(default(int)))
                                 .RequireAnyRole(rolesPostUserIndex);

                    configuration.For<PostController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesPostDetails);

                    configuration.For<PostController>(x => x.Create())
                                  .RequireAnyRole(rolesPostCreate);

                    configuration.For<PostController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesPostEdit);

                    configuration.For<PostController>(x => x.UserSearch(default(string), default(int)))
                                 .RequireAnyRole(rolesPostUserSearch);

                    configuration.For<PostController>(x => x.UserFilter(default(List<string>), default(int)))
                                     .RequireAnyRole(rolesPostUserFilter);

                    //REGLAS PUBLICAS
                    configuration.For<PostController>(x => x.ShowPosts()).Ignore();
                    configuration.For<PostController>(x => x.ShowListPosts()).Ignore();
                    configuration.For<PostController>(x => x.ShowPost(default(Guid))).Ignore();
                    configuration.For<PostController>(x => x.GlobalIndex(default(int))).Ignore();
                    configuration.For<PostController>(x => x.GlobalSearch(default(string), default(int))).Ignore();
                    configuration.For<PostController>(x => x.GlobalFilter(default(List<string>), default(int))).Ignore();
                    //configuration.For<LikeController>().AllowAny();
                   // configuration.For<StartboxController>().AllowAny();

                    #endregion

                    #region Reglas del controlador Like

                    
                    // Se obtienen los roles que tienen acceso al controlador Like y a los métodos de este controlador.
                    //var rolesLikeCreate = assembly.GetRolesMethods("LikeController", "Create");
                    //var rolesLikeDeleteConfirmed = assembly.GetRolesMethods("LikeController", "DeleteConfirmed");
                    //var rolesLikeget_likes = assembly.GetRolesMethods("LikeController", "get_likes");
                    //var rolesLikegetMyLikePost = assembly.GetRolesMethods("LikeController", "getMyLikePost");
                    //var rolesLikeNum_likes = assembly.GetRolesMethods("LikeController", "Num_likes");

                    //Se configuran las reglas de acceso para el controlador Like.                    

                    //configuration.For<LikeController>(x => x.Create(default(Guid)))
                    //              .RequireAnyRole(rolesLikeCreate);

                    //configuration.For<LikeController>(x => x.DeleteConfirmed(default(Guid), default(Guid)))
                    //             .RequireAnyRole(rolesLikeDeleteConfirmed);

                    //configuration.For<LikeController>(x => x.get_likes(default(Guid)))
                    //             .RequireAnyRole(rolesLikeget_likes);

                    //configuration.For<LikeController>(x => x.getMyLikePost(default(Guid), default(Guid)))
                    //                 .RequireAnyRole(rolesLikeget_likes);

                    //configuration.For<LikeController>(x => x.Num_likes(default(Guid)))
                    //                     .RequireAnyRole(rolesLikeNum_likes);

                    #endregion

                    #region Reglas del controlador Startbox
                    //Se obtienen los roles que tienen acceso al controlador Startbox y a los métodos de este controlador.
                    //string[] rolesStartboxIndex = assembly.GetRolesMethods("StartboxController", "Index");
                    //var rolesStartboxCreate = assembly.GetRolesMethods("StartboxController", "Create");
                    //var rolesStartboxDetails = assembly.GetRolesMethods("StartboxController", "Details");
                    //var rolesStartboxEdit = assembly.GetRolesMethods("StartboxController", "Edit");
                    //var rolesStartboxDelete = assembly.GetRolesMethods("StartboxController", "Delete");
                    //var rolesStartboxDeleteConfirmed = assembly.GetRolesMethods("StartboxController", "DeleteConfirmed");



                    #endregion

                    #region Reglas del controlador AnswerChoices
                    //Se obtienen los roles que tienen acceso al controlador AnswerChoices y a los metodos de este controlador
                    string[] rolesAnswerChoicesIndex = assembly.GetRolesMethods("AnswerChoicesController", "Index");
                    var rolesAnswerChoicesCreate = assembly.GetRolesMethods("AnswerChoicesController", "Create");
                    var rolesAnswerChoicesEdit = assembly.GetRolesMethods("AnswerChoicesController", "Edit");
                    var rolesAnswerChoicesDelete = assembly.GetRolesMethods("AnswerChoicesController", "Delete");
                    var rolesAnswerChoicesDeleteConfirmed = assembly.GetRolesMethods("SurveysController", "DeleteConfirmed");
                    var rolesAnswerChoicesDetails = assembly.GetRolesMethods("AnswerChoicesController", "Details");


                    //Se configuran las reglas de acceso para el controlador AnswerChoices.
                    configuration.For<AnswerChoicesController>(x => x.Index(default(Guid)))
                                 .RequireAnyRole(rolesAnswerChoicesIndex);

                    configuration.For<AnswerChoicesController>(x => x.Create(default(Guid)))
                                 .RequireAnyRole(rolesAnswerChoicesCreate);

                    configuration.For<AnswerChoicesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesAnswerChoicesEdit);

                    configuration.For<AnswerChoicesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesAnswerChoicesDelete);

                    configuration.For<AnswerChoicesController>(x => x.DeleteConfirmed(default(Guid)))
                                  .RequireAnyRole(rolesAnswerChoicesDeleteConfirmed);

                    configuration.For<AnswerChoicesController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesAnswerChoicesDetails);

                    #endregion

                    #region Reglas del controlador FillSurvey
                    //Se obtienen los roles que tienen acceso al controlador FillSurvey y a los metodos de este controlador
                    string[] rolesFillSurveyFill = assembly.GetRolesMethods("FillSurveyController", "Fill");


                    //Se configuran las reglas de acceso para el controlador FillSurvey.
                    configuration.For<FillSurveyController>(x => x.Fill(default(Guid), default(string), 0))
                        .Ignore();

                    #endregion

                    #region Reglas del controlador ItemData
                    //Se obtienen los roles que tienen acceso al controlador ItemData y a los metodos de este controlador
                    string[] rolesItemDataCreate = assembly.GetRolesMethods("ItemDataController", "Create");
                    var rolesItemDataDelete = assembly.GetRolesMethods("AnswerChoicesController", "Delete");
                    var rolesItemDataDeleteConfirmed = assembly.GetRolesMethods("SurveysController", "DeleteConfirmed");


                    //Se configuran las reglas de acceso para el controlador ItemData.

                    configuration.For<ItemDataController>(x => x.Create(default(Guid)))
                                 .RequireAnyRole(rolesItemDataCreate);

                    configuration.For<ItemDataController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesItemDataDelete);

                    configuration.For<ItemDataController>(x => x.DeleteConfirmed(default(Guid)))
                                  .RequireAnyRole(rolesItemDataDeleteConfirmed);

                    #endregion

                    #region Reglas del controlador ItemSurveys
                    //Se obtienen los roles que tienen acceso al controlador ItemSurveys y a los metodos de este controlador
                    string[] rolesItemSurveysIndex = assembly.GetRolesMethods("ItemSurveysController", "Index");
                    var rolesItemSurveysCreate = assembly.GetRolesMethods("ItemSurveysController", "Create");
                    var rolesItemSurveysDelete = assembly.GetRolesMethods("ItemSurveysController", "Delete");
                    var rolesItemSurveysDeleteConfirmed = assembly.GetRolesMethods("ItemSurveysController", "DeleteConfirmed");
                    var rolesItemSurveysDetails = assembly.GetRolesMethods("ItemSurveysController", "Details");


                    //Se configuran las reglas de acceso para el controlador ItemSurveys.
                    configuration.For<ItemSurveysController>(x => x.Index(default(Guid)))
                                 .RequireAnyRole(rolesItemSurveysIndex);

                    configuration.For<ItemSurveysController>(x => x.Create(default(Guid)))
                                 .RequireAnyRole(rolesItemSurveysCreate);

                    configuration.For<ItemSurveysController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesItemSurveysDelete);

                    configuration.For<ItemSurveysController>(x => x.DeleteConfirmed(default(Guid)))
                                  .RequireAnyRole(rolesItemSurveysDeleteConfirmed);

                    configuration.For<ItemSurveysController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesItemSurveysDetails);

                    #endregion

                    #region Reglas del controlador Aspnet_Roles
                    //Se obtienen los roles que tienen acceso al controlador Aspnet_Roles y a los metodos de este controlador
                    string[] rolesAspnet_RolesIndex = assembly.GetRolesMethods("Aspnet_RolesController", "Index");
                    var rolesAspnet_RolesCreate = assembly.GetRolesMethods("Aspnet_RolesController", "Create");
                    var rolesAspnet_RolesEdit = assembly.GetRolesMethods("Aspnet_RolesController", "Edit");
                    var rolesAspnet_RolesDelete = assembly.GetRolesMethods("Aspnet_RolesController", "Delete");
                    var rolesAspnet_RolesDeleteConfirmed = assembly.GetRolesMethods("Aspnet_RolesController", "DeleteConfirmed");
                    var rolesAspnet_RolesDetails = assembly.GetRolesMethods("Aspnet_RolesController", "Details");


                    //Se configuran las reglas de acceso para el controlador Aspnet_Roles.
                    configuration.For<Aspnet_RolesController>(x => x.Index())
                                 .RequireAnyRole(rolesAspnet_RolesIndex);

                    configuration.For<Aspnet_RolesController>(x => x.Create())
                                 .RequireAnyRole(rolesAspnet_RolesCreate);

                    configuration.For<Aspnet_RolesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesAspnet_RolesEdit);

                    configuration.For<Aspnet_RolesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesAspnet_RolesDelete);

                    configuration.For<Aspnet_RolesController>(x => x.DeleteConfirmed(default(Guid)))
                                  .RequireAnyRole(rolesAspnet_RolesDeleteConfirmed);

                    configuration.For<Aspnet_RolesController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesAspnet_RolesDetails);

                    #endregion

                    #region Reglas del controlador Questions
                    //Se obtienen los roles que tienen acceso al controlador Questions y a los metodos de este controlador
                    string[] rolesQuestionsIndex = assembly.GetRolesMethods("QuestionsController", "Index");
                    var rolesQuestionsCreate = assembly.GetRolesMethods("QuestionsController", "Create");
                    var rolesQuestionsEdit = assembly.GetRolesMethods("QuestionsController", "Edit");
                    var rolesQuestionsDelete = assembly.GetRolesMethods("QuestionsController", "Delete");
                    var rolesQuestionsDeleteConfirmed = assembly.GetRolesMethods("QuestionsController", "DeleteConfirmed");
                    var rolesQuestionsDetails = assembly.GetRolesMethods("QuestionsController", "Details");


                    //Se configuran las reglas de acceso para el controlador Questions.
                    configuration.For<QuestionsController>(x => x.Index(default(Guid)))
                    .RequireAnyRole(rolesQuestionsIndex);

                    configuration.For<QuestionsController>(x => x.Create(default(Guid)))
                    .RequireAnyRole(rolesQuestionsCreate);

                    configuration.For<QuestionsController>(x => x.Edit(default(Guid), default(Guid)))
                    .RequireAnyRole(rolesQuestionsEdit);

                    configuration.For<QuestionsController>(x => x.Delete(default(Guid)))
                    .RequireAnyRole(rolesQuestionsDelete);

                    configuration.For<QuestionsController>(x => x.DeleteConfirmed(default(Guid)))
                    .RequireAnyRole(rolesQuestionsDeleteConfirmed);

                    configuration.For<QuestionsController>(x => x.Details(default(Guid)))
                    .RequireAnyRole(rolesQuestionsDetails);

                    #endregion

                    #region Reglas del controlador Wizard
                    //Se obtienen los roles que tienen acceso al controlador Wizard y a los metodos de este controlador
                    string[] rolesWizardIndex = assembly.GetRolesMethods("WizardController", "Index");
                    /*var rolesWizardSkip = assembly.GetRolesMethods("WizardController", "Skip");
                    var rolesWizardNext = assembly.GetRolesMethods("WizardController", "Next");
                    var rolesWizardEnd = assembly.GetRolesMethods("WizardController", "End");
                    */

                    //Se configuran las reglas de acceso para el controlador Wizard.
                    configuration.For<WizardController>()
                    .RequireAnyRole(rolesWizardIndex);


                    #endregion

                    #region Reglas del controlador SendSurveys
                    //Se obtienen los roles que tienen acceso al controlador SendSurveys y a los metodos de este controlador
                    string[] rolesSendSurveysSend = assembly.GetRolesMethods("SendSurveysController", "Send");
                    string[] rolesSendSurveysSendSpecific = assembly.GetRolesMethods("SendSurveysController", "SendSpecific");
                    string[] rolesSendSurveysPreview = assembly.GetRolesMethods("SendSurveysController", "Preview");

                    //Se configuran las reglas de acceso para el controlador SendSurveys.
                    configuration.For<SendSurveysController>(x => x.Send(default(Guid)))
                    .RequireAnyRole(rolesSendSurveysSend);

                    configuration.For<SendSurveysController>(x => x.SendSpecific(default(Guid)))
                                    .RequireAnyRole(rolesSendSurveysSendSpecific);

                    configuration.For<SendSurveysController>(x => x.Preview(default(Guid)))
                                                    .RequireAnyRole(rolesSendSurveysPreview);
                    configuration.For<SendSurveysController>(x => x.Autocomplete(default(string))).Ignore();

                    #endregion

                    #region Reglas del controlador Topic
                    //Se obtienen los roles que tienen acceso al controlador topic y a los metodos de este controlador
                    string[] rolesTopicIndex = assembly.GetRolesMethods("TopicController", "Index");
                    var rolesTopicCreate = assembly.GetRolesMethods("TopicController", "Create");
                    var rolesTopicEdit = assembly.GetRolesMethods("TopicController", "Edit");
                    var rolesTopicDelete = assembly.GetRolesMethods("TopicController", "Delete");
                    var rolesTopicDeleteConfirmed = assembly.GetRolesMethods("TopicController", "DeleteConfirmed");
                    var rolesTopicDetails = assembly.GetRolesMethods("TopicController", "Details");


                    //Se configuran las reglas de acceso para el controlador topic.
                    configuration.For<TopicController>(x => x.Index())
                        .RequireAnyRole(rolesTopicIndex);

                    configuration.For<TopicController>(x => x.Create())
                        .RequireAnyRole(rolesTopicCreate);

                    configuration.For<TopicController>(x => x.Edit(default(Guid)))
                        .RequireAnyRole(rolesTopicEdit);

                    configuration.For<TopicController>(x => x.Delete(default(Guid)))
                        .RequireAnyRole(rolesTopicDelete);

                    configuration.For<TopicController>(x => x.DeleteConfirmed(default(Guid)))
                        .RequireAnyRole(rolesTopicDeleteConfirmed);

                    configuration.For<TopicController>(x => x.Details(default(Guid)))
                        .RequireAnyRole(rolesTopicDetails);

                    #endregion

                    #region Reglas del controlador UserController
                    //Se obtienen los roles que tienen acceso al controlador User y a los metodos de este controlador
                    string[] rolesUserIndex = assembly.GetRolesMethods("UserController", "Index");
                    //var rolesUserCreate = assembly.GetRolesMethods("UserController", "Create");
                    var rolesUserBegin = assembly.GetRolesMethods("UserController", "Begin");
                    var rolesUserEdit = assembly.GetRolesMethods("UserController", "Edit");
                    var rolesUserState = assembly.GetRolesMethods("UserController", "State");
                    var rolesUserGenerate = assembly.GetRolesMethods("UserController", "Generate");
                    var rolesUserDetails = assembly.GetRolesMethods("UserController", "Details");
                    var rolesUserRegister = assembly.GetRolesMethods("UserController", "Register");
                    var rolesUserSearch = assembly.GetRolesMethods("UserController", "Search");
                    var rolesUserOut = assembly.GetRolesMethods("UserController", "Out");

                    //Se configuran las reglas de acceso para el controlador User.
                    configuration.For<UserController>(x => x.Index(0)).RequireAnyRole(rolesUserIndex);

                    //configuration.For<UserController>(x => x.Create()).RequireAnyRole(rolesUserCreate);

                    configuration.For<UserController>(x => x.Begin(default(Guid))).RequireAnyRole(rolesUserBegin);

                    configuration.For<UserController>(x => x.Edit(default(Guid))).RequireAnyRole(rolesUserEdit);

                    configuration.For<UserController>(x => x.State(default(Guid))).RequireAnyRole(rolesUserState);

                    configuration.For<UserController>(x => x.Generate(default(Guid))).RequireAnyRole(rolesUserGenerate);

                    configuration.For<UserController>(x => x.Details(default(Guid))).RequireAnyRole(rolesUserDetails);

                    configuration.For<UserController>(x => x.Register()).RequireAnyRole(rolesUserRegister);

                    configuration.For<UserController>(x => x.Search(default(string), 0)).RequireAnyRole(rolesUserSearch);

                    configuration.For<UserController>(x=>x.Out(default(Guid))).RequireAnyRole(rolesUserOut);

                    #endregion

                    #region Reglas del controlador ElectiveController
                    //Se obtienen los roles que tienen acceso al controlador  Elective y a los metodos de este controlador
                    string[] rolesElectiveIndex = assembly.GetRolesMethods("ElectiveController", "Index");
                    //var rolesElectiveCreate = assembly.GetRolesMethods("ElectiveController", "Create");
                    var rolesElectiveEdit = assembly.GetRolesMethods("ElectiveController", "Edit");
                    //var rolesElectiveDelete = assembly.GetRolesMethods("ElectiveController", "Delete");
                    //var rolesElectiveDeleteConfirmed = assembly.GetRolesMethods("ElectiveController", "DeleteConfirmed");
                    var rolesElectiveDetails = assembly.GetRolesMethods("ElectiveController", "Details");


                    //Se configuran las reglas de acceso para el controlador Elective.
                    configuration.For<ElectiveController>(x => x.Index()).RequireAnyRole(rolesElectiveIndex);

                    //configuration.For<ElectiveController>(x => x.Create()).RequireAnyRole(rolesElectiveCreate);

                    configuration.For<ElectiveController>(x => x.Edit(default(Guid))).RequireAnyRole(rolesElectiveEdit);

                    //configuration.For<ElectiveController>(x => x.Delete(default(Guid))).RequireAnyRole(rolesElectiveDelete);

                    //configuration.For<ElectiveController>(x => x.DeleteConfirmed(default(Guid))).RequireAnyRole(rolesElectiveDeleteConfirmed);

                    configuration.For<ElectiveController>(x => x.Details(default(Guid))).RequireAnyRole(rolesElectiveDetails);

                    #endregion

                    #region Reglas del controlador SchoolController
                    //Se obtienen los roles que tienen acceso al controlador School y a los metodos de este controlador
                    string[] rolesSchoolIndex = assembly.GetRolesMethods("SchoolController", "Index");
                    //var rolesSchoolCreate = assembly.GetRolesMethods("SchoolController", "Create");
                    var rolesSchoolEdit = assembly.GetRolesMethods("SchoolController", "Edit");
                    //var rolesSchoolDelete = assembly.GetRolesMethods("SchoolController", "Delete");
                    //var rolesSchoolDeleteConfirmed = assembly.GetRolesMethods("SchoolController", "DeleteConfirmed");
                    var rolesSchoolDetails = assembly.GetRolesMethods("SchoolController", "Details");


                    //Se configuran las reglas de acceso para el controlador School.
                    configuration.For<SchoolController>(x => x.Index()).RequireAnyRole(rolesSchoolIndex);

                    //configuration.For<SchoolController>(x => x.Create()).RequireAnyRole(rolesSchoolCreate);

                    configuration.For<SchoolController>(x => x.Edit(default(Guid))).RequireAnyRole(rolesSchoolEdit);

                    //configuration.For<SchoolController>(x => x.Delete(default(Guid))).RequireAnyRole(rolesSchoolDelete);

                    //configuration.For<SchoolController>(x => x.DeleteConfirmed(default(Guid))).RequireAnyRole(rolesSchoolDeleteConfirmed);

                    configuration.For<SchoolController>(x => x.Details(default(Guid))).RequireAnyRole(rolesSchoolDetails);

                    #endregion

                    #region Reglas del controlador StudyController
                    //Se obtienen los roles que tienen acceso al controlador Study y a los metodos de este controlador
                    string[] rolesStudyIndex = assembly.GetRolesMethods("StudyController", "Index");
                    var rolesStudyCreate = assembly.GetRolesMethods("StudyController", "Create");
                    var rolesStudyEdit = assembly.GetRolesMethods("StudyController", "Edit");
                    var rolesStudyDelete = assembly.GetRolesMethods("StudyController", "Delete");
                    var rolesStudyDeleteConfirmed = assembly.GetRolesMethods("StudyController", "DeleteConfirmed");
                    var rolesStudyDetails = assembly.GetRolesMethods("StudyController", "Details");


                    //Se configuran las reglas de acceso para el controlador Study.
                    configuration.For<StudyController>(x => x.Index(0)).RequireAnyRole(rolesStudyIndex);

                    configuration.For<StudyController>(x => x.Create(0)).RequireAnyRole(rolesStudyCreate);

                    configuration.For<StudyController>(x => x.Edit(default(Guid), 0)).RequireAnyRole(rolesStudyEdit);

                    configuration.For<StudyController>(x => x.Delete(default(Guid), 0)).RequireAnyRole(rolesStudyDelete);

                    configuration.For<StudyController>(x => x.DeleteConfirmed(default(Guid))).RequireAnyRole(rolesStudyDeleteConfirmed);

                    configuration.For<StudyController>(x => x.Details(default(Guid), 0)).RequireAnyRole(rolesStudyDetails);

                    #endregion

                    #region Reglas del controlador ThesisController
                    //Se obtienen los roles que tienen acceso al controlador Thesis y a los metodos de este controlador
                    string[] rolesThesisIndex = assembly.GetRolesMethods("ThesisController", "Index");
                    //var rolesThesisCreate = assembly.GetRolesMethods("ThesisController", "Create");
                    var rolesThesisEdit = assembly.GetRolesMethods("ThesisController", "Edit");
                    // var rolesThesisDelete = assembly.GetRolesMethods("ThesisController", "Delete");
                    //var rolesThesiseleteConfirmed = assembly.GetRolesMethods("ThesisController", "DeleteConfirmed");
                    var rolesThesisDetails = assembly.GetRolesMethods("ThesisController", "Details");


                    //Se configuran las reglas de acceso para el controlador Thesis.
                    configuration.For<ThesisController>(x => x.Index()).RequireAnyRole(rolesThesisIndex);

                    //configuration.For<ThesisController>(x => x.Create()).RequireAnyRole(rolesThesisCreate);

                    configuration.For<ThesisController>(x => x.Edit(default(Guid))).RequireAnyRole(rolesThesisEdit);

                    //configuration.For<ThesisController>(x => x.Delete(default(Guid))).RequireAnyRole(rolesThesisDelete);

                    //configuration.For<ThesisController>(x => x.DeleteConfirmed(default(Guid))).RequireAnyRole(rolesThesisDeleteConfirmed);

                    configuration.For<ThesisController>(x => x.Details(default(Guid))).RequireAnyRole(rolesThesisDetails);

                    #endregion

                    #region Reglas de reporte
                    string[] rolesReportsIndex = assembly.GetRolesMethods("ReportsController", "Index");
                    string[] rolesReportsDetails = assembly.GetRolesMethods("ReportsController", "Details");
                    string[] rolesReportsCreate = assembly.GetRolesMethods("ReportsController", "Create");
                    string[] rolesReportsEdit = assembly.GetRolesMethods("ReportsController", "Edit");
                    string[] rolesReportsDelete = assembly.GetRolesMethods("ReportsController", "Delete");
                    string[] rolesReportsDeleteConfirmed = assembly.GetRolesMethods("ReportsController", "DeleteConfirmed");
                    string[] rolesReportsPreview = assembly.GetRolesMethods("ReportsController", "Preview");
                    string[] rolesReportsRenderReport = assembly.GetRolesMethods("ReportsController", "RenderReport");



                    //Se configuran las reglas de acceso para el controlador surveys.
                    configuration.For<ReportsController>(x => x.Index())
                                 .RequireAnyRole(rolesReportsIndex);
                    configuration.For<ReportsController>(x => x.Create())
                                .RequireAnyRole(rolesReportsCreate);
                    configuration.For<ReportsController>(x => x.Details(default(Guid)))
                                .RequireAnyRole(rolesReportsDetails);
                    configuration.For<ReportsController>(x => x.Edit(default(Guid)))
                                .RequireAnyRole(rolesReportsEdit);
                    configuration.For<ReportsController>(x => x.Delete(default(Guid)))
                                .RequireAnyRole(rolesReportsDelete);
                    configuration.For<ReportsController>(x => x.DeleteConfirmed(default(Guid)))
                                .RequireAnyRole(rolesReportsDeleteConfirmed);
                    configuration.For<ReportsController>(x => x.Preview(default(Guid)))
                                .RequireAnyRole(rolesReportsPreview);
                    configuration.For<ReportsController>(x => x.RenderReport(default(Guid)))
                                .RequireAnyRole(rolesReportsRenderReport);
                    #endregion

                    #region Reglas de seguridad del controlador SurveysTopic

                    string[] rolesSurveysTopicsIndex = assembly.GetRolesMethods("SurveysTopicsController", "Index");
                    string[] rolesSurveysTopicsCreate = assembly.GetRolesMethods("SurveysTopicsController", "Create");
                    string[] rolesSurveysTopicsEdit = assembly.GetRolesMethods("SurveysTopicsController", "Edit");
                    string[] rolesSurveysTopicsDelete = assembly.GetRolesMethods("SurveysTopicsController", "Delete");
                    string[] rolesSurveysTopicsDeleteConfirmed = assembly.GetRolesMethods("SurveysTopicsController", "DeleteConfirmed");

                    configuration.For<SurveysTopicsController>(x => x.Index(default(Guid)))
                                 .RequireAnyRole(rolesSurveysTopicsIndex);
                    configuration.For<SurveysTopicsController>(x => x.Create(default(Guid)))
                                .RequireAnyRole(rolesSurveysTopicsCreate);
                    configuration.For<SurveysTopicsController>(x => x.Edit(default(Guid), default(Guid)))
                                .RequireAnyRole(rolesSurveysTopicsEdit);
                    configuration.For<SurveysTopicsController>(x => x.Delete(default(Guid), default(Guid)))
                                .RequireAnyRole(rolesSurveysTopicsDelete);
                    configuration.For<SurveysTopicsController>(x => x.DeleteConfirmed(default(Guid), default(Guid)))
                                .RequireAnyRole(rolesSurveysTopicsDeleteConfirmed);


                    #endregion

                    #region Reglas de seguridad del Controlador RoleMethods
                    string[] rolesRoleMethodsAssignRolesMethods = assembly.GetRolesMethods("RoleMethodsController", "AssignRolesMethods");
                    configuration.For<RoleMethodsController>(x => x.AssignRolesMethods(default(Guid)))
                                 .RequireAnyRole(rolesSurveysTopicsIndex);

                    #endregion

                    #region Reglas de seguridad del controlador
                    string[] rolesUsersRolesControllerAssignUserRoles = assembly.GetRolesMethods("UsersRolesController", "AssignUserRoles");
                    configuration.For<UsersRolesController>(x => x.AssignUserRoles(default(Guid)))
                                 .RequireAnyRole(rolesUsersRolesControllerAssignUserRoles);

                    #endregion

                    #region Reglas de seguridad del controlador Vacancies

                    //Se obtienen los roles que tienen acceso al controlador Vacancies y a los métodos de este controlador.
                    string[] rolesVacanciesIndex = assembly.GetRolesMethods("VacanciesController", "Index");
                    var rolesVacanciesIndex2 = assembly.GetRolesMethods("VacanciesController", "Index2");
                    var rolesVacanciesCreate = assembly.GetRolesMethods("VacanciesController", "Create");
                    var rolesVacanciesDetails = assembly.GetRolesMethods("VacanciesController", "Details");
                    var rolesVacanciesEdit = assembly.GetRolesMethods("VacanciesController", "Edit");
                    var rolesVacanciesDelete = assembly.GetRolesMethods("VacanciesController", "Delete");
                    var rolesVacanciesDeleteConfirmed = assembly.GetRolesMethods("VacanciesController", "DeleteConfirmed");
                    var rolesVacanciesSearch = assembly.GetRolesMethods("VacanciesController", "Search");


                    //Se configuran las reglas de acceso para el controlador Vacancies.

                    // configuration.For<VacanciesController>(x => x.Index(default(int)))
                    //           .RequireAnyRole(rolesVacanciesIndex);

                    configuration.For<VacanciesController>(x => x.Index(default(int)))
                        .Ignore();

                    //configuration.For<VacanciesController>(x => x.Index2())
                    //              .RequireAnyRole(rolesVacanciesIndex);

                    configuration.For<VacanciesController>(x => x.Index2())
                                 .Ignore();

                    configuration.For<VacanciesController>(x => x.Create())
                                .RequireAnyRole(rolesVacanciesCreate);

                    configuration.For<VacanciesController>(x => x.Details(default(Guid)))
                                 .RequireAnyRole(rolesVacanciesDetails);

                    configuration.For<VacanciesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesVacanciesEdit);

                    configuration.For<VacanciesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesVacanciesDelete);

                    configuration.For<VacanciesController>(x => x.DeleteConfirmed(default(Guid)))
                                 .RequireAnyRole(rolesVacanciesDeleteConfirmed);

                    configuration.For<VacanciesController>(x => x.Search(default(string), default(int)))
                                 .RequireAnyRole(rolesVacanciesSearch);

                    #endregion

                    #region Reglas de seguridad del controlador Companies

                    //Se obtienen los roles que tienen acceso al controlador Companies y a los métodos de este controlador.
                    string[] rolesCompaniesIndex = assembly.GetRolesMethods("CompaniesController", "Index");

                    var rolesCompaniesCreate = assembly.GetRolesMethods("CompaniesController", "Create");
                    var rolesCompaniesCreateForExperience = assembly.GetRolesMethods("CompaniesController", "CreateForExperience");
                    var rolesCompaniesDetails = assembly.GetRolesMethods("CompaniesController", "Details");
                    var rolesCompaniesEdit = assembly.GetRolesMethods("CompaniesController", "Edit");
                    var rolesCompaniesDelete = assembly.GetRolesMethods("CompaniesController", "Delete");
                    var rolesCompaniesDeleteConfirmed = assembly.GetRolesMethods("CompaniesController", "DeleteConfirmed");

                    //Se configuran las reglas de acceso para el controlador Companies.

                    configuration.For<CompaniesController>(x => x.Index())
                                .RequireAnyRole(rolesCompaniesIndex);


                    configuration.For<CompaniesController>(x => x.Create())
                                 .RequireAnyRole(rolesCompaniesCreate);

                    configuration.For<CompaniesController>(x => x.CreateForExperience(default(int)))
                                     .RequireAnyRole(rolesCompaniesCreateForExperience);

                    configuration.For<CompaniesController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesCompaniesDetails);

                    configuration.For<CompaniesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesCompaniesEdit);

                    configuration.For<CompaniesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesCompaniesDelete);

                    configuration.For<CompaniesController>(x => x.DeleteConfirmed(default(Guid)))
                                 .RequireAnyRole(rolesCompaniesDeleteConfirmed);

                    #endregion

                    #region Reglas de seguridad del controlador Experiences

                    //Se obtienen los roles que tienen acceso al controlador Experiences y a los métodos de este controlador.
                    string[] rolesExperiencesIndex = assembly.GetRolesMethods("ExperiencesController", "Index");

                    var rolesExperiencesCreate = assembly.GetRolesMethods("ExperiencesController", "Create");
                    var rolesExperiencesDetails = assembly.GetRolesMethods("ExperiencesController", "Details");
                    var rolesExperiencesEdit = assembly.GetRolesMethods("ExperiencesController", "Edit");
                    var rolesExperiencesDelete = assembly.GetRolesMethods("ExperiencesController", "Delete");
                    var rolesExperiencesDeleteConfirmed = assembly.GetRolesMethods("ExperiencesController", "DeleteConfirmed");

                    //Se configuran las reglas de acceso para el controlador Experiences.

                    configuration.For<ExperiencesController>(x => x.Index(0))
                                .RequireAnyRole(rolesExperiencesIndex);


                    configuration.For<ExperiencesController>(x => x.Create(0))
                                 .RequireAnyRole(rolesExperiencesCreate);

                    configuration.For<ExperiencesController>(x => x.Details(default(Guid), 0))
                                  .RequireAnyRole(rolesExperiencesDetails);

                    configuration.For<ExperiencesController>(x => x.Edit(default(Guid), 0))
                                 .RequireAnyRole(rolesExperiencesEdit);

                    configuration.For<ExperiencesController>(x => x.Delete(default(Guid), 0))
                                 .RequireAnyRole(rolesExperiencesDelete);

                    configuration.For<ExperiencesController>(x => x.DeleteConfirmed(default(Guid), default(FormCollection)))
                                 .RequireAnyRole(rolesExperiencesDeleteConfirmed);

                    #endregion

                    #region Reglas de seguridad del controlador Bosses

                    //Se obtienen los roles que tienen acceso al controlador Bosses y a los métodos de este controlador.
                    string[] rolesBossesIndex = assembly.GetRolesMethods("BossesController", "Index");

                    var rolesBossesCreate = assembly.GetRolesMethods("BossesController", "Create");
                    var rolesBossesDetails = assembly.GetRolesMethods("BossesController", "Details");
                    var rolesBossesEdit = assembly.GetRolesMethods("BossesController", "Edit");
                    var rolesBossesDelete = assembly.GetRolesMethods("BossesController", "Delete");
                    var rolesBossesDeleteConfirmed = assembly.GetRolesMethods("BossesController", "DeleteConfirmed");
                    var rolesBossesDetailsForExperiences = assembly.GetRolesMethods("BossesController", "DetailsForExperiences");
                    var rolesBossesCreateForExperienceBosses = assembly.GetRolesMethods("BossesController", "CreateForExperienceBosses");


                    //Se configuran las reglas de acceso para el controlador Bosses.

                    configuration.For<BossesController>(x => x.Index())
                                .RequireAnyRole(rolesBossesIndex);


                    configuration.For<BossesController>(x => x.Create())
                                 .RequireAnyRole(rolesBossesCreate);

                    configuration.For<BossesController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesBossesDetails);

                    configuration.For<BossesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesBossesEdit);

                    configuration.For<BossesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesBossesDelete);

                    configuration.For<BossesController>(x => x.DeleteConfirmed(default(Guid)))
                                 .RequireAnyRole(rolesBossesDeleteConfirmed);

                    configuration.For<BossesController>(x => x.DetailsForExperiences(default(Guid), 0))
                                    .RequireAnyRole(rolesBossesDetailsForExperiences);

                    configuration.For<BossesController>(x => x.CreateForExperienceBosses(default(Guid), 0))
                                    .RequireAnyRole(rolesBossesCreateForExperienceBosses);


                    #endregion

                    #region Reglas de seguridad del controlador ExperiencesBosses

                    //Se obtienen los roles que tienen acceso al controlador ExperiencesBosses y a los métodos de este controlador.
                    string[] rolesExperiencesBossesIndex = assembly.GetRolesMethods("ExperiencesBossesController", "Index");

                    var rolesExperiencesBossesCreate = assembly.GetRolesMethods("ExperiencesBossesController", "Create");
                    var rolesExperiencesBossesDetails = assembly.GetRolesMethods("ExperiencesBossesController", "Details");
                    var rolesExperiencesBossesEdit = assembly.GetRolesMethods("ExperiencesBossesController", "Edit");
                    var rolesExperiencesBossesDelete = assembly.GetRolesMethods("ExperiencesBossesController", "Delete");
                    var rolesExperiencesBossesDeleteConfirmed = assembly.GetRolesMethods("ExperiencesBossesController", "DeleteConfirmed");

                    //Se configuran las reglas de acceso para el controlador ExperiencesBosses.

                    configuration.For<ExperiencesBossesController>(x => x.Index())
                                .RequireAnyRole(rolesExperiencesBossesIndex);


                    configuration.For<ExperiencesBossesController>(x => x.Create(default(Guid), 0))
                                 .RequireAnyRole(rolesExperiencesBossesCreate);

                    configuration.For<ExperiencesBossesController>(x => x.Details(default(Guid)))
                                  .RequireAnyRole(rolesExperiencesBossesDetails);

                    configuration.For<ExperiencesBossesController>(x => x.Edit(default(Guid)))
                                 .RequireAnyRole(rolesExperiencesBossesEdit);

                    configuration.For<ExperiencesBossesController>(x => x.Delete(default(Guid)))
                                 .RequireAnyRole(rolesExperiencesBossesDelete);

                    configuration.For<ExperiencesBossesController>(x => x.DeleteConfirmed(default(Guid)))
                                 .RequireAnyRole(rolesExperiencesBossesDeleteConfirmed);

                    #endregion

                    #region Reglas de seguridad controlador Items

                    string[] rolesItem = assembly.GetRolesMethods("ItemsController", "GeneralItems");
                    configuration.For<ItemsController>(x => x.GeneralItems((default(Guid))))
                                .RequireAnyRole(rolesItem);

                    configuration.For<ItemSurveysController>(x => x.SurveysList()).Ignore();
                    configuration.For<ItemSurveysController>(x => x.QuestionsList(default(string))).Ignore();
                    configuration.For<ItemSurveysController>(x => x.TopicsList(default(string))).Ignore();
                    configuration.For<ItemDataController>(x => x.ListActions(default(string), default(string))).Ignore();
                    configuration.For<ItemDataController>(x => x.ListFields()).Ignore();
                    configuration.For<ItemDataController>(x => x.ListGroupFields()).Ignore();
                    configuration.For<ItemDataController>(x => x.ListLogic()).Ignore();
                    configuration.For<ItemDataController>(x => x.ListOperators(default(string), default(string))).Ignore();
                    configuration.For<ItemDataController>(x => x.ListSearchFields()).Ignore();


                    #endregion

                    #endregion

                    //****************************************************************************************************//
                    //****************************************************************************************************//
                    //****************************************************************************************************//




                });

                #endregion
                //Se añaden las reglas
           GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);
            

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //CreateSteps();
            
        }

        /*Lineas Necesarias, para Administrar el Wizard*/
        protected void Session_Start(object sender, EventArgs e)
        {
            /*Variable de Sesion para el Wizard*/

            Session["Wizard"] = "0";
            Session["User"] = string.Empty;
            Session["steps"] = null;
            Session["firstTime"] = false;
            /*--------------------------------*/

        }
    }

}