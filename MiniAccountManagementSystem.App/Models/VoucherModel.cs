namespace MiniAccountManagementSystem.App.Models
{
    public class VoucherEntryModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string EntryType { get; set; } // "Debit" or "Credit"
    }

    public class CreateVoucherModel
    {
        public string VoucherType { get; set; }
        public DateTime VoucherDate { get; set; } = DateTime.Now;
        public string ReferenceNo { get; set; }
        public List<VoucherEntryModel> Entries { get; set; } = new();
    }

    public class VoucherListModel
    {
        public int VoucherId { get; set; }
        public string VoucherType { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
    }

    public class VoucherDetailsModel
    {
        public int VoucherId { get; set; }
        public string VoucherType { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public List<VoucherEntryDetailModel> Entries { get; set; } = new();
    }

    public class VoucherEntryDetailModel
    {
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string EntryType { get; set; } // Debit / Credit
    }

}
