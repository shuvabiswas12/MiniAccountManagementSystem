using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.App.Pages.ChartOfAccounts
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _config;
        public EditModel(IConfiguration config) => _config = config;

        [BindProperty] public int AccountId { get; set; }
        [BindProperty] public string AccountName { get; set; }
        [BindProperty] public int? ParentAccountId { get; set; }
        [BindProperty] public string AccountType { get; set; }

        public List<SelectListItem> ParentAccounts = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            // Get account details
            using (var cmd = new SqlCommand("SELECT * FROM ChartOfAccounts WHERE AccountId = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    AccountId = id;
                    AccountName = reader["AccountName"].ToString();
                    ParentAccountId = reader["ParentAccountId"] as int?;
                    AccountType = reader["AccountType"].ToString();
                }
            }

            // Load parent dropdown
            using (var cmd = new SqlCommand("SELECT AccountId, AccountName FROM ChartOfAccounts WHERE AccountId != @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = await cmd.ExecuteReaderAsync();
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "Update");
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            cmd.Parameters.AddWithValue("@AccountName", AccountName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ParentAccountId", ParentAccountId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AccountType", AccountType ?? (object)DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return RedirectToPage("Index");
        }
    }
}

