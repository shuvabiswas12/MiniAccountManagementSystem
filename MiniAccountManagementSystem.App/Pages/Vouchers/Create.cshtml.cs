using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MiniAccountManagementSystem.App.Models;
using MiniAccountManagementSystem.App.Utils;
using System.Data;

namespace MiniAccountManagementSystem.App.Pages.Vouchers
{
    [Authorize(Roles = $"{ApplicationRoles.ADMIN}, {ApplicationRoles.ACCOUNTANT}")]
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;

        public CreateModel(IConfiguration config) => _config = config;

        [BindProperty]
        public CreateVoucherModel Voucher { get; set; } = new();

        public List<SelectListItem> AccountOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadAccountDropdown();
            // initialize 2 rows by default
            Voucher.Entries.Add(new VoucherEntryModel());
            Voucher.Entries.Add(new VoucherEntryModel());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var totalDebit = Voucher.Entries.Where(e => e.EntryType == "Debit").Sum(e => e.Amount);
            var totalCredit = Voucher.Entries.Where(e => e.EntryType == "Credit").Sum(e => e.Amount);

            if (totalDebit > totalCredit)
            {
                ModelState.AddModelError("", "Total Debit must equal to or less than Total Credit.");
                await LoadAccountDropdown();
                return Page();
            }


            DataTable entryTable = new();
            entryTable.Columns.Add("AccountId", typeof(int));
            entryTable.Columns.Add("Amount", typeof(decimal));
            entryTable.Columns.Add("EntryType", typeof(string));

            foreach (var entry in Voucher.Entries)
            {
                entryTable.Rows.Add(entry.AccountId, entry.Amount, entry.EntryType);
            }

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("sp_SaveVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VoucherType", Voucher.VoucherType);
            cmd.Parameters.AddWithValue("@VoucherDate", Voucher.VoucherDate);
            cmd.Parameters.AddWithValue("@ReferenceNo", Voucher.ReferenceNo);

            var param = cmd.Parameters.AddWithValue("@Entries", entryTable);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "VoucherEntryType";

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return RedirectToPage("/ChartOfAccounts/Index");
        }

        private async Task LoadAccountDropdown()
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("SELECT AccountId, AccountName FROM ChartOfAccounts", conn);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                AccountOptions.Add(new SelectListItem
                {
                    Value = reader["AccountId"].ToString(),
                    Text = reader["AccountName"].ToString()
                });
            }
        }
    }
}

