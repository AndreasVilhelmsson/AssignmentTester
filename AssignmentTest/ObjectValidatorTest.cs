using System;
using System.Collections.Generic;
using Xunit;

public class ObjectValidatorTests
{
	private readonly ObjectValidator _validator;

	public ObjectValidatorTests()
	{
		_validator = new ObjectValidator();
	}

	[Fact]
	public void IsNull_ShouldReturnTrue_WhenObjectIsNull()
	{
		// Arrange
		object obj = null;

		// Act
		bool result = _validator.IsNull(obj);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void IsNull_ShouldReturnFalse_WhenObjectIsNotNull()
	{
		// Arrange
		object obj = new object();

		// Act
		bool result = _validator.IsNull(obj);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void GetNullProperties_ShouldThrowArgumentNullException_WhenObjectIsNull()
	{
		// Arrange
		object obj = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => _validator.GetNullProperties(obj));
	}

	[Fact]
	public void GetNullProperties_ShouldReturnEmptyList_WhenNoPropertiesAreNull()
	{
		// Arrange
		var obj = new ExampleClass
		{
			Name = "Test",
			Age = 30,
			Address = "123 Street"
		};

		// Act
		List<string> result = _validator.GetNullProperties(obj);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void GetNullProperties_ShouldReturnListOfNullProperties_WhenPropertiesAreNull()
	{
		// Arrange
		var obj = new ExampleClass
		{
			Name = null,
			Age = null,
			Address = "123 Street"
		};

		// Act
		List<string> result = _validator.GetNullProperties(obj);

		// Assert
		Assert.Contains("Name", result);
		Assert.Contains("Age", result);
		Assert.DoesNotContain("Address", result);
	}

	[Fact]
	public void GetNullProperties_ShouldHandleNestedNullProperties()
	{
		// Arrange
		var obj = new NestedClass
		{
			NestedProperty = new ExampleClass
			{
				Name = null,
				Age = null,
				Address = "123 Street"
			}
		};

		// Act
		List<string> result = _validator.GetNullProperties(obj);

		// Assert
		Assert.Contains("NestedProperty.Name", result);
		Assert.Contains("NestedProperty.Age", result);
		Assert.DoesNotContain("NestedProperty.Address", result);
	}

	[Fact]
	public void GetNullProperties_ShouldReturnEmptyList_ForUnexpectedType()
	{
		// Arrange
		var obj = new UnexpectedType
		{
			Field1 = "Field",
			Field2 = 42
		};

		// Act
		List<string> result = _validator.GetNullProperties(obj);

		// Assert
		Assert.Empty(result);
	}
}

// En exempelklass för att testa GetNullProperties
public class ExampleClass
{
	public string Name { get; set; }
	public int? Age { get; set; }
	public string Address { get; set; }
}

// En nested klass för att testa nested null-properties
public class NestedClass
{
	public ExampleClass NestedProperty { get; set; }
}

// En oväntad typ för att testa kraschfall
public class UnexpectedType
{
	public string Field1 { get; set; }
	public int Field2 { get; set; }
}