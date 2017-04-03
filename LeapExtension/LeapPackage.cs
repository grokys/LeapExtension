using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace LeapExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(LeapPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    public sealed class LeapPackage : Package
    {
        public const string PackageGuidString = "42918412-8613-44cf-909f-045bcd2b4e26";
    }
}
