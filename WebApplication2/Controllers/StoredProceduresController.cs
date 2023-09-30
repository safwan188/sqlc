using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class StoredProceduresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public StoredProceduresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCustomerLoansAndInvestments()
        {
            return View(new CustomerLoansAndInvestmentsCombinedViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomerLoansAndInvestments(CustomerLoansAndInvestmentsCombinedViewModel inputModel)
        {
            if (!ModelState.IsValid)
                return View(inputModel);

            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC GetCustomerLoansAndInvestments @CustomerId";
                command.Parameters.Add(new SqlParameter("@CustomerId", inputModel.CustomerId));

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<CustomerLoansAndInvestmentsViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new CustomerLoansAndInvestmentsViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TotalLoanAmount = reader.GetDecimal(reader.GetOrdinal("TotalLoanAmount")),
                        TotalInvestment = reader.GetDecimal(reader.GetOrdinal("TotalInvestment"))
                    });
                }

                inputModel.Result = result.FirstOrDefault();
                return View(inputModel);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, "Internal server error");
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> AggregateInfoPerBanker(AggregateInfoPerBankerCombinedViewModel inputModel)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC sp_AggregateInfoPerBanker";

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<AggregateInfoPerBankerViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new AggregateInfoPerBankerViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        NumberOfAccountsManaged = reader.GetInt32(reader.GetOrdinal("NumberOfAccountsManaged")),
                        AverageAccountBalance = reader.GetDecimal(reader.GetOrdinal("AverageAccountBalance")),
                        TotalLoansApproved = reader.GetInt32(reader.GetOrdinal("TotalLoansApproved")),
                        AverageLoanAmount = reader.GetDecimal(reader.GetOrdinal("AverageLoanAmount"))
                    });
                }

                inputModel.Results = result;
                return View(inputModel);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> CustomerDetailsWithBothAccounts()
        {
            try
            {
                var viewModel = new CustomerDetailsWithBothAccountsCombinedViewModel();
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC sp_CustomerDetailsWithBothAccounts";

                using var reader = await command.ExecuteReaderAsync();
                viewModel.Results = new List<CustomerDetailsWithBothAccountsViewModel>();

                while (await reader.ReadAsync())
                {
                    viewModel.Results.Add(new CustomerDetailsWithBothAccountsViewModel
                    {
                        CustomerFirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                        CustomerLastName = reader.GetString(reader.GetOrdinal("CustomerLastName")),
                        ManagerFirstName = reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                        ManagerLastName = reader.GetString(reader.GetOrdinal("ManagerLastName"))
                    });
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, "Internal server error");
            }
        }


        public async Task<IActionResult> TotalInvestmentPerCustomer()
        {
            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "EXEC sp_TotalInvestmentPerCustomer";

                using var reader = await command.ExecuteReaderAsync();
                var result = new List<TotalInvestmentPerCustomerViewModel>();

                while (await reader.ReadAsync())
                {
                    result.Add(new TotalInvestmentPerCustomerViewModel
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        TotalInvestment = reader.GetDecimal(reader.GetOrdinal("TotalInvestment"))
                    });
                }

                return View(result); // Assuming there is a corresponding View
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, "Internal server error");
            }
        }

    }

}
