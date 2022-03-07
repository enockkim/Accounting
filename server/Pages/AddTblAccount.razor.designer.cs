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
    public partial class AddTblAccountComponent : ComponentBase
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

        IEnumerable<Accounting.Models.AccountingDb.AccountType> _getAccountTypesForaccount_type_idResult;
        protected IEnumerable<Accounting.Models.AccountingDb.AccountType> getAccountTypesForaccount_type_idResult
        {
            get
            {
                return _getAccountTypesForaccount_type_idResult;
            }
            set
            {
                if (!object.Equals(_getAccountTypesForaccount_type_idResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAccountTypesForaccount_type_idResult", NewValue = value, OldValue = _getAccountTypesForaccount_type_idResult };
                    _getAccountTypesForaccount_type_idResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Accounting.Models.AccountingDb.TblAccount _tblaccount;
        protected Accounting.Models.AccountingDb.TblAccount tblaccount
        {
            get
            {
                return _tblaccount;
            }
            set
            {
                if (!object.Equals(_tblaccount, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "tblaccount", NewValue = value, OldValue = _tblaccount };
                    _tblaccount = value;
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
            getAccountTypesForaccount_type_idResult = accountingDbGetAccountTypesResult;

            tblaccount = new Accounting.Models.AccountingDb.TblAccount(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Accounting.Models.AccountingDb.TblAccount args)
        {
            try
            {
                var accountingDbCreateTblAccountResult = await AccountingDb.CreateTblAccount(tblaccount);
                DialogService.Close(tblaccount);
            }
            catch (System.Exception accountingDbCreateTblAccountException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new TblAccount!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
