using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plossum.CommandLine;
using WinStoreSubmissionTool.CommandLine;

namespace WinStoreSubmissionTool
{
	class Program
	{
		static int Main(string[] args)
		{
			var submissionUpdateRunner =  new SubmissionUpdateRunner();
			return submissionUpdateRunner.RunSubmissionUpdate();
		}
	}
}
