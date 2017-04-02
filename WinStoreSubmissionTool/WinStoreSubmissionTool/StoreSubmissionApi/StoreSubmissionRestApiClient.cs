using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace WinStoreSubmissionTool
{
	public class StoreSubmissionRestApiClient
	{
		public async Task<dynamic> GetSubmissionAsync(IngestionClient client, string appId, string clonedSubmissionId)
		{
			dynamic submission = await client.Invoke<dynamic>(
				HttpMethod.Get,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.GetSubmissionUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId,
					clonedSubmissionId),
				requestContent: null);
			return submission;
		}

		public dynamic GetSubmissionStatusAsync(IngestionClient client, string appId, string clonedSubmissionId)
		{
			dynamic statusResource;
			statusResource = client.Invoke<dynamic>(
				HttpMethod.Get,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.ApplicationSubmissionStatusUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId,
					clonedSubmissionId),
				requestContent: null).Result;
			return statusResource;
		}

		public async Task CommitSubmissionAsync(IngestionClient client, string appId, string clonedSubmissionId)
		{
			dynamic commitResponse = await client.Invoke<dynamic>(
				HttpMethod.Post,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.CommitSubmissionUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId,
					clonedSubmissionId),
				requestContent: null).ConfigureAwait(false);
		}

		public async Task UpdateSubmissionAsync(IngestionClient client, string appId, string clonedSubmissionId,
			dynamic clonedSubmission)
		{
			dynamic updateResponse = await client.Invoke<dynamic>(
				HttpMethod.Put,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.UpdateUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId,
					clonedSubmissionId),
				requestContent: clonedSubmission);
		}


		public async Task<dynamic> CreateClonedSubmissionAsync(IngestionClient client, string appId)
		{
			dynamic clonedSubmission = await client.Invoke<dynamic>(
				HttpMethod.Post,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.CreateSubmissionUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId),
				requestContent: null).ConfigureAwait(false);
			return clonedSubmission;
		}

		public async Task DeletePendingSubmissionAsync(IngestionClient client, string appId, string submissionId)
		{
			await client.Invoke<dynamic>(
				HttpMethod.Delete,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.GetSubmissionUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId,
					submissionId),
				requestContent: null);
		}

		public async Task<dynamic> GetAppAsync(IngestionClient client, string appId)
		{
			dynamic app = await client.Invoke<dynamic>(
				HttpMethod.Get,
				relativeUrl: string.Format(
					CultureInfo.InvariantCulture,
					IngestionClient.GetApplicationUrlTemplate,
					IngestionClient.Version,
					IngestionClient.Tenant,
					appId),
				requestContent: null);
			return app;
		}
	}
}
