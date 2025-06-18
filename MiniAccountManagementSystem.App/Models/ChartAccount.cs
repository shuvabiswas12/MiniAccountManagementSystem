namespace MiniAccountManagementSystem.App.Models
{
    public class ChartAccount
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int? ParentAccountId { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
    }
}
