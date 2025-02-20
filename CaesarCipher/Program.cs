using System.CommandLine;

namespace CaesarCipher;

internal static class Program
{
	private static async Task<int> Main(string[] args)
	{
		var inputOption = new Option<string>("--input", "Input text") { IsRequired = true };
		var shiftOption = new Option<int>("--shift", "How much to shift the text") { IsRequired = true };
		var aboutOption = new Option<bool>("--about", "Display copyright information about the program");
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
		// aboutOption.SetDefaultValue(true);

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
		rootCommand.AddOption(aboutOption);

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
		rootCommand.SetHandler(displayAbout =>
			{
				if (!displayAbout) return;
				// UTF-8 is needed to display the copyright symbol
				Console.OutputEncoding = System.Text.Encoding.UTF8;
				Console.WriteLine(string.Join(
					Environment.NewLine,
					"This software is made available to you under the following licenses:",
					"",
					"CaesarCipher",
					"",
					"Copyright \u00a9 2025 Timberfang",
					"",
					"Permission is hereby granted, free of charge, to any person obtaining a copy",
					"of this software and associated documentation files (the \"Software\"), to deal",
					"in the Software without restriction, including without limitation the rights",
					"to use, copy, modify, merge, publish, distribute, sublicense, and/or sell",
					"copies of the Software, and to permit persons to whom the Software is",
					"furnished to do so, subject to the following conditions:",
					"",
					"The above copyright notice and this permission notice shall be included in all",
					"copies or substantial portions of the Software.",
					"",
					"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR",
					"IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,",
					"FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE",
					"AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER",
					"LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,",
					"OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE",
					"SOFTWARE.",
					"",
					"--------------------------------------------------------------------------------",
					"",
					"System.CommandLine",
					"",
					"The MIT License (MIT)",
					"",
					"Copyright \u00a9 .NET Foundation and Contributors",
					"",
					"All rights reserved.",
					"",
					"Permission is hereby granted, free of charge, to any person obtaining a copy",
					"of this software and associated documentation files (the \"Software\"), to deal",
					"in the Software without restriction, including without limitation the rights",
					"to use, copy, modify, merge, publish, distribute, sublicense, and/or sell",
					"copies of the Software, and to permit persons to whom the Software is",
					"furnished to do so, subject to the following conditions:",
					"",
					"The above copyright notice and this permission notice shall be included in all",
					"copies or substantial portions of the Software.",
					"",
					"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR",
					"IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,",
					"FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE",
					"AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER",
					"LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,",
					"OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE",
					"SOFTWARE."));
				Console.OutputEncoding = System.Text.Encoding.Default;
			},
			aboutOption);
		return await rootCommand.InvokeAsync(args);
	}
}