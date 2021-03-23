using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCoreApplication.Model
{
	public class TokenResponseViewModel
	{
		public string Token  { get; set; }
		public string UserName { get; set; }
		public int ExpiredOn { get; set; }
	}
}
