using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinStoreSubmissionTool.Csv
{
	public class CsvTransposer
	{
		private string inputFileName;
		private string outputFileName;

		private int columns = 0;

		public CsvTransposer(string inputFileName, string outputFileName)
		{
			this.inputFileName = inputFileName;
			this.outputFileName = outputFileName;
		}

		public async Task Transpose()
		{
			List<string> sourceLines = File.ReadLines(this.inputFileName).ToList();
			List<string> outputLines = new List<string>();

			// Read data into 2D array
			string[][] sourceData = new string[sourceLines.Count()][];
			for(int i = 0; i < sourceLines.Count(); i++)
			{
				sourceData[i] = sourceLines[i].Split(';');

				if (this.columns < sourceData[i].Length)
				{
					this.columns = sourceData[i].Length;
				}
			}

			try
			{
				// Write transposed data
				for (int i = 0; i < this.columns; i++)
				{
					StringBuilder line = new StringBuilder();
					for (int j = 0; j < sourceData.Length; j++)
					{
						if (j != 0)
						{
							line.Append(";");
						}

						if (sourceData[j][i] != null)
						{
							line.Append(sourceData[j][i]);
						}
					}

					outputLines.Add(line.ToString());
				}

				File.WriteAllLines(this.outputFileName, outputLines);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
