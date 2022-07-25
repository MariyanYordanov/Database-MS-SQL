namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(sc => new
                {
                    sc.StudentId,
                    sc.CourseId,
                });
            });

            modelBuilder
                .Entity<Student>()
                .Property(x => x.PhoneNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder
                .Entity<Resource>()
                .Property(x => x.Url)
                .IsUnicode(false);

            modelBuilder
                .Entity<Homework>()
                .Property(x => x.Content)
                .IsUnicode(false);
        }

    }
}
