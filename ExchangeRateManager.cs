
namespace Bankapp;

public class ExchangeRateManager
{
	public static decimal UsdToSek { get; private set; } = 10.00m ;
	public static decimal EurToSek { get; private set; } = 20.00m ;

	// admin sets a new rate, 0 or below would break Convert
	public static bool SetRate(CurrencyType currency, decimal rate)
	{
		if (rate <= 0)
			return false;

		switch (currency)
		{
			case CurrencyType.Usd:
				UsdToSek = rate;
				return true;

			case CurrencyType.Eur:
				EurToSek = rate;
				return true;
		}

		return false;
	}

	// converts via SEK, e.g. 10 USD -> 100 SEK -> 5 EUR
	public static decimal Convert(decimal amount, CurrencyType from, CurrencyType to)
	{
		if (from == to)
			return amount;

		decimal amountInSek = amount;

		switch (from)
		{
			case CurrencyType.Usd:
				amountInSek = amount * UsdToSek;
				break;

			case CurrencyType.Eur:
				amountInSek = amount * EurToSek;
				break;
		}

		decimal convertedAmount = amountInSek;

		switch (to)
		{
			case CurrencyType.Usd:
				convertedAmount = amountInSek / UsdToSek;
				break;

			case CurrencyType.Eur:
				convertedAmount = amountInSek / EurToSek;
				break;
		}

		return Math.Round(convertedAmount, 2);
	}
}
