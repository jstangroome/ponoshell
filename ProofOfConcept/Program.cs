using System;
using System.Collections;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProofOfConcept
{
	[StructLayout(LayoutKind.Sequential)]
	struct SYSTEM_INFO
	{
		public int dwOemId;
		public int dwPageSize;
		public IntPtr lpMinAppAddr;
		public IntPtr lpMaxAppAddr;
		public IntPtr dwActiveProcessorMask;
		public int dwNumberOfProcessors;
		public int dwProcessorType;
		public int dwAllocationGranularity;
		public short wProcessorLevel;
		public short wProcessorRevision;
	}

	class MainClass
	{
		[DllImport("kernel32.dll", EntryPoint = "GetSystemInfo")]
		static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

		private static void CreateRegistryKeys()
		{
			using (var softwareKey = Registry.LocalMachine.OpenSubKey (@"SOFTWARE", writable: true))
			using (var psKey = softwareKey.CreateSubKey (@"Microsoft\PowerShell\1\PowerShellEngine")) {
				psKey.SetValue ("PowerShellVersion", "2.0");
				psKey.SetValue ("ApplicationBase", "/home/jason/ponoshell/lib/powershell");
			}
		}

		private static void	TestFakeKernel32()
		{
			Console.WriteLine(string.Format("IntPtr.Size: {0}", IntPtr.Size));
			var info = new SYSTEM_INFO();
			GetSystemInfo(out info);
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Console.WriteLine(string.Format("Environment.OSVersion: {0}", Environment.OSVersion));


			TestFakeKernel32 ();
			//CreateRegistryKeys (); // TODO create keys only if needed

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
