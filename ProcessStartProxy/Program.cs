using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Codeplex.Data;

namespace ProcessSatrtProxy
{
	class Program
	{
		static void Main(string[] args)
		{
			try {
				var configFile = args.Length > 0 ? args[0] : DefaultConfigFilePath;
				var opt = (Options)DynamicJson.Parse(File.ReadAllText(configFile, Encoding.UTF8));

				var psi = new ProcessStartInfo {
					FileName = opt.FileName,
					Arguments = opt.Arguments,
					WorkingDirectory = opt.WorkingDirectory ?? Path.GetDirectoryName(opt.FileName)
				};
				var process = new Process() {
					StartInfo = psi
				};

				if (process.Start() && opt.WaitForExit) {
					Console.WriteLine("Waiting process exit.");
					process.WaitForExit();
				}
			}
			catch (Exception ex) {
				var oldColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine(ex);
				Console.ForegroundColor = oldColor;
			}

			Console.WriteLine("Done.");
		}

		public class Options
		{
			public string FileName { get; set; }
			public string Arguments { get; set; }
			public string WorkingDirectory { get; set; }
			public bool WaitForExit { get; set; }
		}

		static string DefaultConfigFileName
		{
			get
			{
				return "ProcessStartProxy.config.txt";
			}
		}

		static string DefaultConfigFilePath
		{
			get
			{
				var exePath = Assembly.GetExecutingAssembly().Location;
				return Path.Combine(
					Path.GetDirectoryName(exePath),
					DefaultConfigFileName);
			}
		}
	}
}
