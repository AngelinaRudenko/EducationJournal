using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Journal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

        public ApplicationUser()
        {
            Groups = new List<Group>();
            Subjects = new List<Subject>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class Group
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Код доступа")]
        public string Code { get; set; }
        [Display(Name = "Организатор")]
        public string CreatorId { get; set; }
        [Display(Name = "Участники")]
        public virtual ICollection<ApplicationUser> Members { get; set; }
        [Display(Name = "Предметы, преподаваемые в группе")]
        public virtual ICollection<Subject> Subjects { get; set; }

        public Group()
        {
            Members = new List<ApplicationUser>();
            Subjects = new List<Subject>();
        }
    }

    public class Subject
    {
        public int Id { get; set; }
        [Display(Name = "Название предмета")]
        public string Name { get; set; }
        public string TeacherId { get; set; }
        [Display(Name = "Группы, в которых читается предмет")]
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }

    public class Lesson
    {
        public int Id { get; set; }
        [Display(Name = "Тема урока")]
        public string Theme { get; set; }
        [Display(Name = "Дата проведения урока")]
        public DateTime Date { get; set; }
        public int SubjectId { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }

    public class Mark
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string StudentId { get; set; }
        [Display(Name = "Оценка")]
        public string Value { get; set; } // оценка/зачет
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    //public class DbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    //{
    //    protected override void Seed(ApplicationDbContext db)
    //    {
    //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
    //        // создаем две роли
    //        var role1 = new IdentityRole { Name = "Преподаватель" };
    //        var role2 = new IdentityRole { Name = "Ученик" };
    //        // добавляем роли в бд
    //        roleManager.Create(role1);
    //        roleManager.Create(role2);

    //        base.Seed(db);
    //    }
    //}
}