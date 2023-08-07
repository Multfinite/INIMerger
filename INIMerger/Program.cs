using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace INIMerger
{
	internal class Program
	{
		public delegate void ArgumentHandler(List<string> args);

		static void Main(string[] a)
		{
			bool traceSection = false;
			bool traceOnlyName = false;

			List<string> args = new List<string>(a);
			var handlers = new Dictionary<string, ArgumentHandler>();
			handlers["-tr"] = (List<string> arguments) =>
			{
				traceSection = true;
				traceOnlyName = false;
			};
			handlers["-tron"] = (List<string> arguments) =>
			{
				traceSection = true;
				traceOnlyName = true;
			};

			int si = 0;
			foreach(var arg in args)
			{
				if(handlers.ContainsKey(arg))
				{
					handlers[arg](args);
					si++;
				}
			}

			if(args.Count < 3)
			{
				Console.WriteLine("format: INIMerger [-tr|-trn] %result file% %file1 %file2% ... %filen%, - be sure that last file have greatest priority on merge and will overwrite values");
				return;
			}

			var parser = new FileIniDataParser();

			List<IniData> items = new List<IniData>();
			IniData trSect = new IniData();
			for(int i = si + 1; i < args.Count; i++)
			{
				FileInfo fi = new FileInfo(args[i]);
				if(!fi.Exists)
				{
					Console.WriteLine($"A file {fi.FullName} does not exist, skipping");
					continue;
				}
				var data = parser.ReadFile(fi.FullName);
				items.Add(data);
				var tr = traceOnlyName ? fi.Name : args[i];
				trSect["merger_trace_section"][tr] = tr;
			}

			IniData resultData = new IniData();

			//items.Reverse();
			foreach(var item in items)
				resultData.Merge(item);

			if(traceSection)
				resultData.Merge(trSect);			

			FileInfo rfi = new FileInfo(args[si]);
			string dumped = resultData.ToString();
			dumped = dumped.Replace(" = ", "=");
			//parser.WriteFile(rfi.FullName, resultData, System.Text.Encoding.UTF8);

			File.WriteAllText(rfi.FullName, dumped);
			Console.WriteLine($"Saved to {rfi.FullName}");
		}
	}
}
