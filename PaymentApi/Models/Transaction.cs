using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models;

public class Transaction
{
    public string PaymentID { get; set; }

    [RegularExpression(@"^[A-Za-z0-9]{1,34}$")]
    public string DebtorAccount { get; set; }

    [RegularExpression(@"^[A-Za-z0-9]{1,34}$")]
    public string CreditorAccount { get; set; }

    [RegularExpression(@"^-?[0-9]{1,14}(\.[0-9]{1,3})?$")]
    public decimal TransactionAmount { get; set; }

    [RegularExpression(@"^[A-Z]{3}$")]
    public string Currency { get; set; }
}