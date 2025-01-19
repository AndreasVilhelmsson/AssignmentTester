using System;

public class BankAccount
{
	public decimal Balance { get; private set; }

	public Action<decimal> Deposit => amount =>
		Balance += amount > 0 ? amount : throw new ArgumentException("No tengo dinero");

	public Action<decimal> Withdraw => amount =>
		Balance = amount <= 0 ? throw new ArgumentException("  must be positive.")
			   : amount > Balance ? throw new InvalidOperationException("Du är en fattig student utan pengar.\n")
			   : Balance - amount;
}