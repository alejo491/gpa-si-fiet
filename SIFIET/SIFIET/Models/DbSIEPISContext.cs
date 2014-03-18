using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SIFIET.Models.Mapping;

namespace SIFIET.Models
{
    public partial class DbSIEPISContext : DbContext
    {
        static DbSIEPISContext()
        {
            Database.SetInitializer<DbSIEPISContext>(null);
        }

        public DbSIEPISContext()
            : base("Name=DbSIEPISContext")
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AnswerChoice> AnswerChoices { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public DbSet<Boss> Bosses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Elective> Electives { get; set; }
        public DbSet<Exemplar> Exemplars { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<GroupOption> GroupOptions { get; set; }
        public DbSet<ExemplarsQuestion> ExemplarsQuestions { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<ExperiencesBoss> ExperiencesBosses { get; set; }
        public DbSet<FreeField> FreeFields { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FilesPost> FilesPosts { get; set; }
        public DbSet<FreeFieldsValue> FreeFieldsValues { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemData> ItemDatas { get; set; }
        public DbSet<ItemSurvey> ItemSurveys { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<Surveyed> Surveyeds { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveysTopic> SurveysTopics { get; set; }
        public DbSet<aspnet_UsersInRoles> aspnet_UsersInRoles { get; set; }
        public DbSet<Thesis> Theses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersStep> UsersSteps { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<vw_aspnet_Applications> vw_aspnet_Applications { get; set; }
        public DbSet<vw_aspnet_MembershipUsers> vw_aspnet_MembershipUsers { get; set; }
        public DbSet<vw_aspnet_Profiles> vw_aspnet_Profiles { get; set; }
        public DbSet<vw_aspnet_Roles> vw_aspnet_Roles { get; set; }
        public DbSet<vw_aspnet_Users> vw_aspnet_Users { get; set; }
        public DbSet<vw_aspnet_UsersInRoles> vw_aspnet_UsersInRoles { get; set; }
        public DbSet<vw_aspnet_WebPartState_Paths> vw_aspnet_WebPartState_Paths { get; set; }
        public DbSet<vw_aspnet_WebPartState_Shared> vw_aspnet_WebPartState_Shared { get; set; }
        public DbSet<vw_aspnet_WebPartState_User> vw_aspnet_WebPartState_User { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<RoleMethod> RoleMethods { get; set; }
        public DbSet<SecureController> SecureControllers { get; set; }
        public DbSet<ConsultaGeneral> ConsultaGenerals { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Startbox> Startboxs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountMap());
            modelBuilder.Configurations.Add(new AnswerChoiceMap());
            modelBuilder.Configurations.Add(new AnswerMap());
            modelBuilder.Configurations.Add(new aspnet_ApplicationsMap());
            modelBuilder.Configurations.Add(new aspnet_MembershipMap());
            modelBuilder.Configurations.Add(new aspnet_PathsMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationAllUsersMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationPerUserMap());
            modelBuilder.Configurations.Add(new aspnet_ProfileMap());
            modelBuilder.Configurations.Add(new aspnet_RolesMap());
            modelBuilder.Configurations.Add(new aspnet_SchemaVersionsMap());
            modelBuilder.Configurations.Add(new aspnet_UsersMap());
            modelBuilder.Configurations.Add(new aspnet_WebEvent_EventsMap());
            modelBuilder.Configurations.Add(new BossMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ElectiveMap());
            modelBuilder.Configurations.Add(new ExemplarMap());
            modelBuilder.Configurations.Add(new ExemplarsQuestionMap());
            modelBuilder.Configurations.Add(new ExperienceMap());
            modelBuilder.Configurations.Add(new ExperiencesBossMap());
            modelBuilder.Configurations.Add(new FreeFieldMap());
            modelBuilder.Configurations.Add(new FreeFieldsValueMap());
            modelBuilder.Configurations.Add(new FieldMap());
            modelBuilder.Configurations.Add(new FileMap());
            modelBuilder.Configurations.Add(new FilesPostMap());
            modelBuilder.Configurations.Add(new FilterMap());
            modelBuilder.Configurations.Add(new ItemDataMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new ItemSurveyMap());
            modelBuilder.Configurations.Add(new GroupOptionMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new SchoolMap());
            modelBuilder.Configurations.Add(new StepMap());
            modelBuilder.Configurations.Add(new StudyMap());
            modelBuilder.Configurations.Add(new SurveyedMap());
            modelBuilder.Configurations.Add(new SurveyMap());
            modelBuilder.Configurations.Add(new SurveysTopicMap());
            modelBuilder.Configurations.Add(new aspnet_UsersInRolesMap());
            modelBuilder.Configurations.Add(new ThesisMap());
            modelBuilder.Configurations.Add(new TopicMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UsersStepMap());
            modelBuilder.Configurations.Add(new VacancyMap());
            modelBuilder.Configurations.Add(new ConsultaGeneralMap());
            modelBuilder.Configurations.Add(new vw_aspnet_ApplicationsMap());
            modelBuilder.Configurations.Add(new vw_aspnet_MembershipUsersMap());
            modelBuilder.Configurations.Add(new vw_aspnet_ProfilesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_RolesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_UsersMap());
            modelBuilder.Configurations.Add(new vw_aspnet_UsersInRolesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_PathsMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_SharedMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_UserMap());
            modelBuilder.Configurations.Add(new MethodMap());
            modelBuilder.Configurations.Add(new RoleMethodMap());
            modelBuilder.Configurations.Add(new SecureControllerMap());
            modelBuilder.Configurations.Add(new LikeMap());
            modelBuilder.Configurations.Add(new StartboxMap());
        }
    }
}
