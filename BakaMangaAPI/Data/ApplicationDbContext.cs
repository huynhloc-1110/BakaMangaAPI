using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BakaMangaAPI.Data;

public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region DbSets
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = default!;
    public DbSet<ApplicationRole> ApplicationRoles { get; set; } = default!;
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Chapter> Chapters { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<GroupMember> GroupMembers { get; set; } = default!;
    public DbSet<MangaComment> MangaComments { get; set; } = default!;
    public DbSet<Page> Pages { get; set; } = default!;
    public DbSet<Manga> Mangas { get; set; } = default!;
    public DbSet<MangaList> MangaLists { get; set; } = default!;
    public DbSet<MangaListItem> MangaListItems { get; set; } = default!;
    public DbSet<MangaListFollower> MangaListFollowers { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Rating> Ratings { get; set; } = default!;
    public DbSet<React> Reacts { get; set; } = default!;
    public DbSet<CommentReact> CommentReacts { get; set; } = default!;
    public DbSet<Report> Reports { get; set; } = default!;
    public DbSet<Request> Requests { get; set; } = default!;
    public DbSet<View> Views { get; set; } = default!;
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // do not remove, this is necessary when using IdentityDbContext
        base.OnModelCreating(modelBuilder);

        #region Declare Inheritance
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
        modelBuilder.Entity<PostView>().HasBaseType<View>();

        modelBuilder.Entity<PromotionRequest>().HasBaseType<Request>();
        #endregion

        #region Configure relationships
        modelBuilder.Entity<ApplicationUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
        });
        modelBuilder.Entity<GroupMember>().HasKey(gp => new { gp.GroupId, gp.UserId });
        modelBuilder.Entity<MangaListItem>().HasKey(i => new { i.MangaListId, i.MangaId });
        modelBuilder.Entity<MangaListFollower>().HasKey(f => new { f.MangaListID, f.UserId });
        #endregion

        #region Configure Global Filter
        modelBuilder.Entity<Author>().HasQueryFilter(a => a.DeletedAt == null);

        modelBuilder.Entity<Category>().HasQueryFilter(c => c.DeletedAt == null);

        modelBuilder.Entity<Manga>().HasQueryFilter(m => m.DeletedAt == null);
        modelBuilder.Entity<MangaListItem>().HasQueryFilter(i => i.Manga.DeletedAt == null);
        modelBuilder.Entity<Rating>().HasQueryFilter(i => i.Manga.DeletedAt == null);

        modelBuilder.Entity<Group>().HasQueryFilter(g => g.DeletedAt == null);
        modelBuilder.Entity<GroupMember>().HasQueryFilter(gm => gm.Group.DeletedAt == null);
        #endregion
    }
}
