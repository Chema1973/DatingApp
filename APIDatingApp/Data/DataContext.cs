using APIDatingApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace APIDatingApp.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>> 
        //: DbContext // --> Por meter el Identitity
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // public DbSet<AppUser> Users {get; set;}
        // --> Por meter el Identitity

        public DbSet<UserLike> Likes {get; set;}

        public DbSet<Message> Messages {get; set;}

        public DbSet<Group> Groups {get; set;}

        public DbSet<Connection> Connections {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // INI: Configuración de tablas de Identity

            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // FIN: Configuración de tablas de Identity

            modelBuilder.Entity<UserLike>()
                .HasKey(k => new {k.SourceUserId, k.TargetUserId});

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // --> Una persona (SourceUser) tiene a muchas que le gustan (LikedUsers)

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // --> Una persona (TargetUser) es "gustada" por muchos (LikedByUsers)

            modelBuilder.Entity<Message>()
                .HasOne(s => s.Recipient)
                .WithMany(l => l.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            // --> Una persona (Recipient) tiene muchos mensajes recibidos (MessagesReceived)

            modelBuilder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(l => l.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            // --> Una persona (Sender) tiene muchas mensajes enviados (MessagesSent)



        }
    }
}