using System.Text;
using System.Text.RegularExpressions;

namespace App.Extensions;

public static partial class StringExtensions
{
	public static string StripPunctuation(this string s)
	{
		StringBuilder sb = new();
		foreach(char c in s)
		{
			if(!char.IsPunctuation(c))
			{
				sb.Append(c);
			}
		}
		return sb.ToString();
	}

	/// <summary>
	/// Replaces multiple spaces with a single space
	/// </summary>
	public static string ReplaceMultipleSpacesWithSingleSpace(this string text)
	{
		return MultipleSpaceRegex().Replace(text, " ");
	}

	[GeneratedRegex(@"[^\S\r\n]+")]
	private static partial Regex MultipleSpaceRegex();

	public static string ToUnicodeString(this string str)
	{
		int value = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
		return char.ConvertFromUtf32(value);
	}

	public static string ReplaceiOSApostrophe(string? text)
	{
		if(string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}

		text = text.Replace("‘", "'");
		text = text.Replace("’", "'");
		return text;
	}

	public static string FreeTextSanitiser(string? text)
	{
		if(string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}

		text = ReplaceiOSApostrophe(text);

		return text
			.Replace(",", " ")
			.WafTextReplace()
			.RemoveInvalidCharacter()
			.ReplaceMultipleHyphensWith1()
			.KeepFirst11SpecialCharacters();
	}

	/// <summary>
	/// Replaces known WAF unfriendly words
	/// </summary>
	static string WafTextReplace(this string text)
	{
		return KnowWafUnfriendlyWordsRegex().Replace(text, m => WafWorkaround(m.Value));

		static string WafWorkaround(string test)
		{
			return test.Insert(1, "1");
		}
	}

	[GeneratedRegex("""(?i)\b(?<word>&&|\\"'|\\\\da-f|_pop|_samp|_to_date|\|\||after|all|alter|and|between|c_to_time|c_to_timeond|cmp|create|dat|delete|desc|distinct|div|drop|free_lock|from|having|insert|ipv|ipv4|ipv4_compat|ipv4_mapped|is_free_lock|is_ipv4|is_ipv4_compat|is_ipv4_mapped|isnull|like|load|not|or|r_to_date|rcmp|rename|schema|select|send|session_user|sha1|sha2|sign|space|sqrt|std|stddev|stddev_pop|stddev_samp|substr|substring_index|sysdate|system_user|time|truncate|ub|union|update|xor)\b""")]
	private static partial Regex KnowWafUnfriendlyWordsRegex();

	/// <summary>
	/// Removes invalid characters
	/// </summary>
	static string RemoveInvalidCharacter(this string text)
	{
		return InvalidCharactersRegex()
			.Matches(text)
			.Cast<Match>()
			.Aggregate(string.Empty, (s, e) => s + e.Value, s => s);
	}

	[GeneratedRegex(@"([a-z]|[A-Z]|[0-9]|[ ]|[,]|[!]|[?]|[:]|[\n]|[/]|[-]|[']|[.])*")]
	private static partial Regex InvalidCharactersRegex();

	/// <summary>
	/// Keeps the first 11 special characters in the text
	/// </summary>
	/// <remarks>
	/// WAF blocks requests with more than 11 special characters, rule ID: 942430
	/// </remarks>
	static string KeepFirst11SpecialCharacters(this string text)
	{
		List<Match> matches = SpecialCharacterRegex().Matches(text)
			.Cast<Match>()
			.Select(c => c)
			.ToList();

		if(matches.Count > 11)
		{
			IEnumerable<Match> matchesToRemove = matches.Skip(11);
			char[] textArray = text.ToCharArray();
			foreach(Match? match in matchesToRemove.Reverse())
			{
				textArray[match.Index] = ' ';
			}

			text = new string(textArray);
		}

		return text;
	}

	[GeneratedRegex(@"[~!@#$%^&*()\-\+=\{\}\]\|:;""><'´‘`-]")]
	private static partial Regex SpecialCharacterRegex();

	/// <summary>
	/// Replace multiple concurrent hyphens with a single hyphen
	/// </summary>
	/// <remarks>
	/// WAF blocks requests with multiple concurrent hyphens, #Bug-27005
	/// </remarks>
	static string ReplaceMultipleHyphensWith1(this string text)
	{
		return MultipleHyphenRegex().Replace(text, "-");
	}

	[GeneratedRegex("-+", RegexOptions.None)]
	private static partial Regex MultipleHyphenRegex();
}