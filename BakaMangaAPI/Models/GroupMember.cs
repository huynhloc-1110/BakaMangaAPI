namespace BakaMangaAPI.Models
{
    public class GroupMember
    {
        public Group Group { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;

        public bool IsLeader { get; set; }
    }
}
