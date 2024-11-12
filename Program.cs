

namespace BankingApp
{
    public class User
    {
        public string Username { get; set; }  // getters setters 
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public bool ValidateLogin(string username, string password)
        {
            return Username == username && Password == password;
        }
    }

    public class Account
    {
        private static int accountCounter = 1000;  // account number start with 1000
        public int AccountNumber { get; private set; }
        public string AccountHolderName { get; private set; }
        public string AccountType { get; private set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        private DateTime lastInterestDate;

        public Account(string accountHolderName, string accountType, decimal initialDeposit)
        {
            AccountNumber = accountCounter++;
            AccountHolderName = accountHolderName;
            AccountType = accountType;
            Balance = initialDeposit;
            Transactions = new List<Transaction>();
            lastInterestDate = DateTime.Now;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new Transaction("Deposit", amount));
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return false;
            }
            Balance -= amount;
            Transactions.Add(new Transaction("Withdrawal", amount));
            return true;
        }

        public void GenerateStatement()
        {
            Console.WriteLine($"Statement for Account {AccountNumber} - {AccountHolderName}");
            Console.WriteLine("Date\t\tType\tAmount");
            foreach (var transaction in Transactions)
            {
                Console.WriteLine($"{transaction.Date}\t{transaction.Type}\t{transaction.Amount}");
            }
            Console.WriteLine($"Current Balance: {Balance}");
        }

        public void CheckBalance()
        {
            Console.WriteLine($"Account {AccountNumber} - Balance: {Balance:C}");
        }

        public void ApplyMonthlyInterest(decimal interestRate)
        {
            if (AccountType == "Savings")
            {
                // as it is console based that why i have used true in the if to check it
                if (true || lastInterestDate.Month != DateTime.Now.Month || lastInterestDate.Year != DateTime.Now.Year)
                {
                    var interest = Balance * interestRate;
                    Balance += interest;
                    Transactions.Add(new Transaction("Interest", interest));
                    lastInterestDate = DateTime.Now;
                    Console.WriteLine($"Interest of {interest:C} added to Account {AccountNumber}.");
                }
                else
                {
                    Console.WriteLine("Interest has already been applied for this month.");
                }
            }
        }
    }

    public class Transaction
    {
        private static int transactionCounter = 1;
        public int TransactionId { get; private set; }
        public DateTime Date { get; private set; }
        public string Type { get; private set; }
        public decimal Amount { get; private set; }

        public Transaction(string type, decimal amount)
        {
            TransactionId = transactionCounter++;
            Date = DateTime.Now;
            Type = type;
            Amount = amount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<User> users = new List<User>();
            List<Account> accounts = new List<Account>();
            User loggedInUser = null;

            while (true)
            {
                if (loggedInUser == null)
                {
                    // in start only shows 3 option 
                    Console.Clear();
                    Console.WriteLine("Banking Application Menu:");
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            RegisterUser(users);
                            break;
                        case "2":
                            loggedInUser = Login(users);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }
                else
                {
                    // after login user can do all other things
                    Console.Clear();
                    Console.WriteLine($"Welcome, {loggedInUser.Username}!");
                    Console.WriteLine("Banking Application Menu:");
                    Console.WriteLine("1. Open Account");
                    Console.WriteLine("2. Deposit");
                    Console.WriteLine("3. Withdraw");
                    Console.WriteLine("4. Generate Statement");
                    Console.WriteLine("5. Check Balance");
                    Console.WriteLine("6. Apply Interest");
                    Console.WriteLine("7. Logout");
                    Console.WriteLine("8. Exit");
                    Console.Write("Select an option: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            OpenAccount(accounts, loggedInUser);
                            break;
                        case "2":
                            PerformDeposit(accounts);
                            break;
                        case "3":
                            PerformWithdrawal(accounts);
                            break;
                        case "4":
                            GenerateAccountStatement(accounts);
                            break;
                        case "5":
                            CheckAccountBalance(accounts);
                            break;
                        case "6":
                            ApplyInterest(accounts);
                            break;
                        case "7":
                            loggedInUser = null; 
                            Console.WriteLine("Logged out successfully.");
                            break;
                        case "8":
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void RegisterUser(List<User> users)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            users.Add(new User(username, password));
            Console.WriteLine("Registration successful.");
        }

        static User Login(List<User> users)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            foreach (var user in users)
            {
                if (user.ValidateLogin(username, password))
                {
                    Console.WriteLine("Login successful.");
                    return user;
                }
            }
            Console.WriteLine("Invalid credentials.");
            return null;
        }

        static void OpenAccount(List<Account> accounts, User user)
        {
            Console.Write("Enter account holder's name: ");
            string name = Console.ReadLine();
            Console.Write("Enter account type (Savings/Current): ");
            string type = Console.ReadLine();
            Console.Write("Enter initial deposit amount: ");
            decimal initialDeposit = decimal.Parse(Console.ReadLine());

            Account newAccount = new Account(name, type, initialDeposit);
            accounts.Add(newAccount);
            Console.WriteLine($"Account opened successfully. Your account number is: {newAccount.AccountNumber}");
        }

        static void PerformDeposit(List<Account> accounts)
        {
            Console.Write("Enter account number: ");
            int accountNumber = int.Parse(Console.ReadLine());
            Console.Write("Enter deposit amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.Deposit(amount);
                Console.WriteLine("Deposit successful.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        static void PerformWithdrawal(List<Account> accounts)
        {
            Console.Write("Enter account number: ");
            int accountNumber = int.Parse(Console.ReadLine());
            Console.Write("Enter withdrawal amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                if (account.Withdraw(amount))
                    Console.WriteLine("Withdrawal successful.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        static void GenerateAccountStatement(List<Account> accounts)
        {
            Console.Write("Enter account number: ");
            int accountNumber = int.Parse(Console.ReadLine());

            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.GenerateStatement();
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        static void CheckAccountBalance(List<Account> accounts)
        {
            Console.Write("Enter account number: ");
            int accountNumber = int.Parse(Console.ReadLine());

            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.CheckBalance();
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        static void ApplyInterest(List<Account> accounts)
        {
            Console.Write("Enter account number: ");
            int accountNumber = int.Parse(Console.ReadLine());

            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                decimal interestRate = 0.05m; //fixed intrest rate 5% 
                account.ApplyMonthlyInterest(interestRate);
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }
    }
}
