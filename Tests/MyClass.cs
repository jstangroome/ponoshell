using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.Win32;
using NUnit.Framework;

namespace Tests
{
    public class MyClass
    {
        [Test]
        public void Should_parse_guid_format_x_containing_spaces()
        {
            //new Guid("{ 0x95baba28, 0xed26, 0x49c9, { 0xb7, 0x4f, 0x93, 0xb1, 0x70, 0xe1, 0xb8, 0x49 }}");

            new Guid("{ 0x00000001, 0x0002, 0x0003, { 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b }}");
            // if this fails a newer version of Mono is required, at least master-b7adbe3
        }

        [Test]
        public void Should_find_PowerShell_version_in_registry()
        {
            using (var psKey = Registry.LocalMachine.OpenSubKey (@"SOFTWARE\Microsoft\PowerShell\1\PowerShellEngine")) {
                Assert.AreEqual("2.0", psKey.GetValue ("PowerShellVersion"));
            }
        }
        
        [Test]
        public void Should_find_PowerShell_ApplicationBase_in_registry()
        {
            using (var psKey = Registry.LocalMachine.OpenSubKey (@"SOFTWARE\Microsoft\PowerShell\1\PowerShellEngine")) {
                Assert.IsFalse(string.IsNullOrEmpty((string)psKey.GetValue ("ApplicationBase")));
            }
        }

        [Test]
        public void Should_parse_a_simple_command_without_exceptions() 
        {
            ProofOfConcept.FakeKernel32.Initialize();
            Assert.IsNotNull(ScriptBlock.Create("Get-Date"));
        }

