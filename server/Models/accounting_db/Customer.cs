using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Models.AccountingDb
{
  [Table("customers")]
  public partial class Customer
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int customersID
    {
      get;
      set;
    }
    public string Company_Name
    {
      get;
      set;
    }
    public string Company_Address
    {
      get;
      set;
    }
    public string Company_Telephone_Number
    {
      get;
      set;
    }
    public string Company_City
    {
      get;
      set;
    }
    public DateTime Registration_date
    {
      get;
      set;
    }
    public string Contact_Person_Name
    {
      get;
      set;
    }
    public string Contact_Person_Designation
    {
      get;
      set;
    }
    public string Contact_Person_Tel_number
    {
      get;
      set;
    }
    public string customer_img
    {
      get;
      set;
    }
    public DateTime DateAdded
    {
      get;
      set;
    }
    public string Added_By
    {
      get;
      set;
    }
    public int? revenueAccount
    {
      get;
      set;
    }
    public int? expenseAccount
    {
      get;
      set;
    }
  }
}
