using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using MiniAccountManagementSystem.App.Models;

namespace MiniAccountManagementSystem.App.Pages.Vouchers
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config) => _config = config;

        public List<VoucherListModel> Vouchers = new();

        public async Task OnGetAsync()
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("SELECT VoucherId, VoucherType, VoucherDate, ReferenceNo FROM Vouchers ORDER BY VoucherDate DESC", conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Vouchers.Add(new VoucherListModel
                {
                    VoucherId = (int)reader["VoucherId"],
                    VoucherType = reader["VoucherType"].ToString(),
                    VoucherDate = (DateTime)reader["VoucherDate"],
                    ReferenceNo = reader["ReferenceNo"].ToString()
                });
            }
        }
    }
}

