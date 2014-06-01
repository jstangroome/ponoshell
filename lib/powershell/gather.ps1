#requires -version 2
[CmdletBinding()]
param ()

$PSScriptRoot = Split-Path -Path $MyInvocation.MyCommand.Path

if ($PSVersionTable.PSVersion.Major -ne 2) {
    throw 'Requires PowerShell version 2 specifically, not newer, nor older.'
}

$Assemblies = @(
'Microsoft.PowerShell.Commands.Diagnostics'
'Microsoft.PowerShell.Commands.Utility'
'Microsoft.PowerShell.Commands.Management'
'Microsoft.PowerShell.ConsoleHost'
'Microsoft.PowerShell.Security'
'Microsoft.WSMan.Management'
'System.Management.Automation'
)

[AppDomain]::CurrentDomain.GetAssemblies() |
    Where-Object {
        $FullName = $_.FullName
        $Assemblies | Where-Object { $FullName -like "$_, *" }
    } |
    Select-Object -ExpandProperty Location |
    Copy-Item -Destination $PSScriptRoot -PassThru