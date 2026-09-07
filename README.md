# Banking System Simulator

A C# console application developed for **ITCS-3112: Design and Implementation of Object-Oriented Systems**. It models customer accounts, staff workflows, transfers, and a simplified investment system to explore object-oriented application design.

## Features

- Account creation and PIN-based login.
- Separate customer, employee, and manager menus.
- Account balances and transaction activity.
- Transfers between accounts.
- PIN changes and staff-assisted account workflows.
- Simulated investments with periodically changing prices.

## Build and run

The project targets **.NET Framework 4.7.2** and uses the traditional Visual Studio project format.

1. Install Visual Studio on Windows with the **.NET desktop development** workload and the **.NET Framework 4.7.2 targeting pack**.
2. Clone the repository:

   ```bash
   git clone https://github.com/oabdulr/Banking-System.git
   ```

3. Open `Banking System (ITCS-3112).sln` in Visual Studio.
4. Build the solution and start the console application.
5. Choose **Create Account**, then log in with the generated account number and your PIN.

Follow the numbered menus to view activity, transfer funds, or explore investments. The application seeds sample accounts and gives newly constructed accounts a simulated starting balance. All data is held in memory and resets when the process restarts.

## Design

- [Program.cs](Banking%20System%20%28ITCS-3112%29/Program.cs): application loop and background investment updates.
- [Bank.cs](Banking%20System%20%28ITCS-3112%29/Bank%20Data/Bank.cs): account lookup, account creation, and transfer coordination.
- [Account.cs](Banking%20System%20%28ITCS-3112%29/Bank%20Data/Account.cs): shared account state and operations.
- [Accounts](Banking%20System%20%28ITCS-3112%29/Bank%20Data/Accounts): customer, employee, and manager subclasses with specialized menus.
- [Transaction.cs](Banking%20System%20%28ITCS-3112%29/Bank%20Data/Transaction.cs): transaction validation and execution.
- [Company.cs](Banking%20System%20%28ITCS-3112%29/Bank%20Data/Investments/Company.cs): simulated company values and investment records.

The project demonstrates inheritance, method overriding, collection-based state management, and coordination between domain objects. Console interaction currently lives alongside domain logic.

## Educational scope

This application uses simulated funds and is not suitable for real financial data or credentials. The PIN transformation is a custom mathematical function rather than a secure password-hashing implementation. Balances currently use floating-point values, and there is no database or comprehensive authorization layer.

Future improvements would include standard credential hashing, decimal monetary values, persistent storage, separation of UI and business logic, and tests covering transfer failures, repeated transactions, and balance invariants. An automated test suite is not currently included.
