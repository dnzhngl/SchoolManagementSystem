using Microsoft.EntityFrameworkCore;
using SMS.Model;
using System;
using System.Collections.Generic;
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
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<LessonTime> LessonTimes { get; set; }
        public DbSet<SemesterInformation> SemesterInformation { get; set; }
    }
}
