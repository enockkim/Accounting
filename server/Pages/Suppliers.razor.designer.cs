using System;
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
    public partial class SuppliersComponent : ComponentBase
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
        protected RadzenDataGrid<Accounting.Models.AccountingDb.Supplier> grid0;

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

        IEnumerable<Accounting.Models.AccountingDb.Supplier> _getSuppliersResult;
        protected IEnumerable<Accounting.Models.AccountingDb.Supplier> getSuppliersResult
        {
            get
            {
                return _getSuppliersResult;
            }
            set
            {
                if (!object.Equals(_getSuppliersResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSuppliersResult", NewValue = value, OldValue = _getSuppliersResult };
                    _getSuppliersResult = value;
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

            var accountingDbGetSuppliersResult = await AccountingDb.GetSuppliers(new Query() { Filter = $@"i => i.Company_Name.Contains(@0) || i.Company_Address.Contains(@1) || i.Company_Telephone_Number.Contains(@2) || i.Company_City.Contains(@3) || i.Contact_Person_Name.Contains(@4) || i.Contact_Person_Designation.Contains(@5) || i.Contact_Person_Tel_number.Contains(@6) || i.supplier_img.Contains(@7) || i.Added_By.Contains(@8)", FilterParameters = new object[] { search, search, search, search, search, search, search, search, search } });
            getSuppliersResult = accountingDbGetSuppliersResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSupplier>("Add Supplier", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await AccountingDb.ExportSuppliersToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "suppliersID,Company_Name,Company_Address,Company_Telephone_Number,Company_City,Registration_date,Contact_Person_Name,Contact_Person_Designation,Contact_Person_Tel_number,supplier_img,DateAdded,Added_By,expenseAccount,revenueAccount" }, $"Suppliers");

            }

            if (args == null || args.Value == "xlsx")
            {
                await AccountingDb.ExportSuppliersToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "suppliersID,Company_Name,Company_Address,Company_Telephone_Number,Company_City,Registration_date,Contact_Person_Name,Contact_Person_Designation,Contact_Person_Tel_number,supplier_img,DateAdded,Added_By,expenseAccount,revenueAccount" }, $"Suppliers");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(DataGridRowMouseEventArgs<Accounting.Models.AccountingDb.Supplier> args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSupplier>("Edit Supplier", new Dictionary<string, object>() { {"suppliersID", args.Data.suppliersID} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var accountingDbDeleteSupplierResult = await AccountingDb.DeleteSupplier(data.suppliersID);
                    if (accountingDbDeleteSupplierResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception accountingDbDeleteSupplierException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Supplier" });
            }
        }
    }
}
