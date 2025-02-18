using Spectre.Console;

namespace CaesarCipher;

internal static class Program
{
	private static void Main()
	{
		AnsiConsole.WriteLine("WARNING");
		AnsiConsole.WriteLine("This program implements the Caesar cipher for educational purposes only.");
		AnsiConsole.WriteLine("The Caesar cipher is not secure, and should not be used for sensitive data.");
		AnsiConsole.WriteLine("Assume any string 'encrypted' by the Caesar cipher is no more secure than plain text.");
		AnsiConsole.WriteLine();

		string choice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("What would you like to do?")
				.AddChoices("Encrypt", "Decrypt", "Brute-force Decrypt"));

		string input;
		int shift;
		switch (choice)
		{
			case "Encrypt":
				input = AnsiConsole.Prompt(
					new TextPrompt<string>("Enter input string - letters and spaces only, please:")
						.Validate(c => c.All(d => char.IsLetter(d) || char.IsWhiteSpace(d))));
				shift = AnsiConsole.Ask<int>("How much do you want to shift the characters by?");
				string encrypted = CaesarCipher.Encrypt(input, shift);
				AnsiConsole.WriteLine($"Encrypted string: {encrypted}");
				break;
			case "Decrypt":
				input = AnsiConsole.Prompt(
					new TextPrompt<string>("Enter input string - letters and spaces only, please:")
						.Validate(c => c.All(d => char.IsLetter(d) || char.IsWhiteSpace(d))));
				shift = AnsiConsole.Ask<int>("What shift was used to encrypt the characters?");
				string decrypted = CaesarCipher.Decrypt(input, shift);
				Console.WriteLine($"Decrypted string: {decrypted}");
				break;
			case "Brute-force Decrypt":
				input = AnsiConsole.Prompt(
					new TextPrompt<string>("Enter input string - letters and spaces only, please:")
						.Validate(c => c.All(d => char.IsLetter(d) || char.IsWhiteSpace(d))));
				string search = AnsiConsole.Ask<string>("Enter a search string:");
				var bruteForceDecrypted = CaesarCipher.BruteForceDecrypt(input, search);
				AnsiConsole.WriteLine($"Outputs containing pattern '{search}':");
				foreach (var match in bruteForceDecrypted) { AnsiConsole.WriteLine($"- {match.Item2} (Shift: {match.Item1})"); }
				break;
		}
	}
}