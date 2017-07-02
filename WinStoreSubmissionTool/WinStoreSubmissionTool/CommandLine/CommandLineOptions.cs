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

		[CommandLineOption(Description = "Specifies the input csv file", MinOccurs = 1, MaxOccurs = 1)]
		public string CsvInputFile { get; set; }

		[CommandLineOption(Description = "Specifies the output csv file", MinOccurs = 1, MaxOccurs = 1)]
		public string CsvOutputFile { get; set; }
	}
}
