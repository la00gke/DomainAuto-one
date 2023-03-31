using DomainInfo.Module.DomainInfo;
using System;
using System.Collections.Generic;

namespace DomainInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<object> UserLists = ObjectInfo.GetUsers();
            Console.WriteLine(UserLists[0]);
            ObjectInfo.GetDomainCs();
            ObjectInfo.GetComputers();
            ObjectInfo.GetGroups();
            ObjectInfo.GetSPNs();
            ObjectInfo.GetOUs();
            ObjectInfo.GetGPOs();
            ObjectInfo.GetAllowDelegations();
            ObjectInfo.GetNoPreAuths();
            ObjectInfo.GetConstrainDelegations();
            ObjectInfo.GetRPConstrainDelegations();
            ObjectInfo.GetDomainTrusts();
            // ObjectInfo.GetObjectACL(ldapPath);
            ObjectInfo.GetNoAuthPasswordAccounts();
            ObjectInfo.GetDomainAdminGroups();
            ObjectInfo.IsInDomain();
            ObjectInfo.GetGroupUser();


        }


    }
}
