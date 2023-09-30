namespace WebApplication2.Models
{
    // ViewModel for GetCustomerLoansAndInvestments Procedure
    public class CustomerLoansAndInvestmentsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalLoanAmount { get; set; }
        public decimal TotalInvestment { get; set; }
    }

    // ViewModel for sp_AggregateInfoPerBanker Procedure
    public class AggregateInfoPerBankerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfAccountsManaged { get; set; }
        public decimal AverageAccountBalance { get; set; }
        public int TotalLoansApproved { get; set; }
        public decimal AverageLoanAmount { get; set; }
    }

    // ViewModel for sp_CustomerDetailsWithBothAccounts Procedure
    public class CustomerDetailsWithBothAccountsViewModel
    {
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }

    // ViewModel for sp_TotalInvestmentPerCustomer Procedure
    public class TotalInvestmentPerCustomerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalInvestment { get; set; }
    }
    
    
      public class CustomerLoansAndInvestmentsCombinedViewModel
        {
            public Guid CustomerId { get; set; } // For input
            public CustomerLoansAndInvestmentsViewModel? Result { get; set; } // For output
        }
    public class CustomerDetailsWithBothAccountsCombinedViewModel
    {
        public List<CustomerDetailsWithBothAccountsViewModel> Results { get; set; }
    }

    public class AggregateInfoPerBankerCombinedViewModel
    {
        public List<AggregateInfoPerBankerViewModel> Results { get; set; }
    }
    public class AverageInterestFeesPerBankerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AverageInterestFees { get; set; }
    }
    public class TopTransactionAmountsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Amount { get; set; }
    }
    public class TransactionTypeConsistencyViewModel
    {
        public string Type { get; set; }
        public int NumberOfTransactions { get; set; }
        public decimal AverageTransactionAmount { get; set; }
    }
    public class HighValueCustomersViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
    public class AccountBalanceStatusViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
    }
    public class BankerPerformanceViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfAccountsManaged { get; set; }
    }
    public class CustomerSalaryVsLoanAmountViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal TotalLoanAmount { get; set; }
    }
    public class OverdueLoansViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Amount { get; set; }
    }
    public class MostCommonTransactionTypesViewModel
    {
        public string Type { get; set; }
        public int NumberOfOccurrences { get; set; }
    }
    public class CustomerLoanToSalaryRatioViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal LoanToSalaryRatio { get; set; }
    }

}
