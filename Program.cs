using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubdirSorter
{
	class Program
	{
		static bool IsLetter(char ch)
		{
			return Regex.IsMatch(ch.ToString(), @"[а-яА-Яa-zA-Z]+$");
		}

		static void Main(string[] args)
		{
			string errors = string.Empty;
			var MyIni = new IniFile();
			var currPath = AppDomain.CurrentDomain.BaseDirectory;
			var Source = string.Empty;
			Source = MyIni.Read(nameof(Source));
			Console.WriteLine($"{nameof(Source)}: " + Source);
			if (!PathValidator.ValidatePath(Source))
			{
				errors += $"Incorrect {nameof(Source)} parameter!" + Environment.NewLine;
				MyIni.Write(nameof(Source), currPath);
			}
			if (Source == currPath)
				errors += $"The {nameof(Source)} parameter mathes with default value! Please check it." + Environment.NewLine;
			var Destination = string.Empty;
			Destination = MyIni.Read(nameof(Destination));
			Console.WriteLine($"{nameof(Destination)}: " + Destination);
			if (!PathValidator.ValidatePath(Destination))
			{
				errors += $"Incorrect {nameof(Destination)} parameter!" + Environment.NewLine;
				MyIni.Write(nameof(Destination), currPath);
			}
			if (Destination == currPath)
				errors += $"The {nameof(Destination)} parameter mathes with default value! Please check it." + Environment.NewLine;
			if (errors != string.Empty)
			{
				Console.WriteLine(errors);
				Console.WriteLine($"{MyIni.EXE}.ini was updated.");
				Console.ReadKey();
				Environment.Exit(0);
			}

			DirectoryInfo dirSource = new DirectoryInfo(Source);
			DirectoryInfo dirDest = new DirectoryInfo(Destination);
			string dirDestPath = dirDest.FullName;
			string slashes = @"\/";
			while (slashes.Contains (dirDestPath.Last()))
			{
				dirDestPath = dirDestPath.Remove(dirDestPath.Length - 1);
			}
			Console.WriteLine($"Files in {Source} :");
			var OK = 0;
			var notOK = 0;
			foreach (FileInfo file in dirSource.GetFiles())
			{
				var firstChar = IsLetter(file.Name[0])? file.Name[0].ToString().ToUpper(): "_OTHER";
				var path = $"{dirDestPath}\\{firstChar}";
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				var pathName = $"{path}\\{file.Name}";
				try
				{
					file.MoveTo(pathName);
					Console.WriteLine($"{file.Name} => {dirDestPath}\\{firstChar}");
					OK++;
				}
				catch (Exception)
				{
					Console.WriteLine($"{file.Name} =X> {dirDestPath}\\{firstChar}");
					notOK++;
				}
			}
			Console.WriteLine($"Total: OK {OK}, not OK {notOK}");
			Console.ReadKey();
		}
	}
}
