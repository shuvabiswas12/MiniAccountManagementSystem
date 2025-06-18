using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.App.Pages.ChartOfAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _config;
        public DeleteModel(IConfiguration config) => _config = config;

        [BindProperty] public int AccountId { get; set; }
        [BindProperty] public string AccountName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("SELECT AccountName FROM ChartOfAccounts WHERE AccountId = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            if (result == null) return NotFound();

            AccountId = id;
            AccountName = result.ToString();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "Delete");
            cmd.Parameters.AddWithValue("@AccountId", AccountId);
            cmd.Parameters.AddWithValue("@AccountName", DBNull.Value);
            cmd.Parameters.AddWithValue("@ParentAccountId", DBNull.Value);
            cmd.Parameters.AddWithValue("@AccountType", DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return RedirectToPage("Index");
        }
    }
}

