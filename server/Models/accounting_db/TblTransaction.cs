using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models.AccountingDb
{
  [Table("tbl_transactions")]
  public partial class TblTransaction
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int transaction_id
    {
      get;
      set;
    }
    public int? general_ledger_id
    {
      get;
      set;
    }
    public int? account_id
    {
      get;
      set;
    }
    public TblAccount TblAccount { get; set; }
    public string description
    {
      get;
      set;
    }
    public int? credit
    {
      get;
      set;
    }
    public int? debit
    {
      get;
      set;
    }
    public int? account_type
    {
      get;
      set;
    }
    public AccountType AccountType { get; set; }
  }
}
