using System;
using System.DirectoryServices;

namespace DomainInfo
{
    internal class LDAPCoon
    {
        public static int Port;
        public static string DomainName;

        public static DirectorySearcher Searcher()
        {
            DomainName = Environment.GetEnvironmentVariable("USERDNSDOMAIN");
            Port = 389;
            DirectoryEntry entry = new DirectoryEntry($"LDAP://{DomainName}:{Port}", null, null, AuthenticationTypes.Secure);
            DirectorySearcher Searcher = new DirectorySearcher(entry);
            return Searcher;
        }

    }
}
