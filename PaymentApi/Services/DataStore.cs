using System.Collections.Concurrent;
using PaymentApi.Models;

namespace PaymentApi.Services;

public static class DataStore
{
    public static ConcurrentDictionary<string, List<Transaction>> TransactionsByIban = new();
}