using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Models;

public class PaymentRequest
{
    [Required]
    [RegularExpression(@"^[A-Za-z0-9]{1,34}$", ErrorMessage = "Debtor Account must be up to 34 alphanumeric characters.")]
    public string DebtorAccount { get; set; }

    [Required]
    [RegularExpression(@"^[A-Za-z0-9]{1,34}$", ErrorMessage = "Creditor Account must be up to 34 alphanumeric characters.")]
    public string CreditorAccount { get; set; }

    [Required]
    [RegularExpression(@"^(\d{1,14})(\.\d{1,3})?$", ErrorMessage = "Instructed Amount must be a valid number with up to 14 digits and optionally up to 3 decimal places.")]
    public string InstructedAmount { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be an ISO 4217 Alpha 3 currency code.")]
    public string Currency { get; set; }

}