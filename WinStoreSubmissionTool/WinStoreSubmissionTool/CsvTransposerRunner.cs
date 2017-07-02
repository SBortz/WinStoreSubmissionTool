using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plossum.CommandLine;
using WinStoreSubmissionTool.Csv;

namespace WinStoreSubmissionTool.CommandLine
{
	public class CsvTransposerRunner
	{
		private CommandLineParser commandLineParser;
		private CommandLineOptions commandLineOptions;


		public CsvTransposerRunner()
		{
			this.commandLineOptions = new CommandLineOptions();
			this.commandLineParser = new CommandLineParser(commandLineOptions);
		}

		public int RunSubmissionUpdate()
		{
			return this.RunSubmissionUpdateAsync().GetAwaiter().GetResult();
		}

		public async Task<int> RunSubmissionUpdateAsync()
		{
			this.ParseCommandLine();

			Console.WriteLine(this.commandLineParser.UsageInfo.GetHeaderAsString(78));

			if (!this.Validate())
			{
				return -1;
			}

			if (this.commandLineOptions.Help)
			{
				this.OutputHelp();
				return 0;
			}

			var csvTransposer = new CsvTransposer(commandLineOptions.CsvInputFile, commandLineOptions.CsvOutputFile);
			await csvTransposer.Transpose().ConfigureAwait(false);

			return 0;
		}

		private void OutputHelp()
		{
			Console.WriteLine(this.commandLineParser.UsageInfo.GetOptionsAsString(78));
		}

		private void ParseCommandLine()
		{
			this.commandLineParser.Parse();
		}

		private bool Validate()
		{
			if (this.commandLineParser.HasErrors)
			{
				Console.WriteLine(this.commandLineParser.UsageInfo.GetErrorsAsString(78));
				{
					{
						return false;
					}
				}
			}
			return true;
		}

	}
}
