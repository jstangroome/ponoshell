using System;
using System.Collections;
using System.Management.Automation;

namespace ProofOfConcept
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			IEnumerable results;
			// ensure Mono master-b7adbe3 or later is used to avoid bug #19915
			// see https://bugzilla.xamarin.com/show_bug.cgi?id=19915
			using (var shell = PowerShell.Create ())
			{
				shell.AddCommand ("Get-Date");
				results = shell.Invoke ();
			}

			foreach (var result in results) 
			{
				Console.WriteLine ("result: " + result.ToString ());
			}

			Console.WriteLine ("Woot!");

		}
	}
}
