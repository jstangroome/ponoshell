using System;
using System.Collections;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProofOfConcept
{
	public class FakeKernel32() 
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

		[StructLayout(LayoutKind.Sequential)]
		struct MEMORY_BASIC_INFORMATION
		{
		    public UIntPtr BaseAddress;
		    public UIntPtr AllocationBase;
		    public uint AllocationProtect;
		    public UIntPtr RegionSize;
		    public uint State;
		    public uint Protect;
		    public uint Type;
		}

		[DllImport("kernel32", EntryPoint = "GetSystemInfo")]
		static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32")]
		static extern UIntPtr VirtualQuery(UIntPtr lpAddress, ref MEMORY_BASIC_INFORMATION lpBuffer, UIntPtr dwLength);

		const uint PAGE_SIZE = 0; // dummy

		public static void Initialize()
		{
			Debug.WriteLine(string.Format("IntPtr.Size: {0}", IntPtr.Size));
		
			var info = new SYSTEM_INFO();
			GetSystemInfo(out info);

		    var lpBuffer = new MEMORY_BASIC_INFORMATION();
		    VirtualQuery(UIntPtr.Zero, ref lpBuffer, new UIntPtr((ulong)Marshal.SizeOf(lpBuffer)));

		}

	}

	class MainClass	
	{
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
			FakeKernel32.Initialize();
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
