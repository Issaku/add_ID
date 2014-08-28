// Xamarin Studio on MAcOSX 10.9
using System;
using System.IO;
using System.Text ;  // for Encoding
using System.Collections.Generic;
using System.Collections;

namespace add_ID
{
	class MainClass
	{
		/// <summary>
		/// arg1 = directry, arg2 = InChI file
		/// </summary>
		/// <param name="s">The command-line arguments.</param>
		public static void Main(string[] s)
		{
			StringBuilder sb = new StringBuilder ();
			string file_sdf = String.Empty;
			string file_inchi = String.Empty;

			for (int N = 1; N < Environment.GetCommandLineArgs ().Length; N++) {
				file_sdf = Environment.GetCommandLineArgs () [1];
				file_inchi = Environment.GetCommandLineArgs () [2];
			}

			if (File.Exists(file_inchi) == true && File.Exists(file_sdf) == true) {
			
				StreamReader reader = new StreamReader (file_inchi, Encoding.Default);
				StreamReader readerSdf = new StreamReader (file_sdf, Encoding.Default);
				string SDF = readerSdf.ReadToEnd ();
				List<string> IDs = new List<string> ();

				string[] mols = SDF.Split (new string[] {"$$$$"}, StringSplitOptions.RemoveEmptyEntries);

				foreach (var mol in mols) {

					string[] ID = SDF.Split (new string[] {">  <ID>"}, StringSplitOptions.RemoveEmptyEntries);

					if (ID.Length > 1) {
						string[] line = ID[1].Replace("\r", "").Split(new char[] {'\n'}) ;
						if (line.Length > 0) {
							IDs.Add (line [0]);
						}
					}
				}
				Console.WriteLine ("ID count: " + IDs.Count);

				string inchi = reader.ReadToEnd ();
				string[] inchis = inchi.Replace("\r", "").Split(new char[] {'\n'}) ;

				Console.WriteLine ("InChI count: " + inchis.Length);


				if (inchis.Length == IDs.Count) {
					for (var i = 0; i > inchis.Length; i++) {
						sb.AppendLine (IDs [i] + "\t" + inchis [i]);
					}

					DateTime dt = DateTime.Now;
					string dtString = dt.ToString ("yyyyMMddHHmmss");

					StreamWriter writer = new StreamWriter ("add_ID_" + dtString + ".txt",
						                      false,  // 上書き （ true = 追加 ）
						                      Encoding.UTF8);

					writer.Write (sb.ToString ());
					writer.Close ();
					Console.WriteLine ("finished");
				} else {
					Console.WriteLine ("Please check data-count");
				}

			} else {
				Console.WriteLine ("Please check data");
			}
		}
	}
}
