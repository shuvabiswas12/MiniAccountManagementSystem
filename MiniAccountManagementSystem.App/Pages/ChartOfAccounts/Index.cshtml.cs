using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.App.Pages.ChartOfAccounts
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config)
        {
            _config = config;
            Accounts = new List<Account>();
        }

        public List<Account> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            var allAccounts = new List<Account>();
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand("sp_GetAllChartOfAccounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allAccounts.Add(new Account
                        {
                            AccountId = reader.GetInt32(0),
                            AccountName = reader.GetString(1),
                            ParentAccountId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            AccountType = reader.GetString(3)
                        });
                    }
                }
            }
            // Build hierarchical structure
            Accounts = BuildHierarchy(allAccounts);
        }

        private List<Account> BuildHierarchy(List<Account> flatList)
        {
            var lookup = flatList.ToDictionary(a => a.AccountId);
            var roots = new List<Account>();

            foreach (var acc in flatList)
            {
                if (acc.ParentAccountId.HasValue && lookup.ContainsKey(acc.ParentAccountId.Value))
                {
                    lookup[acc.ParentAccountId.Value].Children.Add(acc);
                }
                else
                {
                    roots.Add(acc);
                }
            }
            return roots;
        }

        public class Account
        {
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public int? ParentAccountId { get; set; }
            public string AccountType { get; set; }
            public List<Account> Children { get; set; } = new List<Account>();
        }
    }
}
