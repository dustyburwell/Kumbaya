// Guids.cs
// MUST match guids.h
using System;

namespace Kumbaya
{
    static class GuidList
    {
        public const string guidKumbayaPkgString = "3d651388-a1c8-4a37-afbb-39ed7e561960";
        public const string guidKumbayaCmdSetString = "f3bb3afd-9b57-423a-9982-483c77d92646";
        public const string guidToolWindowPersistanceString = "14004daf-08a2-4a31-9f98-676c07208f79";

        public static readonly Guid guidKumbayaCmdSet = new Guid(guidKumbayaCmdSetString);
    };
}