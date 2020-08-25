using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SMS.DAL
{
    public class SMSDbContext : DbContext
    {
        public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options)
        {

        }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<MainSubject> MainSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<LessonTime> LessonTimes { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Timetable> Timetables { get; set; }

        public DbSet<TimetableView> TimetableViews { get; set; }  //DbQuery ile de yapabilirsin.


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TimetableView'i kullanabilmek için
            modelBuilder.Entity<TimetableView>(d =>
            {
                d.HasKey("Id");
                d.ToView("View_Timetables");
            });

            //modelBuilder.Entity<TimetableView>().ToView("TimetableView");

        }
    }



    //Normalde MVC de model klasöründe bulunan DbContext, BLL yani Class Library projesinde bulunduğu için Migration Ekleme'de "Unable to create an object of type 'SMSDbContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728" böyle bir hata veriyor. Bunun önüne geçmek için aşağıdaki kod bloğunu yazmamız gerek. Burada DBContext'i nasıl başlatacığını biz belirtiyoruz.

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SMSDbContext>
    {
        public SMSDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../SMS.WebUI/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<SMSDbContext>();
            var connectionString = configuration.GetConnectionString("SMSDbConnection");
            builder.UseSqlServer(connectionString);
            return new SMSDbContext(builder.Options);
        }
    }
}
