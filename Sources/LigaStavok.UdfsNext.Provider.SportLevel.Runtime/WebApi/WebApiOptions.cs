﻿using System;
using System.Runtime.Serialization;

namespace LigaStavok.UdfsNext.Provider.SportLevel.WebApi
{
	public class WebApiOptions
	{
		public Uri Uri { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
	}
}