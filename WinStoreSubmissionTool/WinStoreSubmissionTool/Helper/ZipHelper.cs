using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace WinStoreSubmissionTool
{
	public static class ZipHelper
	{
		public static void ZipAppPackage(string zipFilename, string appPackageFilePath)
		{
			File.Delete(zipFilename);

			using (ZipFile zip = new ZipFile())
			{
				zip.AddFile(appPackageFilePath);
				zip.Save(zipFilename);
			}
		}
	}
}
