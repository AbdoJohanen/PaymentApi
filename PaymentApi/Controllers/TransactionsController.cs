using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models;
using PaymentApi.Services;

namespace PaymentApi.Controllers;

[ApiController]
[Route("accounts/{iban}/transactions")]
public class TransactionsController : ControllerBase
{
    private static List<Transaction> _transactions = new List<Transaction>();

    [HttpGet]
    public IActionResult GetTransactions(string iban)
    {
        if (string.IsNullOrEmpty(iban))
        {
            return BadRequest("IBAN is required.");
        }

        if (!DataStore.TransactionsByIban.TryGetValue(iban, out var transactions))
        {
            return NoContent();
        }

        var completedTransactions = transactions.ToList();

        if (completedTransactions.Count == 0)
        {
            return NoContent();
        }

        return Ok(completedTransactions);
    }
}