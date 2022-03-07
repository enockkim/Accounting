using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.Data;

namespace Accounting
{
    public partial class ExportAccountingDbController : ExportController
    {
        private readonly AccountingDbContext context;
        private readonly AccountingDbService service;
        public ExportAccountingDbController(AccountingDbContext context, AccountingDbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/AccountingDb/accounttypes/csv")]
        [HttpGet("/export/AccountingDb/accounttypes/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAccountTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAccountTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/AccountingDb/accounttypes/excel")]
        [HttpGet("/export/AccountingDb/accounttypes/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAccountTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAccountTypes(), Request.Query), fileName);
        }
        [HttpGet("/export/AccountingDb/customers/csv")]
        [HttpGet("/export/AccountingDb/customers/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }

        [HttpGet("/export/AccountingDb/customers/excel")]
        [HttpGet("/export/AccountingDb/customers/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCustomersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCustomers(), Request.Query), fileName);
        }
        [HttpGet("/export/AccountingDb/suppliers/csv")]
        [HttpGet("/export/AccountingDb/suppliers/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportSuppliersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSuppliers(), Request.Query), fileName);
        }

        [HttpGet("/export/AccountingDb/suppliers/excel")]
        [HttpGet("/export/AccountingDb/suppliers/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportSuppliersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSuppliers(), Request.Query), fileName);
        }
        [HttpGet("/export/AccountingDb/tblaccounts/csv")]
        [HttpGet("/export/AccountingDb/tblaccounts/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportTblAccountsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblAccounts(), Request.Query), fileName);
        }

        [HttpGet("/export/AccountingDb/tblaccounts/excel")]
        [HttpGet("/export/AccountingDb/tblaccounts/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportTblAccountsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblAccounts(), Request.Query), fileName);
        }
        [HttpGet("/export/AccountingDb/tbltransactions/csv")]
        [HttpGet("/export/AccountingDb/tbltransactions/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportTblTransactionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTblTransactions(), Request.Query), fileName);
        }

        [HttpGet("/export/AccountingDb/tbltransactions/excel")]
        [HttpGet("/export/AccountingDb/tbltransactions/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportTblTransactionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTblTransactions(), Request.Query), fileName);
        }
    }
}
