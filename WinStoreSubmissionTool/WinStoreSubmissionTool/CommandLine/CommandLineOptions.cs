using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plossum.CommandLine;

namespace WinStoreSubmissionTool
{
	[CommandLineManager(ApplicationName = "WinStoreSubmissionTool",
		Copyright = "Copyright (c) Sebastian Bortz")]
	public class CommandLineOptions
	{
		private string appPackage;

		[CommandLineOption(Description = "Submits an app package to Windows Store.")]
		public bool Help = false;

		[CommandLineOption(Description = "Specifies the app package file", MinOccurs = 1, MaxOccurs = 1)]
		public string AppPackage { get; set; }

		[CommandLineOption(Description = "Store-Id of the app. ", MinOccurs = 1, MaxOccurs = 1)]
		public string StoreAppId { get; set; }

		[CommandLineOption(Description = "ClientId for registered Azure AD App", MinOccurs = 1, MaxOccurs = 1)]
		public string ClientId { get; set; }

		[CommandLineOption(Description = "ClientSecret for registered Azure AD App", MinOccurs = 1, MaxOccurs = 1)]
		public string ClientSecret { get; set; }

		[CommandLineOption(Description = "Azure AD Tenant ID (p.a. myname.onmicrosoft.com)", MinOccurs = 1, MaxOccurs = 1)]
		public string TenantId { get; set; }

		[CommandLineOption(Description = "Commit submission to store.", MaxOccurs = 1)]
		public bool Commit { get; set; }

		[CommandLineOption(Description = "Remove app packages from previous submission.", MaxOccurs = 1)]
		public bool RemovePreviousPackages { get; set; }

//		[CommandLineOption(Description = "Deletes pending submission (works only with submissions created by API).")]
//		public bool DeletePendingSubmission { get; set; }
	}
}
