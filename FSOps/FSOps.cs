using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;


namespace FSOps {
    public static class FSOps {
        public static IEnumerable<DriveNode> EnumerateLocalDrives () => DriveInfo.GetDrives ().Select (_ => new DriveNode (_));

        // TODO: [?;?].
        public static T TryGetConcreteFSNode<T> (FSNode fsNode) where T : FSNode => fsNode as T;

        public static bool HasRights (FileInfo fileInfo, FileSystemRights rightsToCheck) {
            try {
                var accessControl = fileInfo.GetAccessControl (AccessControlSections.Access);
                var authorizationRuleCollection = accessControl.GetAccessRules (true, true, typeof (NTAccount));

                return CheckRightsForCurrentUser (authorizationRuleCollection, rightsToCheck);
            } catch (Exception/* ex*/) {
                // TODO: [2;1] Readable system errors (description instead of error code).

                return false;
            }
        }

        public static bool HasRights (DirectoryInfo directoryInfo, FileSystemRights rightsToCheck) {
            try {
                var accessControl = directoryInfo.GetAccessControl (AccessControlSections.Access);
                var authorizationRuleCollection = accessControl.GetAccessRules (true, true, typeof (NTAccount));

                return CheckRightsForCurrentUser (authorizationRuleCollection, rightsToCheck);
            } catch (Exception/* ex*/) {
                return false;
            }
        }

        private static bool CheckRightsForCurrentUser (AuthorizationRuleCollection authorizationRuleCollection, FileSystemRights rightsToCheck) {
            FileSystemRights permissiveFileSystemRights = 0;
            FileSystemRights prohibitiveFileSystemRights = 0;

            try {
                var currentIdentity = WindowsIdentity.GetCurrent ();
                var currentPrincipal = new WindowsPrincipal (currentIdentity);

                foreach (
                    var asFileSystemAccessRule in
                    from asFileSystemAccessRule in authorizationRuleCollection.OfType<FileSystemAccessRule> ()
                    let asNTAccount = asFileSystemAccessRule.IdentityReference as NTAccount
                    where asNTAccount != null
                    where currentPrincipal.IsInRole (asNTAccount.Value)
                    select asFileSystemAccessRule
                )
                {
                    if (asFileSystemAccessRule.AccessControlType == AccessControlType.Allow) {
                        permissiveFileSystemRights |= asFileSystemAccessRule.FileSystemRights;
                    } else {
                        prohibitiveFileSystemRights |= asFileSystemAccessRule.FileSystemRights;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine (ex.Message);
                permissiveFileSystemRights = 0;
                prohibitiveFileSystemRights = 0;
            }

            var effectiveRights = permissiveFileSystemRights & ~prohibitiveFileSystemRights;

            return (effectiveRights & rightsToCheck) == rightsToCheck;
        }

        [DllImport ("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize (long fileSize, [MarshalAs (UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static string StrFormatByteSize (long filesize)
        {
            var sb = new StringBuilder (11);
            StrFormatByteSize (filesize, sb, sb.Capacity);

            return sb.ToString ();
        }
    }
}
