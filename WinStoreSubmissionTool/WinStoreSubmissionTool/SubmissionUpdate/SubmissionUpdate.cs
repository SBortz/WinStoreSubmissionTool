using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace WinStoreSubmissionTool
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Newtonsoft.Json.Linq;

	public class SubmissionUpdate
	{
		private ClientConfiguration ClientConfig;
		private StoreSubmissionRestApiClient submissionApiClient;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="c">An instance of ClientConfiguration that contains all parameters populated</param>
		public SubmissionUpdate(StoreSubmissionRestApiClient submissionApiClient, ClientConfiguration c)
		{
			this.submissionApiClient = submissionApiClient;
			this.ClientConfig = c;
		}

		public async Task<bool> RunAppSubmissionUpdate()
		{
			var appId = this.ClientConfig.ApplicationId;
			var clientId = this.ClientConfig.ClientId;
			var clientSecret = this.ClientConfig.ClientSecret;
			var serviceEndpoint = this.ClientConfig.ServiceUrl;
			var tokenEndpoint = this.ClientConfig.TokenEndpoint;
			var appPackageFilePath = this.ClientConfig.AppPackageFilePath;

			// Get authorization token
			Console.WriteLine("Getting authorization token ");
			string accessToken = await IngestionClient.GetClientCredentialAccessToken(
					tokenEndpoint,
					clientId,
					clientSecret);
			
			Console.WriteLine("Getting application ");
			var client = new IngestionClient(accessToken, serviceEndpoint);
			var app = await this.submissionApiClient.GetAppAsync(client, appId);
			Console.WriteLine(app.ToString());

			// Let's get the last published submission, and print its contents, just for information
			if (app.lastPublishedApplicationSubmission == null)
			{
				// It is not possible to create the very first submission through the API
				Console.WriteLine("You need at least one published submission to create new submissions through API.");
				return false;
			}

			// Let's see if there is a pending submission. Warning! If it was created through the API,
			// it will be deleted so that we could create a new one in its stead.
			if (app.pendingApplicationSubmission != null)
			{
				var submissionId = app.pendingApplicationSubmission.id.Value as string;

				// Try deleting it. If it was NOT created via the API, then you need to manually
				// delete it from the dashboard. This is done as a safety measure to make sure that a
				// user and an automated system don't make conflicting edits.
				Console.WriteLine("Deleting the pending submission...");

				await this.submissionApiClient.DeletePendingSubmissionAsync(client, appId, submissionId);
			}

			// Create a new submission, which will be an exact copy of the last published submission.
			Console.WriteLine("Creating a new cloned submission...");
			var clonedSubmission = await this.submissionApiClient.CreateClonedSubmissionAsync(client, appId);

			// Update packages
			// Let's say we want to delete the existing package:
			if (this.ClientConfig.RemovePackages)
			{
				foreach (var applicationPackage in clonedSubmission.applicationPackages)
				{
					Console.WriteLine("Deleting package " + applicationPackage.fileName);
					applicationPackage.fileStatus = "PendingDelete";
				}
			}
			
			// Now, let's add a new package
			var packages = new List<dynamic>();
			packages.Add(clonedSubmission.applicationPackages[0]);
			packages.Add(
				new
				{
					fileStatus = "PendingUpload",
					fileName = appPackageFilePath,
				});

			clonedSubmission.applicationPackages = JToken.FromObject(packages.ToArray());
			var clonedSubmissionId = clonedSubmission.id.Value as string;

			// Uploaded the zip archive with all new files to the SAS url returned with the submission
			var fileUploadUrl = clonedSubmission.fileUploadUrl.Value as string;
			Console.WriteLine("FileUploadUrl: " + fileUploadUrl);
			Console.WriteLine("Uploading file");

			string zipFilename = "apppackages.zip";

			ZipHelper.ZipAppPackage(zipFilename, appPackageFilePath);

			await IngestionClient.UploadFileToBlob(zipFilename, fileUploadUrl).ConfigureAwait(false);
			
			// Update the submission
			Console.WriteLine("Updating the submission...");
			await this.submissionApiClient.UpdateSubmissionAsync(client, appId, clonedSubmissionId, clonedSubmission);

			// Tell the system that we are done updating the submission.
			// Update the submission
			if (this.ClientConfig.Commit)
			{
				if (!await Commit(client, appId, clonedSubmissionId))
				{
					return false;
				}
			}

			return true;
		}

		private async Task<bool> Commit(IngestionClient client, string appId, string clonedSubmissionId)
		{
			Console.WriteLine("Committing the submission...");
			await this.submissionApiClient.CommitSubmissionAsync(client, appId, clonedSubmissionId);

			// Let's periodically check the status until it changes from "CommitsStarted" to either
			// successful status or a failure.
			Console.WriteLine("Waiting for the submission commit processing to complete. This may take a couple of minutes.");
			TimeSpan checkStatusTimeMax = TimeSpan.FromMinutes(30);
			DateTime checkStatusStartTime = DateTime.Now;

			string submissionStatus = null;
			dynamic statusResource;
			do
			{
				await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
				statusResource = this.submissionApiClient.GetSubmissionStatusAsync(client, appId, clonedSubmissionId);

				submissionStatus = statusResource.status.Value as string;
				Console.WriteLine("Current status: " + submissionStatus);
			} while ("CommitStarted".Equals(submissionStatus) && 
					DateTime.Now.Subtract(checkStatusStartTime) < checkStatusTimeMax);

			if ("CommitFailed".Equals(submissionStatus))
			{
				Console.WriteLine("Submission has failed:");
				return false;
			}

			Console.WriteLine(statusResource.ToString());

			//			else
			//			{
			//				Console.WriteLine("Submission committed successfully:");
			//				var submission = await this.submissionApiClient.GetSubmissionAsync(client, appId, clonedSubmissionId);
			//				Console.WriteLine("Packages: " + submission.applicationPackages);
			//			}
			return true;
		}
	}
}
