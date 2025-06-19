using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using MiniAccountManagementSystem.App.Models;
using MiniAccountManagementSystem.App.Utils;

namespace MiniAccountManagementSystem.App.Pages.Vouchers
{
    //[Authorize(Roles = $"{ApplicationRoles.ADMIN}, {ApplicationRoles.ACCOUNTANT}, {ApplicationRoles.VIEWER}")]
    public class DetailsModel : PageModel
    {
        private readonly IConfiguration _config;
        public DetailsModel(IConfiguration config) => _config = config;

        [BindProperty]
        public VoucherDetailsModel Voucher { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            // Load Voucher Header
            var cmd1 = new SqlCommand("SELECT VoucherId, VoucherType, VoucherDate, ReferenceNo FROM Vouchers WHERE VoucherId = @id", conn);
            cmd1.Parameters.AddWithValue("@id", id);
            using var reader1 = await cmd1.ExecuteReaderAsync();
            if (!reader1.HasRows) return NotFound();

            while (await reader1.ReadAsync())
            {
                Voucher.VoucherId = id;
                Voucher.VoucherType = reader1["VoucherType"].ToString();
                Voucher.VoucherDate = (DateTime)reader1["VoucherDate"];
                Voucher.ReferenceNo = reader1["ReferenceNo"].ToString();
            }
            reader1.Close();

            // Load Entries
            var cmd2 = new SqlCommand(@"
                SELECT ca.AccountName, ve.Amount, ve.EntryType
                FROM VoucherEntries ve
                JOIN ChartOfAccounts ca ON ca.AccountId = ve.AccountId
                WHERE ve.VoucherId = @id", conn);

            cmd2.Parameters.AddWithValue("@id", id);
            using var reader2 = await cmd2.ExecuteReaderAsync();
            while (await reader2.ReadAsync())
            {
                Voucher.Entries.Add(new VoucherEntryDetailModel
                {
                    AccountName = reader2["AccountName"].ToString(),
                    Amount = (decimal)reader2["Amount"],
                    EntryType = reader2["EntryType"].ToString()
                });
            }

            return Page();
        }
    }
}

