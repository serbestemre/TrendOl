﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendOl.Entities
{
	[Table("Comments")]
	public class Comment :MyEntityBase
	{
	
		[Required]

		
		public string Text { get; set; }

		[Required]
		public virtual MyUser Owner { get; set; }
		[Required]
		public virtual Product Product { get; set; }
	}
}
