using DomainInfo.lib.json;
using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;

namespace DomainInfo.Module.DomainInfo
{
    internal class ObjectInfo
    {
        public static List<object> GetUsers()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=person)(objectClass=user))";
            SearchResultCollection SearcherResults = Searcher.FindAll();

            string Usersjson = ObjToJson.objTojson(SearcherResults);
            string filePath = "users-output.json";
            File.WriteAllText(filePath, Usersjson);

            // 遍历搜索结果
            List<object> UsersList = new List<object>();
            foreach (SearchResult result in SearcherResults)
            {
                try
                {
                    // 获取用户的属性
                    var samAccountName = result.Properties["samAccountName"][0];
                    var displayName = result.Properties["displayName"][0];

                    // 输出用户信息
                    // Console.WriteLine($"Username: {samAccountName},     Display Name: {displayName}");
                    UsersList.Add(samAccountName);
                }
                catch
                {
                    var name = result.Properties["cn"][0];
                    // Console.WriteLine($"未启用用户名 : {name}");
                }

            }
            return UsersList;
        }
        /*
         * 获取域控制器列表
         */
        public static void GetDomainCs()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=computer)(objectClass=computer)(userAccountControl:1.2.840.113556.1.4.803:=8192))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string Dcjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "Dcs-output.json";
            File.WriteAllText(filePath, Dcjson);
        }
        public static void GetComputers()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(objectCategory=computer)";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string Computersjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "Computers-output.json";
            File.WriteAllText(filePath, Computersjson);
        }
        public static void GetGroups()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(objectCategory=group)";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string Groupsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "Groups-output.json";
            File.WriteAllText(filePath, Groupsjson);
        }
        public static void GetSPNs()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectClass=user)(servicePrincipalName=*))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string SPNsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "SPNs-output.json";
            File.WriteAllText(filePath, SPNsjson);
        }
        public static void GetOUs()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(objectCategory=organizationalUnit)";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string OUsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "OUs-output.json";
            File.WriteAllText(filePath, OUsjson);
        }
        public static void GetGPOs()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(objectCategory=organizationalUnit)";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string GPOsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "GPOs-output.json";
            File.WriteAllText(filePath, GPOsjson);
        }
        /*
         * 允许委派给服务
         */
        public static void GetAllowDelegations()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=524288))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string AllowDelegationsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "AllowDelegations-output.json";
            File.WriteAllText(filePath, AllowDelegationsjson);
        }
        /*
         * No pre-authentication
         */
        public static void GetNoPreAuths()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=16777216))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string NoPreAuthsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "NoPreAuths-output.json";
            File.WriteAllText(filePath, NoPreAuthsjson);
        }
        /*
         * constrained delegation
         */
        public static void GetConstrainDelegations()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=user)(msDS-AllowedToDelegateTo=*))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string ConstrainDelegationsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "ConstrainDelegations-output.json";
            File.WriteAllText(filePath, ConstrainDelegationsjson);
        }
        /*
         * 资源属性基于约束委派（RBCD）
         */
        public static void GetRPConstrainDelegations()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&(objectCategory=computer)(msDS-AllowedToActOnBehalfOfOtherIdentity=*))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string RPConstrainDelegationsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "RPConstrainDelegations-output.json";
            File.WriteAllText(filePath, RPConstrainDelegationsjson);
        }
        public static void GetDomainTrusts()
        {
            Domain currentDomain = Domain.GetCurrentDomain();
            TrustRelationshipInformationCollection trustRelationships = currentDomain.GetAllTrustRelationships();
            foreach (TrustRelationshipInformation trustRelationship in trustRelationships)
            {
                Console.WriteLine($"Trusted Domain source: {trustRelationship.SourceName}");
                Console.WriteLine($"Trusted Domain TargetName: {trustRelationship.TargetName}");
                Console.WriteLine($"Trust Direction: {trustRelationship.TrustDirection}");
                Console.WriteLine($"Trust Type: {trustRelationship.TrustType}");
            }
        }
        /*
         * ACL list
         * 
         * 您需要将 ldapPath 变量替换为您要查询的 Active Directory 对象的 LDAP 路径。
         * 该示例将打印出对象的访问控制列表信息，
         * 包括授权的身份（Identity）、权限（Rights）和访问控制类型（Access Type）。
         * 
         * 将 LDAP 路径替换为您要查询的 Active Directory 对象的路径
         * string ldapPath = "LDAP://la0gke.local:389/CN=krbtgt,CN=Users,DC=la0gke,DC=local";
         */
        public static void GetObjectACL(string ldapPath)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry(ldapPath);
            ActiveDirectorySecurity adsSecurity = directoryEntry.ObjectSecurity;
            AuthorizationRuleCollection authorizationRules = adsSecurity.GetAccessRules(true, true, typeof(NTAccount));
            foreach (ActiveDirectoryAccessRule rule in authorizationRules)
            {
                NTAccount identity = rule.IdentityReference as NTAccount;
                ActiveDirectoryRights rights = rule.ActiveDirectoryRights;
                AccessControlType accessType = rule.AccessControlType;

                Console.WriteLine($"Identity: {identity.Value}");
                Console.WriteLine($"Rights: {rights}");
                Console.WriteLine($"Access Type: {accessType}");
                Console.WriteLine("-------------------------------");
            }
        }
        /*
         * 获取无需密码的用户账户
         * Get a user account without password authentication
         */
        public static void GetNoAuthPasswordAccounts()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(userAccountControl:1.2.840.113556.1.4.803:=65536)";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string NoAuthPasswordAccountsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "NoAuthPasswordAccounts-output.json";
            File.WriteAllText(filePath, NoAuthPasswordAccountsjson);
        }
        /*
         * 获取具有域管理员权限的组
         */
        public static void GetDomainAdminGroups()
        {
            DirectorySearcher Searcher = LDAPCoon.Searcher();
            Searcher.Filter = "(&((objectCategory=group))(adminCount=1))";
            SearchResultCollection SearcherResults = Searcher.FindAll();
            string DomainAdminGroupsjson = ObjToJson.objTojson(SearcherResults);

            string filePath = "DomainAdminGroups-output.json";
            File.WriteAllText(filePath, DomainAdminGroupsjson);
        }
        /*
         * 判断当前机器是否在域内
         */
        public static bool IsInDomain()
        {
            try
            {
                var currentDomain = Domain.GetCurrentDomain();
                Console.WriteLine($"The computer is part of the domain: {currentDomain.Name}");
                return true;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                Console.WriteLine("The computer is not part of a domain.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        /*
         * 获取域内指定组的成员
         * 
         * groupName: 待查询组名，默认域管组
         */
        public static void GetGroupUser(string groupName = "Domain Admins")
        {
            var currentDomain = Domain.GetCurrentDomain();
            string domainName = currentDomain.Name;

            // 获取当前域
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName))
            {
                // 查找指定的组
                using (GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName))
                {
                    if (group != null)
                    {
                        // 遍历组成员
                        foreach (Principal member in group.Members)
                        {
                            Console.WriteLine($"Name: {member.Name}, SAM Account Name: {member.SamAccountName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Group '{groupName}' not found in the domain '{domainName}'.");
                    }
                }
            }
        }
    }
}
