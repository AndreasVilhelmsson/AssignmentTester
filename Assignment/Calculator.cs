using System;
namespace Assignment
{
	public class Calculator
	{
		public static float Add(float a, float b) => a + b;
		public static float Subtract(float a, float b) => a - b;
		public static float Multiply(float a, float b) => a * b;
		public static float Divide(float a, float b)
		{
			if (b == 0)
			{
				throw new DivideByZeroException("Division by zero is not allowed.");
			}
			return a / b;
		}


	}

}