        [Test]
        public void Should_return_current_time_from_GetDate()
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Date");
                var result = (DateTime)shell.Invoke().Single().BaseObject;
                var delta = DateTime.Now.Subtract(result);
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.IsTrue(delta.TotalSeconds < 60);
            }
        }

        [Test]
        public void Should_return_ls_alias_by_name()
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Alias").AddParameter("Name", "ls");
                var result = (AliasInfo)shell.Invoke().Single().BaseObject;
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreEqual("Get-ChildItem", result.Definition);
            }
        }

        [Test]
        public void Should_return_approved_verbs() 
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Verb");
                var result = shell.Invoke();
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreNotEqual(0, result.Count);
            }
        }

        [Test]
        public void Should_return_PSVersionTable_via_GetVariable()
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Variable").AddParameter("Name", "PSVersionTable");
                var result = (PSVariable)shell.Invoke().Single().BaseObject;
                var hash = (IDictionary)result.Value;
                var version = (Version)hash["PSVersion"];
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreEqual(2, version.Major);
            }
        }

        [Test]
        public void Should_return_a_FileSystem_PSDrive() 
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-PSDrive").AddParameter("PSProvider", "FileSystem");
                var result = shell.Invoke();
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreNotEqual(0, result.Count);
            }
        }

        [Test]
        public void Should_return_content_from_Environment_drive() 
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Content").AddParameter("Path", "Env:PATH");
                var result = (string)shell.Invoke().Single().BaseObject;
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreEqual(Environment.GetEnvironmentVariable("PATH"), result);
            }
        }

        [Test]
        public void Should_return_this_process_by_id() 
        {
            var pid = Process.GetCurrentProcess().Id;
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Process").AddParameter("Id", pid);
                var result = (Process)shell.Invoke().Single().BaseObject;
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreEqual("localhost", result.MachineName);
            }
        }

        [Test]
        public void Should_return_current_location() 
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-Location");
                var result = (PathInfo)shell.Invoke().Single().BaseObject;
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreEqual("/", result.Drive.Name);
                // Assert.AreEqual(@"/:\", result.Path);
                // Assert.AreEqual(@"\", result.ProviderPath);
                // TODO confirm if this is broken with respect to Environment.CurrentDirectory
            }
        }

        [Test]
        public void Should_return_trace_sources() 
        {
            using (var shell = PowerShell.Create ())
            {
                shell.AddCommand ("Get-TraceSource");
                var result = shell.Invoke();
                foreach (var item in result) {
                    var ts = (PSTraceSource)item.BaseObject;
                    Console.WriteLine(string.Format("tracesource: {0}", ts.Name));
                }
                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreNotEqual(0, result.Count);
            }
        }

        [Test]      
        public void Should_trace_command() 
        {
            using (var shell = PowerShell.Create ())
            {
                var twtl = new TextWriterTraceListener("twtlout");
                twtl.WriteLine("TWTL" + DateTime.Now.ToString());
                twtl.Flush();

                var scriptBlock = ScriptBlock.Create(@"Get-Date");

                shell.AddScript(@"$t = New-Object System.Diagnostics.TextWriterTraceListener -Arg 'pst4'; $t.WriteLine('woo'); Get-TraceSource | % { $_.Listeners.Add($t); $_.Options='All'; $_.Switch.Level='All' }; Get-Date; $t.Dispose();");
                shell.Invoke();
                shell.Commands.Clear();

                shell.AddCommand ("Trace-Command")
                    .AddParameter("Name", "*")
                    .AddParameter("FilePath", @"pstrace2")
                    .AddParameter("Option", @"All")
                    .AddParameter("Force", true)
                    .AddParameter("PSHost", true)
                    .AddParameter("Debugger", true)
                    .AddParameter("Expression", scriptBlock);

                shell.AddScript(@"Get-TraceSource | % { $_.Listeners } | % { $_.Flush() }");

                var result = shell.Invoke();

                foreach (var item in result) {
                    Console.WriteLine(string.Format("Item: {0}", item));
                }
                foreach (var item in shell.Streams.Debug) {
                    Console.WriteLine(string.Format("DBG: {0}", item));
                }
                foreach (var item in shell.Streams.Error) {
                    Console.WriteLine(string.Format("ERR: {0}", item));
                }


                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreNotEqual(0, result.Count);
                //Assert.AreEqual("z", Environment.GetEnvironmentVariable("MONO_TRACE_LISTENER"));
                Console.WriteLine("WOO!");
            }
        }

        [Test]
        public void Should_return_FileSystem_root_items() 
        {
            //Console.WriteLine(DriveInfo.GetDrives()[0].RootDirectory.GetDirectories()[0].GetFileSystemInfos()[0].FullName);
            using (var shell = PowerShell.Create ())
            {
                //shell.AddCommand ("Get-ChildItem").AddParameter("Path", @"/:\");
                var scriptBlock = ScriptBlock.Create(@"Get-ChildItem -Path /:/bin");
                shell.AddCommand ("Trace-Command")
                    .AddParameter("Name", "*") //new string[] {"Cmdlet", "LocationGlobber", "PathResolution", "ParameterBinding", "SessionState", "CmdletProviderIntrinsics", "CoreCommandProvider", "FileSystemProvider"})
                    .AddParameter("FilePath", @"pstrace")
                    .AddParameter("Option", @"All")
                    .AddParameter("Force", true)
                    .AddParameter("Expression", scriptBlock);
                var result = shell.Invoke();
                foreach (var item in result) {
                    var fsi = (FileSystemInfo)item.BaseObject;
                    Console.WriteLine(string.Format("FullName: {0}", fsi.FullName));
                }
                foreach (var item in shell.Streams.Error) {
                    Console.WriteLine(string.Format("ERR: {0}", item));
                }

                Assert.AreEqual(0, shell.Streams.Error.Count, "ErrorStream");
                Assert.AreNotEqual(0, result.Count);
            }
        }

        // TODO Get-Command, Get-Module
        // TODO Get-Item / Get-ChildItem / which providers? Env:, /:
        // TODO Set-Location

        // tracing: LocationGlobber, PathResolution

    }
}

