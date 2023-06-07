using GymEats.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymEats.Data
{
    public class GymEatsDbContext : IdentityDbContext<User>
    {
        
        public GymEatsDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Diet> Diet { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Survey> Survey { get; set; }
        //public DbSet<SurveyQuestionMapping> SurveyQuestionMapping { get; set; }
        public DbSet<ShoppingList> ShoppingList { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<SurveyOption> SurveyOption { get; set;}
        public DbSet<SurveyDiet> SurveyDiet { get;set; }
        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
    }
}
