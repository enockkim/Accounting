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
    public partial class TblAccountsComponent : ComponentBase
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
        protected RadzenDataGrid<Accounting.Models.AccountingDb.TblAccount> grid0;

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

        IEnumerable<Accounting.Models.AccountingDb.TblAccount> _getTblAccountsResult;
        protected IEnumerable<Accounting.Models.AccountingDb.TblAccount> getTblAccountsResult
        {
            get
            {
                return _getTblAccountsResult;
            }
            set
            {
                if (!object.Equals(_getTblAccountsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getTblAccountsResult", NewValue = value, OldValue = _getTblAccountsResult };
                    _getTblAccountsResult = value;
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

            var accountingDbGetTblAccountsResult = await AccountingDb.GetTblAccounts(new Query() { Filter = $@"i => i.gl_account_name.Contains(@0) || i.opening_balance.Contains(@1) || i.status.Contains(@2)", FilterParameters = new object[] { search, search, search }, Expand = "AccountType" });
            getTblAccountsResult = accountingDbGetTblAccountsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTblAccount>("Add Tbl Account", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await AccountingDb.ExportTblAccountsToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AccountType", Select = "gl_account_no,gl_account_name,current_balance,opening_balance,date,status,AccountType.account_type_name as AccountTypeaccount_type_name" }, $"Tbl Accounts");

            }

            if (args == null || args.Value == "xlsx")
            {
                await AccountingDb.ExportTblAccountsToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AccountType", Select = "gl_account_no,gl_account_name,current_balance,opening_balance,date,status,AccountType.account_type_name as AccountTypeaccount_type_name" }, $"Tbl Accounts");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(DataGridRowMouseEventArgs<Accounting.Models.AccountingDb.TblAccount> args)
        {
            var dialogResult = await DialogService.OpenAsync<EditTblAccount>("Edit Tbl Account", new Dictionary<string, object>() { {"gl_account_no", args.Data.gl_account_no} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var accountingDbDeleteTblAccountResult = await AccountingDb.DeleteTblAccount(data.gl_account_no);
                    if (accountingDbDeleteTblAccountResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception accountingDbDeleteTblAccountException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete TblAccount" });
            }
        }
    }
}
