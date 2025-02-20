using System.CommandLine;

namespace CaesarCipher;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		var inputOption = new Option<string>("--input", "Input text") { IsRequired = true };
		var shiftOption = new Option<int>("--shift", "How much to shift the text") { IsRequired = true };
		inputOption.AddAlias("-i");
		shiftOption.AddAlias("-s");
		shiftOption.AddValidator(result =>
		{
			try
			{
				if (result.GetValueForOption(shiftOption) < 0)
				{
					result.ErrorMessage = $"Shift must be between 0 and {int.MaxValue}.";
				}
			}
			catch (InvalidOperationException)
			{
				result.ErrorMessage = $"Shift must be between 0 and {int.MaxValue}.";
			}
		});

		var rootCommand = new RootCommand("Caesar cipher implementation using C#.");
		var encryptCommand = new Command("encrypt", "Encrypt the text")
		{
			inputOption,
			shiftOption
		};
		var decryptCommand = new Command("decrypt", "Decrypt the text")
		{
			inputOption,
			shiftOption
		};
		var listCommand = new Command("list", "List all possible decrypted texts")
		{
			inputOption
		};
		rootCommand.AddCommand(encryptCommand);
		rootCommand.AddCommand(decryptCommand);
		rootCommand.AddCommand(listCommand);

		encryptCommand.SetHandler((input, shift) =>
			{
				 Console.WriteLine(CaesarCipher.Encrypt(input, shift));
			},
			inputOption, shiftOption);
		decryptCommand.SetHandler((input, shift) =>
			{
				Console.WriteLine(CaesarCipher.Decrypt(input, shift));
			},
			inputOption, shiftOption);
		listCommand.SetHandler(input =>
			{
				var output = CaesarCipher.DecryptAll(input);
				foreach (var match in output) { Console.WriteLine($"- {match.Item2} ({match.Item1})"); }
			},
			inputOption);
		return await rootCommand.InvokeAsync(args);
	}
}