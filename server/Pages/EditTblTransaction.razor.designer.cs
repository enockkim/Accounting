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
    public partial class EditTblTransactionComponent : ComponentBase
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

        [Parameter]
        public dynamic transaction_id { get; set; }

        Accounting.Models.AccountingDb.TblTransaction _tbltransaction;
        protected Accounting.Models.AccountingDb.TblTransaction tbltransaction
        {
            get
            {
                return _tbltransaction;
            }
            set
            {
                if (!object.Equals(_tbltransaction, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "tbltransaction", NewValue = value, OldValue = _tbltransaction };
                    _tbltransaction = value;
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
            var accountingDbGetTblTransactionBytransactionIdResult = await AccountingDb.GetTblTransactionBytransactionId(transaction_id);
            tbltransaction = accountingDbGetTblTransactionBytransactionIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Accounting.Models.AccountingDb.TblTransaction args)
        {
            try
            {
                var accountingDbUpdateTblTransactionResult = await AccountingDb.UpdateTblTransaction(transaction_id, tbltransaction);
                DialogService.Close(tbltransaction);
            }
            catch (System.Exception accountingDbUpdateTblTransactionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update TblTransaction" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
