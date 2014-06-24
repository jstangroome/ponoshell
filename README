PonoShell
=========

PonoShell exists to determine the feasibility of running PowerShell on Linux, via Mono. 
However, unlike the Pash project, PonoShell aims to leverage the existing 
Microsoft PowerShell binaries instead of re-implementing from source.

Windows-specific dependencies of PowerShell that do not have an equivalent
on Mono or Linux will be mocked or avoided.

Not Planning To Support
-----------------------

Some aspects of PowerShell that are expected to be troublesome to run on Linux are:

 * WMI
 * COM (and hence PSSnapins)
 * PowerShell Workflow
 * powershell.exe Host
 * Modules for IIS, AD, and other Windows-specific technologies

PonoShell will not attempt to address these.

Not Immediately Planning To Support
-----------------------------------

The initial feasibility study is based on executing built-in PowerShell cmdlets
within a non-interactive host and confirming their functionality is correct.
Subsequently a number of aspects of PowerShell will not be addresses until later.
These include:

 * An interactive console
 * Debugging
 * Remoting
 * *-Service cmdlets
 * Event Log cmdlets

How It Works Now
----------------

What follows is not the only way to use PonoShell now but it is how it has been
developed so far and is known to work.

 1. Clone the PonoShell respository to a Windows machine.
 1. Execute lib/powershell/gather.cmd to copy the PowerShell v2 assemblies from the GAC into the repository.
 1. Download and unzip NUnit 2.6.3 to tools/NUnint-2.6.3/.
 1. Install the NUnit NuGet package to packages/.
 1. Install VirtualBox, Vagrant and Cygwin.
 1. From a Cygwin terminal, navigate to the repository vagrant/ directory and execute `vagrant up`. This will take quite some time while it performs a custom build of Mono (for a recent BCL bug fix).
 1. Then execute `vagrant ssh`.
 1. Inside the VM change to the ~/ponoshell/ directory.
 1. Execute `./build.sh`

TODO
----

 * Replace the build.sh files with Make
 * Improve the quality of the kernel32.c fake implementation
 * Add the download/install of NUnit to the build process
 * Investigate the impact of the value of the ApplicationBase registry key
 * Test many more cmdlets
