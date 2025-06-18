using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.App.Pages.ChartOfAccounts
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;

        public CreateModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty] public string AccountName { get; set; }

        [BindProperty] public int? ParentAccountId { get; set; }

        [BindProperty] public string AccountType { get; set; }

        public List<SelectListItem> ParentAccounts { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@AccountId", DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountName", AccountName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ParentAccountId", ParentAccountId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountType", AccountType ?? (object)DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand("SELECT AccountId, AccountName FROM ChartOfAccounts", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    ParentAccounts.Add(new SelectListItem { Value = "", Text = "None" });
                    while (await reader.ReadAsync())
                    {
                        ParentAccounts.Add(new SelectListItem
                        {
                            Value = reader["AccountId"].ToString(),
                            Text = reader["AccountName"].ToString()
                        });
                    }
                }
            }
            return Page();
        }
    }
}
