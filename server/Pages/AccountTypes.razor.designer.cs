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
    public partial class AccountTypesComponent : ComponentBase
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
        protected RadzenDataGrid<Accounting.Models.AccountingDb.AccountType> grid0;

        IEnumerable<Accounting.Models.AccountingDb.AccountType> _getAccountTypesResult;
        protected IEnumerable<Accounting.Models.AccountingDb.AccountType> getAccountTypesResult
        {
            get
            {
                return _getAccountTypesResult;
            }
            set
            {
                if (!object.Equals(_getAccountTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAccountTypesResult", NewValue = value, OldValue = _getAccountTypesResult };
                    _getAccountTypesResult = value;
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
            var accountingDbGetAccountTypesResult = await AccountingDb.GetAccountTypes();
            getAccountTypesResult = accountingDbGetAccountTypesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAccountType>("Add Account Type", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Accounting.Models.AccountingDb.AccountType args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAccountType>("Edit Account Type", new Dictionary<string, object>() { {"account_type_id", args.account_type_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var accountingDbDeleteAccountTypeResult = await AccountingDb.DeleteAccountType(data.account_type_id);
                    if (accountingDbDeleteAccountTypeResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception accountingDbDeleteAccountTypeException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete AccountType" });
            }
        }
    }
}
