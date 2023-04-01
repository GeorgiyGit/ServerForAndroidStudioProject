using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Monitoring
	{
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public bool IsEdited { get; set; }
		public bool IsDeleted { get; set; }
	}
}
