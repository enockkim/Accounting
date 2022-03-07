using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models.AccountingDb
{
  [Table("account_type")]
  public partial class AccountType
  {
    [Key]
    public int account_type_id
    {
      get;
      set;
    }

    public ICollection<TblAccount> TblAccounts { get; set; }
    public ICollection<TblTransaction> TblTransactions { get; set; }
    public string account_type_name
    {
      get;
      set;
    }
  }
}
