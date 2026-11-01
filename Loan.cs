using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp;

public class Loan
{
    public decimal LoanAmount { get; set; }
    public decimal AccountBalanceAtApproval { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int LoanDurationYears { get; set; }
    public long PayoutAccountNumber { get; set; }

    public decimal InterestCost =>
        LoanAmount * AnnualInterestRate * LoanDurationYears;

    public decimal TotalCost =>
        LoanAmount + InterestCost;

    
public static void ApplyForLoan()
    {

    }
}