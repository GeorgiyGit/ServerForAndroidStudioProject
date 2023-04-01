using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
	public class User:IdentityUser
	{
		public Image? Avatar { get; set; }
		public int? ImageId { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public bool IsEdited { get; set; }
		public bool IsDeleted { get; set; }
	}
}
