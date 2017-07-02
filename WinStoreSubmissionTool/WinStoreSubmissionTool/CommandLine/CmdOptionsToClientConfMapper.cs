using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinStoreSubmissionTool.CommandLine
{
	public static class CmdOptionsToClientConfMapper
	{
		public static ClientConfiguration Map(CommandLineOptions options)
		{
			var config = new ClientConfiguration()
			{
//				ApplicationId = options.StoreAppId,
//				InAppProductId = "...",
//				FlightId = "...",
//				ClientId = options.ClientId,
//				ClientSecret = options.ClientSecret,
//				ServiceUrl = "https://manage.devcenter.microsoft.com",
//				TokenEndpoint = string.Format("https://login.microsoftonline.com/{0}/oauth2/token", options.TenantId),
//				Scope = "https://manage.devcenter.microsoft.com",
//				AppPackageFilePath = options.CsvInputFile,
//				RemovePackages = options.RemovePreviousPackages,
//				Commit = options.Commit,
//				DeletePendingSubmission = options.DeletePendingSubmission
			};
			return config;
		}
	}
}
