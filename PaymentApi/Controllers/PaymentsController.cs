using System.Collections.Concurrent;
using System.Globalization;
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
            var paymentId = Guid.NewGuid().ToString();
            if (decimal.TryParse(paymentRequest.InstructedAmount, NumberStyles.Any, CultureInfo.InvariantCulture,
                    out var transactionAmount))
            {
                var transaction = new Transaction
                {
                    PaymentID = paymentId,
                    DebtorAccount = paymentRequest.DebtorAccount,
                    CreditorAccount = paymentRequest.CreditorAccount,
                    TransactionAmount = transactionAmount,
                    Currency = paymentRequest.Currency
                };

                Task.Run(async () =>
                {
                    await Task.Delay(2000);

                    DataStore.TransactionsByIban.AddOrUpdate(
                        paymentRequest.DebtorAccount,
                        new List<Transaction> { transaction },
                        (key, existingVal) =>
                        {
                            existingVal.Add(transaction);
                            return existingVal;
                        });

                    _paymentProcessingStates.TryTake(out _);
                });

                return Ok(transaction);
            }
            else
            {
                return BadRequest(
                    "Instructed Amount must be a valid decimal number with up to 14 digits and optionally up to 3 decimal places.");

            }
        }
        catch
        {
            return BadRequest("Something went wrong please try again.");
        }
    }
}