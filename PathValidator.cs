using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubdirSorter
{
	static class PathValidator
	{
		public static bool ValidatePath(string path)
		{
			if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
			{
				try
				{
					// If path is relative take %IGXLROOT% as the base directory
					if (!Path.IsPathRooted(path))
					{
						path = Path.GetFullPath(string.Empty);
					}
					return true;
				}
				catch (Exception){}
			}
			return false;
		}
	}
}
