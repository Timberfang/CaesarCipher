namespace CaesarCipher;

public static class CaesarCipher
{
	private static readonly char[] Alphabet = [
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
		'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
	];
	
	public static string Encrypt(string input, int shift)
	{
		// Clean up input
		input = input.Replace(" ", "");
		if (shift >= Alphabet.Length) { shift -= Alphabet.Length; }
		char[] output = input.ToUpper().ToCharArray();

		// Shift characters forward to encrypt
		for (int i = 0; i < output.Length; i++)
		{
			int replacementIndex = (Array.IndexOf(Alphabet, output[i])) + shift;
			if (replacementIndex > Alphabet.Length - 1) { replacementIndex -= (Alphabet.Length); }
			output[i] = Alphabet[replacementIndex];
		}
		
		return new string(output);
	}

	public static string Decrypt(string input, int shift)
	{
		// Clean up input
		if (shift >= Alphabet.Length) { shift -= Alphabet.Length; }
		char[] output = input.ToCharArray();

		// Shift characters *back* to decrypt
		for (int i = 0; i < output.Length; i++)
		{
			int replacementIndex = (Array.IndexOf(Alphabet, output[i])) - shift;
			if (replacementIndex < 0) { replacementIndex += Alphabet.Length; }
			output[i] = Alphabet[replacementIndex];
		}
		
		return new string(output);
	}

	public static IEnumerable<(int, string)> DecryptAll(string input)
	{
		// Clean up input
		List<(int, string)> output = [];

		// Return all possibilities
		for (int i = 1; i < Alphabet.Length; i++)
		{
			char[] testArray = Decrypt(input, i).ToCharArray();
			output.Add((i, new string(testArray)));
		}
		
		return output;
	}
}