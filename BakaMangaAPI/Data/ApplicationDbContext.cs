using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region DbSets
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Chapter> Chapters { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Image> Images { get; set; } = default!;
    public DbSet<Manga> Mangas { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<React> Reacts { get; set; } = default!;
    public DbSet<Report> Reports { get; set; } = default!;
    public DbSet<Request> Requests { get; set; } = default!;
    public DbSet<View> Views { get; set; } = default!;
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // do not remove, this is necessary when using IdentityDbContext
        base.OnModelCreating(modelBuilder);

        #region declare inheritance
        modelBuilder.Entity<MangaComment>().HasBaseType<Comment>();
        modelBuilder.Entity<ChapterComment>().HasBaseType<Comment>();
        modelBuilder.Entity<PostComment>().HasBaseType<Comment>();

        modelBuilder.Entity<CommentReact>().HasBaseType<React>();
        modelBuilder.Entity<PostReact>().HasBaseType<React>();

        modelBuilder.Entity<ChapterReport>().HasBaseType<Report>();
        modelBuilder.Entity<CommentReport>().HasBaseType<Report>();
        modelBuilder.Entity<PostReport>().HasBaseType<Report>();
        modelBuilder.Entity<UserReport>().HasBaseType<Report>();

        modelBuilder.Entity<ChapterView>().HasBaseType<View>();
        modelBuilder.Entity<CommentView>().HasBaseType<View>();
        modelBuilder.Entity<PostView>().HasBaseType<View>();

        modelBuilder.Entity<PromotionRequest>().HasBaseType<Request>();
        #endregion

        // set default delete behaviors to restrict
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
