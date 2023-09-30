using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class QueriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            return View();
        }
        public QueriesController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<IActionResult> TopTransactionAmounts()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT TOP 5 PERCENT c.FirstName, c.LastName, l.Amount
                                        FROM Ledger.Ledger l
                                        JOIN Ledger.Accounts a ON l.AccountId = a.Id
                                        JOIN Core.Customers c ON a.CustomerId = c.Id
                                        ORDER BY l.Amount DESC;";

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<TopTransactionAmountsViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new TopTransactionAmountsViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> TransactionTypeConsistencyOverTime()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT l.Type, COUNT(*) AS NumberOfTransactions, AVG(l.Amount) AS AverageTransactionAmount
                                        FROM Ledger.Ledger l
                                        GROUP BY l.Type
                                        ORDER BY NumberOfTransactions DESC;";
                using var reader = await command.ExecuteReaderAsync();
                var result = new List<TransactionTypeConsistencyViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new TransactionTypeConsistencyViewModel
                    {
                        Type = reader.GetInt32(reader.GetOrdinal("Type")).ToString(), // Get as int and then convert to string
                        NumberOfTransactions = reader.GetInt32(reader.GetOrdinal("NumberOfTransactions")),
                        AverageTransactionAmount = reader.GetDecimal(reader.GetOrdinal("AverageTransactionAmount"))
                    });
                }


                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
       
       
        [HttpGet]
        public async Task<IActionResult> HighValueCustomers()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();


                command.CommandText = @"SELECT c.FirstName, c.LastName, SUM(l.Amount) AS TotalTransactionAmount
                        FROM Ledger.Ledger l
                        JOIN Ledger.Accounts a ON l.AccountId = a.Id
                        JOIN Core.Customers c ON a.CustomerId = c.Id
                        GROUP BY c.FirstName, c.LastName
                        HAVING SUM(l.Amount) > 100000;";

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<HighValueCustomersViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new HighValueCustomersViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TotalTransactionAmount = reader.GetDecimal(reader.GetOrdinal("TotalTransactionAmount"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AccountBalanceStatus()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT c.FirstName, c.LastName, a.Balance
                        FROM Ledger.Accounts a
                        JOIN Core.Customers c ON a.CustomerId = c.Id
                        WHERE a.Balance < 0;";


                using var reader = await command.ExecuteReaderAsync();
                var result = new List<AccountBalanceStatusViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new AccountBalanceStatusViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Balance = reader.GetDecimal(reader.GetOrdinal("Balance"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> BankerPerformance()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT b.FirstName, b.LastName, COUNT(a.Id) AS NumberOfAccountsManaged
                        FROM Ledger.Accounts a
                        JOIN Core.Bankers b ON a.BankerId = b.Id
                        GROUP BY b.FirstName, b.LastName;";

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<BankerPerformanceViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new BankerPerformanceViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        NumberOfAccountsManaged = reader.GetInt32(reader.GetOrdinal("NumberOfAccountsManaged"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CustomerSalaryVsLoanAmount()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT c.FirstName, c.LastName, c.SalaryAmount, SUM(l.Amount) AS TotalLoanAmount
                        FROM Ledger.Loans l
                        JOIN Ledger.Accounts a ON l.AccountId = a.Id
                        JOIN Core.Customers c ON a.CustomerId = c.Id
                        GROUP BY c.FirstName, c.LastName, c.SalaryAmount;";


                using var reader = await command.ExecuteReaderAsync();
                var result = new List<CustomerSalaryVsLoanAmountViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new CustomerSalaryVsLoanAmountViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        SalaryAmount = reader.GetDecimal(reader.GetOrdinal("SalaryAmount")),
                        TotalLoanAmount = reader.GetDecimal(reader.GetOrdinal("TotalLoanAmount"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OverdueLoans()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
               command.CommandText = @"SELECT c.FirstName, c.LastName, l.Amount
                        FROM Ledger.Loans l
                        JOIN Ledger.Accounts a ON l.AccountId = a.Id
                        JOIN Core.Customers c ON a.CustomerId = c.Id
                        WHERE l.Term < 0;";


                using var reader = await command.ExecuteReaderAsync();
                var result = new List<OverdueLoansViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new OverdueLoansViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> MostCommonTransactionTypes()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT l.Type, COUNT(*) AS NumberOfOccurrences
                        FROM Ledger.Ledger l
                        GROUP BY l.Type
                        ORDER BY NumberOfOccurrences DESC;";


                using var reader = await command.ExecuteReaderAsync();
                var result = new List<MostCommonTransactionTypesViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new MostCommonTransactionTypesViewModel
                    {
                        Type = reader.GetInt32(reader.GetOrdinal("Type")).ToString(),

                        NumberOfOccurrences = reader.GetInt32(reader.GetOrdinal("NumberOfOccurrences"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CustomerLoanToSalaryRatio()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = @"SELECT c.FirstName, c.LastName, (SUM(l.Amount) / c.SalaryAmount) AS LoanToSalaryRatio
                        FROM Ledger.Loans l
                        JOIN Ledger.Accounts a ON l.AccountId = a.Id
                        JOIN Core.Customers c ON a.CustomerId = c.Id
                        GROUP BY c.FirstName, c.LastName, c.SalaryAmount;";


                using var reader = await command.ExecuteReaderAsync();
                var result = new List<CustomerLoanToSalaryRatioViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new CustomerLoanToSalaryRatioViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        LoanToSalaryRatio = reader.GetDecimal(reader.GetOrdinal("LoanToSalaryRatio"))
                    });
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }

}
