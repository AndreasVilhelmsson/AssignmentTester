using Assignment;
using Xunit;

namespace Assignment.Tests
{
	public class StringProcessorTests
	{
		private readonly StringProcessor _stringProcessor;

		public StringProcessorTests()
		{
			_stringProcessor = new StringProcessor();
		}

		[Theory]
		[InlineData("Hello", "hello")]
		[InlineData("WORLD", "world")]
		[InlineData("TeSt", "test")]
		public void ToLowerCase_ConvertsToLowerCase(string input, string expected)
		{
			string result = _stringProcessor.ToLowerCase(input);

			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("He!llo@", "Hello")]
		[InlineData("W#o$r^l&d*", "World")]
		[InlineData("123-456", "123456")]
		[InlineData("!@#$%^&*", "")]
		public void Sanitize_RemovesSpecialCharacters(string input, string expected)
		{
			string result = _stringProcessor.Sanitize(input);

			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("He!llo@", "hello", true)]
		[InlineData("WORLD", "world", true)]
		[InlineData("Te&st", "test", true)]
		[InlineData("Different", "Strings", false)]
		[InlineData(null, "test", false)]
		[InlineData("test", null, false)]
		public void AreEqual_ReturnsTrueForMatchingSanitizedStrings(string input1, string input2, bool expected)
		{
			bool result = _stringProcessor.AreEqual(input1, input2);

			Assert.Equal(expected, result);
		}

		[Fact]
		public void Sanitize_ReturnsNull_ForNullInput()
		{
			string result = _stringProcessor.Sanitize(null);

			Assert.Null(result);
		}

		[Fact]
		public void AreEqual_ReturnsFalse_ForNullInputs()
		{
			bool result = _stringProcessor.AreEqual(null, null);

			Assert.False(result);
		}

		[Theory]
		[InlineData("", "", true)]
		[InlineData("", "nonempty", false)]
		[InlineData("nonempty", "", false)]
		public void AreEqual_WithEmptyStrings(string input1, string input2, bool expected)
		{
			bool result = _stringProcessor.AreEqual(input1, input2);

			Assert.Equal(expected, result);
		}
	}
}