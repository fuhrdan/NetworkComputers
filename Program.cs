//*****************************************************************************
//** Program to show computers on a windows network.                         **
//*****************************************************************************


using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace NetworkComputers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string domainName = GetDomainName();
                List<string> computerNames = GetNetworkComputers(domainName);
                Console.WriteLine($"Computers on the network (Domain: {domainName}):");
                foreach (string computer in computerNames)
                {
                    Console.WriteLine(computer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static string GetDomainName()
        {
            try
            {
                return Domain.GetCurrentDomain().Name;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve domain name: {ex.Message}");
                throw;
            }
        }

        static List<string> GetNetworkComputers(string domainName)
        {
            List<string> computerNames = new List<string>();
            string ldapPath = $"LDAP://{domainName}";
            DirectoryEntry entry = new DirectoryEntry(ldapPath);
            DirectorySearcher searcher = new DirectorySearcher(entry)
            {
                Filter = "(&(objectClass=computer))"
            };
            searcher.PropertiesToLoad.Add("name");

            SearchResultCollection results = searcher.FindAll();

            foreach (SearchResult result in results)
            {
                if (result.Properties.Contains("name"))
                {
                    computerNames.Add((string)result.Properties["name"][0]);
                }
            }

            return computerNames;
        }
    }
}