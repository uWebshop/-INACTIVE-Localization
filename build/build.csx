#r "System.IO.Compression.FileSystem.dll"

/*!
 * mindrevolution
 * http://www.mindrevolution.com
 *
 * Made in Stuttgart. Engineered to perfection.
 */
 
 
 
// .Net Framework 4.5+ **ONLY**


using System.IO;
using System.IO.Compression;

string installerPath = @".\installer";

Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Creating Umbraco packages with localization data for uWebshop ...");
Console.WriteLine();
Console.WriteLine("> Merging installer master from {0}", installerPath);
Console.WriteLine();

// - iterate over the language folders
try
{
	string ziptarget = "";
	string curlangdir = "";
	string packagexmlSource = "";
	
	List<string> dirs = new List<string>(Directory.EnumerateDirectories(@"..\", "??-??"));
	
	// - installer package XML
	packagexmlSource = System.IO.File.ReadAllText(installerPath + @"\package.xml");

	foreach (var dir in dirs)
	{
		curlangdir = dir.Substring(dir.LastIndexOf("\\") + 1);
		Console.WriteLine("> Processing {0}", curlangdir);

		ziptarget = @".\output\uwebshop.dictionary." + curlangdir + ".package.installer.zip";
		if (System.IO.File.Exists(ziptarget))
		{
			Console.WriteLine("  Removing existing file '{0}'", ziptarget);
			System.IO.File.Delete(ziptarget);
		}
		Console.WriteLine("  Creating '{0}'", ziptarget);
		
		// - add the language XML file (temporarily)
		System.IO.File.Copy(@".\..\" + curlangdir + @"\uWebshopDictionary.xml", @".\installer\uWebshopDictionary.xml", true);
		
		// - inject language code into package XML
		System.IO.File.WriteAllText(installerPath + @"\package.xml", packagexmlSource.Replace("{{LanguageCode}}", curlangdir));
				
		// - create package ZIP
		ZipFile.CreateFromDirectory(installerPath, ziptarget);
		
		Console.WriteLine();
	}
	
	// - restore original file and folder contents
	System.IO.File.WriteAllText(installerPath + @"\package.xml", packagexmlSource);
	if (System.IO.File.Exists(installerPath + @"\uWebshopDictionary.xml"))
	{
		System.IO.File.Delete(installerPath + @"\uWebshopDictionary.xml");
	}

	Console.WriteLine("{0} language packages created.",  dirs.Count);
}
catch (UnauthorizedAccessException UAEx)
{
	Console.WriteLine(UAEx.Message);
}
catch (PathTooLongException PathEx)
{
	Console.WriteLine(PathEx.Message);
}

