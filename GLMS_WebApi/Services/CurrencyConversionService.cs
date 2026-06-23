namespace GLMS.API.Services;

public interface ICurrencyStrategy
{
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency);
}

public class LiveExchangeRateStrategy : ICurrencyStrategy
{
    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency)
    {
        return amount; 
    }
}

public class CurrencyConversionService
{
    private readonly ICurrencyStrategy _strategy;

    public CurrencyConversionService(ICurrencyStrategy strategy) =>
        _strategy = strategy;

    public Task<decimal> ConvertToBaseAsync(decimal amount, string currency) =>
        _strategy.ConvertAsync(amount, currency);
}