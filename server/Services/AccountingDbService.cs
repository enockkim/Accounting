using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Accounting.Data;

namespace Accounting
{
    public partial class AccountingDbService
    {
        AccountingDbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly AccountingDbContext context;
        private readonly NavigationManager navigationManager;

        public AccountingDbService(AccountingDbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportAccountTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/accounttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/accounttypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/accounttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/accounttypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountTypesRead(ref IQueryable<Models.AccountingDb.AccountType> items);

        public async Task<IQueryable<Models.AccountingDb.AccountType>> GetAccountTypes(Query query = null)
        {
            var items = Context.AccountTypes.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAccountTypesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountTypeCreated(Models.AccountingDb.AccountType item);
        partial void OnAfterAccountTypeCreated(Models.AccountingDb.AccountType item);

        public async Task<Models.AccountingDb.AccountType> CreateAccountType(Models.AccountingDb.AccountType accountType)
        {
            OnAccountTypeCreated(accountType);

            var existingItem = Context.AccountTypes
                              .Where(i => i.account_type_id == accountType.account_type_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AccountTypes.Add(accountType);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(accountType).State = EntityState.Detached;
                throw;
            }

            OnAfterAccountTypeCreated(accountType);

            return accountType;
        }
        public async Task ExportCustomersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCustomersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCustomersRead(ref IQueryable<Models.AccountingDb.Customer> items);

        public async Task<IQueryable<Models.AccountingDb.Customer>> GetCustomers(Query query = null)
        {
            var items = Context.Customers.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCustomersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCustomerCreated(Models.AccountingDb.Customer item);
        partial void OnAfterCustomerCreated(Models.AccountingDb.Customer item);

        public async Task<Models.AccountingDb.Customer> CreateCustomer(Models.AccountingDb.Customer customer)
        {
            OnCustomerCreated(customer);

            var existingItem = Context.Customers
                              .Where(i => i.customersID == customer.customersID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Customers.Add(customer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(customer).State = EntityState.Detached;
                throw;
            }

            OnAfterCustomerCreated(customer);

            return customer;
        }
        public async Task ExportSuppliersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/suppliers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/suppliers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSuppliersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/suppliers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/suppliers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSuppliersRead(ref IQueryable<Models.AccountingDb.Supplier> items);

        public async Task<IQueryable<Models.AccountingDb.Supplier>> GetSuppliers(Query query = null)
        {
            var items = Context.Suppliers.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSuppliersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSupplierCreated(Models.AccountingDb.Supplier item);
        partial void OnAfterSupplierCreated(Models.AccountingDb.Supplier item);

        public async Task<Models.AccountingDb.Supplier> CreateSupplier(Models.AccountingDb.Supplier supplier)
        {
            OnSupplierCreated(supplier);

            var existingItem = Context.Suppliers
                              .Where(i => i.suppliersID == supplier.suppliersID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Suppliers.Add(supplier);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(supplier).State = EntityState.Detached;
                throw;
            }

            OnAfterSupplierCreated(supplier);

            return supplier;
        }
        public async Task ExportTblAccountsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/tblaccounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/tblaccounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblAccountsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/tblaccounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/tblaccounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblAccountsRead(ref IQueryable<Models.AccountingDb.TblAccount> items);

        public async Task<IQueryable<Models.AccountingDb.TblAccount>> GetTblAccounts(Query query = null)
        {
            var items = Context.TblAccounts.AsQueryable();

            items = items.Include(i => i.AccountType);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTblAccountsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblAccountCreated(Models.AccountingDb.TblAccount item);
        partial void OnAfterTblAccountCreated(Models.AccountingDb.TblAccount item);

        public async Task<Models.AccountingDb.TblAccount> CreateTblAccount(Models.AccountingDb.TblAccount tblAccount)
        {
            OnTblAccountCreated(tblAccount);

            var existingItem = Context.TblAccounts
                              .Where(i => i.gl_account_no == tblAccount.gl_account_no)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblAccounts.Add(tblAccount);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblAccount).State = EntityState.Detached;
                tblAccount.AccountType = null;
                throw;
            }

            OnAfterTblAccountCreated(tblAccount);

            return tblAccount;
        }
        public async Task ExportTblTransactionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/tbltransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/tbltransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTblTransactionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accountingdb/tbltransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accountingdb/tbltransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTblTransactionsRead(ref IQueryable<Models.AccountingDb.TblTransaction> items);

        public async Task<IQueryable<Models.AccountingDb.TblTransaction>> GetTblTransactions(Query query = null)
        {
            var items = Context.TblTransactions.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTblTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTblTransactionCreated(Models.AccountingDb.TblTransaction item);
        partial void OnAfterTblTransactionCreated(Models.AccountingDb.TblTransaction item);

        public async Task<Models.AccountingDb.TblTransaction> CreateTblTransaction(Models.AccountingDb.TblTransaction tblTransaction)
        {
            OnTblTransactionCreated(tblTransaction);

            var existingItem = Context.TblTransactions
                              .Where(i => i.transaction_id == tblTransaction.transaction_id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TblTransactions.Add(tblTransaction);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tblTransaction).State = EntityState.Detached;
                throw;
            }

            OnAfterTblTransactionCreated(tblTransaction);

            return tblTransaction;
        }

        partial void OnAccountTypeDeleted(Models.AccountingDb.AccountType item);
        partial void OnAfterAccountTypeDeleted(Models.AccountingDb.AccountType item);

        public async Task<Models.AccountingDb.AccountType> DeleteAccountType(int? accountTypeId)
        {
            var itemToDelete = Context.AccountTypes
                              .Where(i => i.account_type_id == accountTypeId)
                              .Include(i => i.TblAccounts)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountTypeDeleted(itemToDelete);

            Context.AccountTypes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountTypeDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnAccountTypeGet(Models.AccountingDb.AccountType item);

        public async Task<Models.AccountingDb.AccountType> GetAccountTypeByaccountTypeId(int? accountTypeId)
        {
            var items = Context.AccountTypes
                              .AsNoTracking()
                              .Where(i => i.account_type_id == accountTypeId);

            var itemToReturn = items.FirstOrDefault();

            OnAccountTypeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AccountingDb.AccountType> CancelAccountTypeChanges(Models.AccountingDb.AccountType item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAccountTypeUpdated(Models.AccountingDb.AccountType item);
        partial void OnAfterAccountTypeUpdated(Models.AccountingDb.AccountType item);

        public async Task<Models.AccountingDb.AccountType> UpdateAccountType(int? accountTypeId, Models.AccountingDb.AccountType accountType)
        {
            OnAccountTypeUpdated(accountType);

            var itemToUpdate = Context.AccountTypes
                              .Where(i => i.account_type_id == accountTypeId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(accountType);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterAccountTypeUpdated(accountType);

            return accountType;
        }

        partial void OnCustomerDeleted(Models.AccountingDb.Customer item);
        partial void OnAfterCustomerDeleted(Models.AccountingDb.Customer item);

        public async Task<Models.AccountingDb.Customer> DeleteCustomer(int? customersId)
        {
            var itemToDelete = Context.Customers
                              .Where(i => i.customersID == customersId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCustomerDeleted(itemToDelete);

            Context.Customers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCustomerDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCustomerGet(Models.AccountingDb.Customer item);

        public async Task<Models.AccountingDb.Customer> GetCustomerBycustomersId(int? customersId)
        {
            var items = Context.Customers
                              .AsNoTracking()
                              .Where(i => i.customersID == customersId);

            var itemToReturn = items.FirstOrDefault();

            OnCustomerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AccountingDb.Customer> CancelCustomerChanges(Models.AccountingDb.Customer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCustomerUpdated(Models.AccountingDb.Customer item);
        partial void OnAfterCustomerUpdated(Models.AccountingDb.Customer item);

        public async Task<Models.AccountingDb.Customer> UpdateCustomer(int? customersId, Models.AccountingDb.Customer customer)
        {
            OnCustomerUpdated(customer);

            var itemToUpdate = Context.Customers
                              .Where(i => i.customersID == customersId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(customer);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCustomerUpdated(customer);

            return customer;
        }

        partial void OnSupplierDeleted(Models.AccountingDb.Supplier item);
        partial void OnAfterSupplierDeleted(Models.AccountingDb.Supplier item);

        public async Task<Models.AccountingDb.Supplier> DeleteSupplier(int? suppliersId)
        {
            var itemToDelete = Context.Suppliers
                              .Where(i => i.suppliersID == suppliersId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSupplierDeleted(itemToDelete);

            Context.Suppliers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSupplierDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnSupplierGet(Models.AccountingDb.Supplier item);

        public async Task<Models.AccountingDb.Supplier> GetSupplierBysuppliersId(int? suppliersId)
        {
            var items = Context.Suppliers
                              .AsNoTracking()
                              .Where(i => i.suppliersID == suppliersId);

            var itemToReturn = items.FirstOrDefault();

            OnSupplierGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AccountingDb.Supplier> CancelSupplierChanges(Models.AccountingDb.Supplier item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSupplierUpdated(Models.AccountingDb.Supplier item);
        partial void OnAfterSupplierUpdated(Models.AccountingDb.Supplier item);

        public async Task<Models.AccountingDb.Supplier> UpdateSupplier(int? suppliersId, Models.AccountingDb.Supplier supplier)
        {
            OnSupplierUpdated(supplier);

            var itemToUpdate = Context.Suppliers
                              .Where(i => i.suppliersID == suppliersId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(supplier);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterSupplierUpdated(supplier);

            return supplier;
        }

        partial void OnTblAccountDeleted(Models.AccountingDb.TblAccount item);
        partial void OnAfterTblAccountDeleted(Models.AccountingDb.TblAccount item);

        public async Task<Models.AccountingDb.TblAccount> DeleteTblAccount(int? glAccountNo)
        {
            var itemToDelete = Context.TblAccounts
                              .Where(i => i.gl_account_no == glAccountNo)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblAccountDeleted(itemToDelete);

            Context.TblAccounts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblAccountDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnTblAccountGet(Models.AccountingDb.TblAccount item);

        public async Task<Models.AccountingDb.TblAccount> GetTblAccountByglAccountNo(int? glAccountNo)
        {
            var items = Context.TblAccounts
                              .AsNoTracking()
                              .Where(i => i.gl_account_no == glAccountNo);

            items = items.Include(i => i.AccountType);

            var itemToReturn = items.FirstOrDefault();

            OnTblAccountGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AccountingDb.TblAccount> CancelTblAccountChanges(Models.AccountingDb.TblAccount item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblAccountUpdated(Models.AccountingDb.TblAccount item);
        partial void OnAfterTblAccountUpdated(Models.AccountingDb.TblAccount item);

        public async Task<Models.AccountingDb.TblAccount> UpdateTblAccount(int? glAccountNo, Models.AccountingDb.TblAccount tblAccount)
        {
            OnTblAccountUpdated(tblAccount);

            var itemToUpdate = Context.TblAccounts
                              .Where(i => i.gl_account_no == glAccountNo)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblAccount);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterTblAccountUpdated(tblAccount);

            return tblAccount;
        }

        partial void OnTblTransactionDeleted(Models.AccountingDb.TblTransaction item);
        partial void OnAfterTblTransactionDeleted(Models.AccountingDb.TblTransaction item);

        public async Task<Models.AccountingDb.TblTransaction> DeleteTblTransaction(int? transactionId)
        {
            var itemToDelete = Context.TblTransactions
                              .Where(i => i.transaction_id == transactionId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTblTransactionDeleted(itemToDelete);

            Context.TblTransactions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTblTransactionDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnTblTransactionGet(Models.AccountingDb.TblTransaction item);

        public async Task<Models.AccountingDb.TblTransaction> GetTblTransactionBytransactionId(int? transactionId)
        {
            var items = Context.TblTransactions
                              .AsNoTracking()
                              .Where(i => i.transaction_id == transactionId);

            var itemToReturn = items.FirstOrDefault();

            OnTblTransactionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AccountingDb.TblTransaction> CancelTblTransactionChanges(Models.AccountingDb.TblTransaction item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTblTransactionUpdated(Models.AccountingDb.TblTransaction item);
        partial void OnAfterTblTransactionUpdated(Models.AccountingDb.TblTransaction item);

        public async Task<Models.AccountingDb.TblTransaction> UpdateTblTransaction(int? transactionId, Models.AccountingDb.TblTransaction tblTransaction)
        {
            OnTblTransactionUpdated(tblTransaction);

            var itemToUpdate = Context.TblTransactions
                              .Where(i => i.transaction_id == transactionId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tblTransaction);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterTblTransactionUpdated(tblTransaction);

            return tblTransaction;
        }
    }
}
