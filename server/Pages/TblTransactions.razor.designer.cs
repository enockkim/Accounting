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
    public partial class TblTransactionsComponent : ComponentBase
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
        protected RadzenDataGrid<Accounting.Models.AccountingDb.TblTransaction> grid0;

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

        IEnumerable<Accounting.Models.AccountingDb.TblTransaction> _getTblTransactionsResult;
        protected IEnumerable<Accounting.Models.AccountingDb.TblTransaction> getTblTransactionsResult
        {
            get
            {
                return _getTblTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getTblTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getTblTransactionsResult", NewValue = value, OldValue = _getTblTransactionsResult };
                    _getTblTransactionsResult = value;
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

            var accountingDbGetTblTransactionsResult = await AccountingDb.GetTblTransactions(new Query() { Filter = $@"i => i.description.Contains(@0)", FilterParameters = new object[] { search }, Expand = "TblAccount,AccountType" });
            getTblTransactionsResult = accountingDbGetTblTransactionsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTblTransaction>("Add Tbl Transaction", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await AccountingDb.ExportTblTransactionsToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "TblAccount,AccountType", Select = "transaction_id,general_ledger_id,TblAccount.gl_account_name as TblAccountgl_account_name,description,credit,debit,AccountType.account_type_name as AccountTypeaccount_type_name" }, $"Tbl Transactions");

            }

            if (args == null || args.Value == "xlsx")
            {
                await AccountingDb.ExportTblTransactionsToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "TblAccount,AccountType", Select = "transaction_id,general_ledger_id,TblAccount.gl_account_name as TblAccountgl_account_name,description,credit,debit,AccountType.account_type_name as AccountTypeaccount_type_name" }, $"Tbl Transactions");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(DataGridRowMouseEventArgs<Accounting.Models.AccountingDb.TblTransaction> args)
        {
            var dialogResult = await DialogService.OpenAsync<EditTblTransaction>("Edit Tbl Transaction", new Dictionary<string, object>() { {"transaction_id", args.Data.transaction_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var accountingDbDeleteTblTransactionResult = await AccountingDb.DeleteTblTransaction(data.transaction_id);
                    if (accountingDbDeleteTblTransactionResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception accountingDbDeleteTblTransactionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete TblTransaction" });
            }
        }
    }
}
