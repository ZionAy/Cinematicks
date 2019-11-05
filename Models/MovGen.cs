using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematicks.Models
{
    public class MovGen
    {
		public int MovieID { get; set; }
		public int GenreID { get; set; }

		public virtual Movie Movie { get; set; }
		public virtual Genre Genre { get; set; }
	}
}
