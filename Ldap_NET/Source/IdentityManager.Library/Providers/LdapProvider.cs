using System;
using System.DirectoryServices;

namespace IdentityManager.Library.Providers
{
    public class LdapProvider
    {
        public static bool Authenticate(string userNameWithDomain, string password, string ldapPath)
        {
            try
            {
                ldapPath  =string.Format("LDAP://{0}",ldapPath);
                var entry = new DirectoryEntry(ldapPath, userNameWithDomain, password);
                var nativeObject = entry.NativeObject;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool IsInRole(string userName, string role,string ldapPath)
        {
            try
            {
                var search = new DirectorySearcher(new DirectoryEntry(null));
                search.Filter = string.Format("samaccountname={0}", userName);
                var searchResult = search.FindOne();
                var entry= searchResult.GetDirectoryEntry();
                var properties = entry.Properties["memberOf"];
                for (var i = 0; i < properties.Count; ++i)
                {
                    var s = properties[i].ToString().Substring(3);
                    s = s.Substring(0, s.IndexOf(',')).ToLowerInvariant();
                    if (s.Equals(role,StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                throw new Exception();
            }
            catch
            {
                return false;
            }
        }
    }
}
