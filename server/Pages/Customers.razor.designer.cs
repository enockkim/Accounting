﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Accounting.Models.AccountingDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Accounting.Models;

namespace Accounting.Pages
{
    public partial class CustomersComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected AccountingDbService AccountingDb { get; set; }
        protected RadzenDataGrid<Accounting.Models.AccountingDb.Customer> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Accounting.Models.AccountingDb.Customer> _getCustomersResult;
        protected IEnumerable<Accounting.Models.AccountingDb.Customer> getCustomersResult
        {
            get
            {
                return _getCustomersResult;
            }
            set
            {
                if (!object.Equals(_getCustomersResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCustomersResult", NewValue = value, OldValue = _getCustomersResult };
                    _getCustomersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var accountingDbGetCustomersResult = await AccountingDb.GetCustomers(new Query() { Filter = $@"i => i.Company_Name.Contains(@0) || i.Company_Address.Contains(@1) || i.Company_Telephone_Number.Contains(@2) || i.Company_City.Contains(@3) || i.Contact_Person_Name.Contains(@4) || i.Contact_Person_Designation.Contains(@5) || i.Contact_Person_Tel_number.Contains(@6) || i.customer_img.Contains(@7) || i.Added_By.Contains(@8)", FilterParameters = new object[] { search, search, search, search, search, search, search, search, search } });
            getCustomersResult = accountingDbGetCustomersResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCustomer>("Add Customer", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await AccountingDb.ExportCustomersToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "customersID,Company_Name,Company_Address,Company_Telephone_Number,Company_City,Registration_date,Contact_Person_Name,Contact_Person_Designation,Contact_Person_Tel_number,customer_img,DateAdded,Added_By,revenueAccount,expenseAccount" }, $"Customers");

            }

            if (args == null || args.Value == "xlsx")
            {
                await AccountingDb.ExportCustomersToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "customersID,Company_Name,Company_Address,Company_Telephone_Number,Company_City,Registration_date,Contact_Person_Name,Contact_Person_Designation,Contact_Person_Tel_number,customer_img,DateAdded,Added_By,revenueAccount,expenseAccount" }, $"Customers");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(DataGridRowMouseEventArgs<Accounting.Models.AccountingDb.Customer> args)
        {
            var dialogResult = await DialogService.OpenAsync<EditCustomer>("Edit Customer", new Dictionary<string, object>() { {"customersID", args.Data.customersID} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var accountingDbDeleteCustomerResult = await AccountingDb.DeleteCustomer(data.customersID);
                    if (accountingDbDeleteCustomerResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception accountingDbDeleteCustomerException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Customer" });
            }
        }
    }
}
