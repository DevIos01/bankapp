using System.Globalization;

namespace Bankapp;

public class ExchangeRateManager
{
	public static decimal UsdToSek { get; private set; } = 10.00m ;
	public static decimal EurToSek { get; private set; } = 20.00m ;

	// the admin's rates are saved next to users.json so they are still there after a restart
	private static readonly string FilePath =
		Path.Combine(AppContext.BaseDirectory, "rates.txt");

	// runs once before the rates are used, loads the saved rates if there are any
	static ExchangeRateManager()
	{
		if (!File.Exists(FilePath))
			return;

		string[] lines = File.ReadAllLines(FilePath);

		if (lines.Length >= 2 &&
			decimal.TryParse(lines[0], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal usd) && usd > 0 &&
			decimal.TryParse(lines[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal eur) && eur > 0)
		{
			UsdToSek = usd;
			EurToSek = eur;
		}
	}

	// admin sets a new rate, 0 or below would break Convert
	public static bool SetRate(CurrencyType currency, decimal rate)
	{
		if (rate <= 0)
			return false;

		switch (currency)
		{
			case CurrencyType.Usd:
				UsdToSek = rate;
				SaveRates();
				return true;

			case CurrencyType.Eur:
				EurToSek = rate;
				SaveRates();
				return true;
		}

		return false;
	}

	// line 1 = USD, line 2 = EUR, with a dot so it reads the same on every computer
	private static void SaveRates()
	{
		File.WriteAllLines(FilePath, new[]
		{
			UsdToSek.ToString(CultureInfo.InvariantCulture),
			EurToSek.ToString(CultureInfo.InvariantCulture)
		});
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
