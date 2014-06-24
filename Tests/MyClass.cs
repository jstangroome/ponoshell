using System;
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
                //TODO psKey.GetValue ("ApplicationBase");
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
        public void Should_parse() 
        {
            ProofOfConcept.FakeKernel32.Initialize();
            Assert.IsNotNull(ScriptBlock.Create("Get-Date"));
        }
    }
}

