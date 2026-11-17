using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Common
{
	public static class NumberToWordsConverter
	{
		private static readonly string[] Units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
		private static readonly string[] Teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
		private static readonly string[] Tens = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
		private static readonly string[] Thousands = { "", "Thousand", "Lakh", "Crore" };

		public static string ConvertToWords(decimal amount)
		{
			// Split into rupees and paise
			long rupees = (long)amount;
			int paise = (int)((amount - rupees) * 100);

			string rupeesInWords = ConvertRupeesToWords(rupees);
			string paiseInWords = paise > 0 ? $" and {ConvertRupeesToWords(paise)} Paise" : "";

			return $"{rupeesInWords} Rupees{paiseInWords}".Trim();
		}

		private static string ConvertRupeesToWords(long number)
		{
			if (number == 0) return "Zero";

			string words = "";
			int place = 0;

			while (number > 0)
			{
				int chunk = (int)(number % 1000);
				if (chunk > 0)
				{
					string chunkWords = ConvertChunkToWords(chunk);
					if (place > 0)
						words = $"{chunkWords} {Thousands[place]} {words}".Trim();
					else
						words = chunkWords;
				}
				number /= 1000;
				place++;
			}

			return words;
		}

		private static string ConvertChunkToWords(int number)
		{
			string words = "";

			if (number >= 100)
			{
				words += $"{Units[number / 100]} Hundred ";
				number %= 100;
			}

			if (number >= 20)
			{
				words += $"{Tens[number / 10]} ";
				number %= 10;
			}
			else if (number >= 10)
			{
				return words + Teens[number - 10];
			}

			if (number > 0)
				words += Units[number];

			return words.Trim();
		}
	}
}
