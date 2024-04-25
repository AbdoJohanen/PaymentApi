using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models;

public class Transaction
{
    public string PaymentID { get; set; } = null!;

    [RegularExpression(@"^[A-Za-z0-9]{1,34}$")]
    public string DebtorAccount { get; set; } = null!;

    [RegularExpression(@"^[A-Za-z0-9]{1,34}$")]
    public string CreditorAccount { get; set; } = null!;

    [RegularExpression(@"^-?[0-9]{1,14}(\.[0-9]{1,3})?$")]
    public decimal TransactionAmount { get; set; }

    [RegularExpression(@"^[A-Z]{3}$")]
    public string Currency { get; set; } = null!;
}