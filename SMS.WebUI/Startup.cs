using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMS.BLL.Abstract;
using SMS.BLL.SMSService;
using SMS.Core.Data.UnitOfWork;
using SMS.DAL;
using SMS.Mapping.ConfigProfile;
using SMS.WebUI.CustomHandler;

namespace SMS.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            MapperConfig.RegisterMappers();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var optionsBuilder = new DbContextOptionsBuilder<SMSDbContext>();

            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SMSDbConnection"));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.EnableSensitiveDataLogging();

            services.AddSingleton<DbContext>(new SMSDbContext(optionsBuilder.Options));

            using (var context = new SMSDbContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            #region Authorization
            services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", config =>
            {
                config.Cookie.Name = "UserLoginCookie";
                config.LoginPath = "/Login";
                config.AccessDeniedPath = "/AccessDenied";
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.Name);
                });
            });

            services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            #endregion


            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IInstructorService, InstructorService>();
            services.AddSingleton<IBranchService, BranchService>();

            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<IParentService, ParentService>();

            services.AddSingleton<IGradeService, GradeService>();
            services.AddSingleton<ISectionService, SectionService>();

            services.AddSingleton<IMainSubjectService, MainSubjectService>();
            services.AddSingleton<ISubjectService, SubjectService>();

            services.AddSingleton<IExamTypeService, ExamTypeService>();
            services.AddSingleton<IExamService, ExamService>();
            services.AddSingleton<IExamResultService, ExamResultService>();

            services.AddSingleton<ISemesterService, SemesterService>();

            services.AddSingleton<IAttendanceTypeService, AttendanceTypeService>();
            services.AddSingleton<IAttendanceService, AttendanceService>();

            services.AddSingleton<ILessonTimeService, LessonTimeService>();
            services.AddSingleton<IDayService, DayService>();
            services.AddSingleton<IClassroomService, ClassroomService>();
            services.AddSingleton<ITimetableService, TimetableService>();

            services.AddSingleton<ITimetableViewService, TimetableViewService>();

            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAdminService, AdminService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Login",
                    pattern: "Login",
                    defaults: new { controller = "Login", action = "UserLogin" });

                endpoints.MapControllerRoute(
                     name: "Logout",
                     pattern: "Logout",
                     defaults: new { controller = "Login", action = "Logout" });



                endpoints.MapControllerRoute(
                   name: "SubjectAdd",
                   pattern: "Subject/SubjectAdd",
                   defaults: new { controller = "Subject", action = "SubjectAdd" });

                endpoints.MapControllerRoute(
                   name: "SubjectList",
                   pattern: "Subject/SubjectList",
                   defaults: new { controller = "Subject", action = "SubjectList" });





                endpoints.MapControllerRoute(
                   name: "MainSubjectAdd",
                   pattern: "MainSubject/MainSubjectAdd",
                   defaults: new { controller = "MainSubject", action = "MainSubjectAdd" });

                endpoints.MapControllerRoute(
                   name: "MainSubjectList",
                   pattern: "MainSubject/MainSubjectList",
                   defaults: new { controller = "MainSubject", action = "MainSubjectList" });




                endpoints.MapControllerRoute(
                   name: "ParentAdd",
                   pattern: "Parent/ParentAdd",
                   defaults: new { controller = "Parent", action = "ParentAdd" });

                endpoints.MapControllerRoute(
                   name: "ParentList",
                   pattern: "Parent/ParentList",
                   defaults: new { controller = "Parent", action = "ParentList" });



                endpoints.MapControllerRoute(
                    name: "StudentUpdate",
                    pattern: "Student/StudentUpdate",
                    defaults: new { controller = "Student", action = "StudentUpdate" });

                endpoints.MapControllerRoute(
                    name: "StudentDetails",
                    pattern: "Student/StudentDetails",
                    defaults: new { controller = "Student", action = "StudentDetails" });

                endpoints.MapControllerRoute(
                   name: "RegistrationAdd",
                   pattern: "Student/RegistrationAdd",
                   defaults: new { controller = "Student", action = "RegistrationAdd" });

                endpoints.MapControllerRoute(
                   name: "RegistrationList",
                   pattern: "Student/RegistrationList",
                   defaults: new { controller = "Student", action = "RegistrationList" });




                endpoints.MapControllerRoute(
                  name: "SectionAdd",
                  pattern: "Section/SectionAdd",
                  defaults: new { controller = "Section", action = "SectionAdd" });

                endpoints.MapControllerRoute(
                  name: "SectionList",
                  pattern: "Section/SectionList",
                  defaults: new { controller = "Section", action = "SectionList" });





                endpoints.MapControllerRoute(
                    name: "GradeAdd",
                    pattern: "Grade/GradeAdd",
                    defaults: new { controller = "Grade", action = "GradeAdd" });

                endpoints.MapControllerRoute(
                  name: "GradeList",
                  pattern: "Grade/GradeList",
                  defaults: new { controller = "Grade", action = "GradeList" });





                endpoints.MapControllerRoute(
                   name: "InstructorAdd",
                   pattern: "Instructor/InstructorAdd",
                   defaults: new { controller = "Instructor", action = "InstructorAdd" });

                endpoints.MapControllerRoute(
                   name: "InstructorList",
                   pattern: "Instructor/InstructorList",
                   defaults: new { controller = "Instructor", action = "InstructorList" });





                endpoints.MapControllerRoute(
                   name: "BranchAdd",
                   pattern: "Branch/BranchAdd",
                   defaults: new { controller = "Branch", action = "BranchAdd" });

                endpoints.MapControllerRoute(
                   name: "BranchList",
                   pattern: "Branch/BranchList",
                   defaults: new { controller = "Branch", action = "BranchList" });




                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");

            });
        }
    }
}
