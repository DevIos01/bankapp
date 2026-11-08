# Bankapp

A small C# console application that simulates basic banking workflows. The application supports customer registration and login, account management, transfers, currency conversion, administrator actions, and loan applications. It is intended as a learning project.

## Development Plan

The application can be understood as a sequence of feature areas:

1. Define the core banking objects: users, accounts, transactions, and loans.
2. Add registration, KYC data collection, login, and separate customer and administrator menus.
3. Implement account operations and transaction handling, including transfers and currency conversion.
4. Add loan calculations and the loan application flow.
5. Persist the user and account data as JSON and verify the main console workflows.

## Source Code Structure

The code is organized by responsibility in C# classes. The main menu classes handle console interaction, while model classes represent banking data and service or utility classes provide shared operations.

| Class or type | Responsibility |
| --- | --- |
| `Program` | Application entry point. Loads saved users, ensures an administrator account exists, starts transaction processing, and routes signed-in users to the correct menu. |
| `User` | Represents a customer or administrator. Holds profile details, accounts, loans, role, and login lockout state. Also contains registration, KYC, login, and account-selection behavior. |
| `UserRole` | Enum that distinguishes customer and administrator users. |
| `UserMenu` | Customer-facing console menus for profile details, accounts, deposits, transfers, transaction history, and loans. |
| `AdminMenu` | Administrator-facing console menu for creating users, unlocking accounts, changing exchange rates, and viewing user statistics. |
| `Account` | Represents a bank account and provides account operations such as opening accounts, deposits, withdrawals, and transaction-log access. |
| `SavingsAccount` | Specialized account type that adds savings-interest information and calculations. |
| `CurrencyType` | Enum for the currencies supported by accounts and currency conversion. |
| `NotificationChannel` | Enum describing how a user prefers to receive notifications. |
| `Transaction` | Records transaction details such as type, source, destination, amount, timestamp, and status. |
| `TransactionService` | Coordinates deposits and transfers, including transfers between customers and the timed processing flow. |
| `ExchangeRateManager` | Holds exchange rates and converts amounts between supported currencies. |
| `Loan` | Represents a loan and handles loan applications, payout to an account, and displaying a user's loans and estimated costs. |
| `LoanRules` | Contains loan policy calculations, including the maximum loan amount and annual interest-rate tiers. |
| `UserStore` | Loads and saves the user list as JSON so that data remains available between application runs. |
| `UserStats` | Calculates and displays user-related statistics for the administrator menu. |
| `Ui` | Shared console helpers for prompts, menus, input validation, status messages, and screen formatting. |

## Main Objects and Relationships

- A `User` has a role, profile details, a list of `Account` objects, and a list of `Loan` objects.
- `SavingsAccount` extends the general `Account` behavior with savings interest.
- An `Account` keeps its transaction history as `Transaction` objects.
- `UserMenu` and `AdminMenu` collect choices from the console and call the relevant classes and services.
- `Loan` uses `LoanRules` to determine loan limits and interest rates.
- `TransactionService` uses `ExchangeRateManager` when an operation involves different currencies.
- `Program` uses `UserStore` at startup and during application operations to persist user data.

## UML

![UML diagram](uml.png)

## Typical Application Flow

1. `Program` loads users from `UserStore` and shows the start menu.
2. A new customer can register through `User.CreateAccount()`, which collects KYC details and creates an initial account.
3. A signed-in customer is sent to `UserMenu`; an administrator is sent to `AdminMenu`.
4. Menu actions call account, transaction, currency, or loan functionality.
5. Changes are saved by `UserStore` as JSON.

## Running the Application

With the .NET SDK installed, open a terminal in the project directory and run:

```bash
dotnet run
```

The application reads and writes `users.json` in its runtime application directory. This project is a console-based learning example and is not intended for handling real financial data.
