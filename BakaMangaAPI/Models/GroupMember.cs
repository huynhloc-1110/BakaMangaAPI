namespace BakaMangaAPI.Models
{
    public class GroupMember
    {
        public string GroupId { get; set; } = default!;
        public Group Group { get; set; } = default!;

        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public bool IsLeader { get; set; }
    }
}
