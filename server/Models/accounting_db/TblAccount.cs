using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models.AccountingDb
{
  [Table("tbl_accounts")]
  public partial class TblAccount
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int gl_account_no
    {
      get;
      set;
    }
    public string gl_account_name
    {
      get;
      set;
    }
    public decimal? current_balance
    {
      get;
      set;
    }
    public string opening_balance
    {
      get;
      set;
    }
    public DateTime? date
    {
      get;
      set;
    }
    public string status
    {
      get;
      set;
    }
    public int account_type_id
    {
      get;
      set;
    }
    public AccountType AccountType { get; set; }
  }
}
