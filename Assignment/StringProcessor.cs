using System;
using System.Text.RegularExpressions;

namespace Assignment
{
	public class StringProcessor
	{
		public string ToLowerCase(string input)
		{
			if (input == null) return null;
			return input.ToLower();
		}

		public string Sanitize(string input)
		{
			if (input == null) return null;
			return Regex.Replace(input, "[^a-zA-Z0-9]", "");
		}

		public bool AreEqual(string input1, string input2)
		{
			if (input1 == null || input2 == null) return false;

			string sanitizedInput1 = Sanitize(input1)?.ToLower();
			string sanitizedInput2 = Sanitize(input2)?.ToLower();

			return sanitizedInput1 == sanitizedInput2;
		}
	}
}