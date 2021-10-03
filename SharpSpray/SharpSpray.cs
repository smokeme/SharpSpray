                DirectoryEntry dEntry = new DirectoryEntry("LDAP://" + System.DirectoryServices.ActiveDirectory.ActiveDirectorySite.GetComputerSite().InterSiteTopologyGenerator.Name);
                DirectorySearcher dSearch = new DirectorySearcher(dEntry);
                dSearch.Filter = "(&(objectCategory=Person)(sAMAccountName=*)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))";
                dSearch.PageSize = 1000;
                dSearch.PropertiesToLoad.Add("sAMAccountName");
                dSearch.SearchScope = SearchScope.Subtree;
                SearchResultCollection results = dSearch.FindAll();
                if (results != null)
                {
                    for (var i = 0; i < results.Count; i++)
                    {
                        UserList.Add((string)results[i].Properties["sAMAccountName"][0]);
                    }
                }
                else
                {
                    Console.WriteLine("[-] Failed to retrieve the usernames from Active Directory; the script will exit.");
                    System.Environment.Exit(1);
                }

                if (UserList != null)
                {
                    int UserCount = UserList.Count;
                    Console.WriteLine("[+] Successfully collected " + UserCount + " usernames from Active Directory.");
                    lockoutThreshold = (int)dEntry.Properties["minPwdLength"].Value;
                    Console.WriteLine("[*] The Lockout Threshold for the current domain is " + lockoutThreshold + ".");
                    minPwdLength = (int)dEntry.Properties["minPwdLength"].Value;
                    Console.WriteLine("[*] The Min Password Length for the current domain is " + minPwdLength + ".");
                }
                else
                {
                    Console.WriteLine("[-] Failed to create a list the usernames from Active Directory; the script will exit.");
                    System.Environment.Exit(1);
                }
