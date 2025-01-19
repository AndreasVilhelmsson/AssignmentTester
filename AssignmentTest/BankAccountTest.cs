using System;
using Xunit;

public class BankAccountTests
{
	[Fact]
	public void Deposit_IncreasesBalance()
	{
		// Arrange
		var account = new BankAccount();
		decimal initialBalance = account.Balance;
		decimal depositAmount = 100m;

		// Act
		account.Deposit(depositAmount);

		// Assert
		Assert.Equal(initialBalance + depositAmount, account.Balance);
	}

	[Fact]
	public void Withdraw_DecreasesBalance()
	{
		// Arrange
		var account = new BankAccount();
		account.Deposit(200m);
		decimal initialBalance = account.Balance;
		decimal withdrawAmount = 100m;

		// Act
		account.Withdraw(withdrawAmount);

		// Assert
		Assert.Equal(initialBalance - withdrawAmount, account.Balance);
	}

	[Fact]
	public void Withdraw_ThrowsInvalidOperationException_WhenAmountExceedsBalance()
	{
		// Arrange
		var account = new BankAccount();
		account.Deposit(100m);
		decimal withdrawAmount = 200m;

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => account.Withdraw(withdrawAmount));
	}

	[Fact]
	public void Deposit_ThrowsArgumentException_WhenAmountIsNegative()
	{
		// Arrange
		var account = new BankAccount();
		decimal depositAmount = -50m;

		// Act & Assert
		Assert.Throws<ArgumentException>(() => account.Deposit(depositAmount));
	}

	[Fact]
	public void Withdraw_ThrowsArgumentException_WhenAmountIsNegative()
	{
		// Arrange
		var account = new BankAccount();
		decimal withdrawAmount = -50m;

		// Act & Assert
		Assert.Throws<ArgumentException>(() => account.Withdraw(withdrawAmount));
	}

	[Fact]
	public void Deposit_ZeroAmount_DoesNotChangeBalance()
	{
		// Arrange
		var account = new BankAccount();
		decimal initialBalance = account.Balance;
		decimal depositAmount = 0m;

		// Act
		var exception = Record.Exception(() => account.Deposit(depositAmount));

		// Assert
		Assert.Equal(initialBalance, account.Balance);
		Assert.IsType<ArgumentException>(exception);
	}

	[Fact]
	public void Withdraw_ZeroAmount_DoesNotChangeBalance()
	{
		// Arrange
		var account = new BankAccount();
		decimal initialBalance = account.Balance;
		decimal withdrawAmount = 0m;

		// Act
		var exception = Record.Exception(() => account.Withdraw(withdrawAmount));

		// Assert
		Assert.Equal(initialBalance, account.Balance);
		Assert.IsType<ArgumentException>(exception);
	}

	[Fact]
	public void Withdraw_AllBalance_SetsBalanceToZero()
	{
		// Arrange
		var account = new BankAccount();
		account.Deposit(100m);
		decimal initialBalance = account.Balance;

		// Act
		account.Withdraw(initialBalance);

		// Assert
		Assert.Equal(0m, account.Balance);
	}

	[Fact]
	public void MultipleDeposits_IncreaseBalanceCorrectly()
	{
		// Arrange
		var account = new BankAccount();
		decimal initialBalance = account.Balance;
		decimal depositAmount1 = 50m;
		decimal depositAmount2 = 150m;

		// Act
		account.Deposit(depositAmount1);
		account.Deposit(depositAmount2);

		// Assert
		Assert.Equal(initialBalance + depositAmount1 + depositAmount2, account.Balance);
	}
}