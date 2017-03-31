using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace LeapExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(LeapPackage.PackageGuidString)]
    public sealed class LeapPackage : Package
    {
        public const string PackageGuidString = "42918412-8613-44cf-909f-045bcd2b4e26";
    }
}
