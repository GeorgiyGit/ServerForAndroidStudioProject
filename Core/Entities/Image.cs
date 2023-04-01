using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Image:Monitoring
	{
		[System.ComponentModel.DataAnnotations.Key]
		public int Id { get; set; }
		public string Path { get; set; }
		public string FullName { get; set; }
		public string Name { get; set; }

		public User? User { get; set; }
	}
}
