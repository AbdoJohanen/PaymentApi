using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Models;
using PaymentApi.Services;

namespace PaymentApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private static ConcurrentBag<string> _paymentProcessingStates = new();

    [HttpPost]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest paymentRequest)
    {
        if (paymentRequest == null)
        {
            return BadRequest("Invalid payment request.");
        }

        string clientId = Request.Headers["Client-ID"];
        if (string.IsNullOrWhiteSpace(clientId))
        {
            return BadRequest("Client-ID header is required.");
        }

        if (_paymentProcessingStates.Contains(clientId))
        {
            return Conflict("A payment is already being processed for this Client-ID.");
        }
        
        _paymentProcessingStates.Add(clientId);

        try
        {
            await Task.Delay(2000);

            var paymentId = Guid.NewGuid().ToString();
            var transaction = new Transaction
            {
                PaymentID = paymentId,
                DebtorAccount = paymentRequest.DebtorAccount,
                CreditorAccount = paymentRequest.CreditorAccount,
                TransactionAmount = paymentRequest.InstructedAmount,
                Currency = paymentRequest.Currency
            };
            
            DataStore.TransactionsByIban.AddOrUpdate(
                paymentRequest.DebtorAccount,
                new List<Transaction> { transaction },
                (key, existingVal) =>
                {
                    existingVal.Add(transaction);
                    return existingVal;
                });

            return Ok(transaction);
        }
        finally
        {
            _paymentProcessingStates.TryTake(out _);
        }
    }
}