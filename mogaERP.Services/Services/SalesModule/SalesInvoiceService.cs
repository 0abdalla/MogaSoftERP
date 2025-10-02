using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.SalesModule.SalesInvoices;
using mogaERP.Domain.Interfaces.SalesModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Services.Services.SalesModule
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SalesInvoiceService> _logger;

        public SalesInvoiceService(IUnitOfWork unitOfWork, ILogger<SalesInvoiceService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<SalesInvoiceResponse>> CreateAsync(SalesInvoiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var customerExists = await _unitOfWork.Repository<Customer>()
                    .AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

                if (!customerExists)
                    return ApiResponse<SalesInvoiceResponse>.Failure(AppErrors.NotFound);

                var invoice = new Invoice
                {
                    InvoiceNumber = await GenerateInvoiceNumber(cancellationToken),
                    Date = DateOnly.FromDateTime(request.InvoiceDate),
                    CustomerId = request.CustomerId,
                    QuotationId = request.QuotationId,
                    RevenueTypeId = request.RevenueTypeId,
                    TaxId = request.TaxId,
                    IsTaxIncluded = request.IsTaxIncluded,
                    Items = request.Items.Select(i => new InvoiceItem
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                await _unitOfWork.Repository<Invoice>().AddAsync(invoice, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var spec = new SalesInvoiceSpecification(invoice.Id);
                var created = await _unitOfWork.Repository<Invoice>().GetEntityWithSpecAsync(spec, cancellationToken);
                if (created == null)
                    return ApiResponse<SalesInvoiceResponse>.Failure(AppErrors.TransactionFailed);

                var response = MapToResponse(created);
                return ApiResponse<SalesInvoiceResponse>.Success(AppErrors.Success, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return ApiResponse<SalesInvoiceResponse>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<SalesInvoiceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new SalesInvoiceSpecification(id);
                var invoice = await _unitOfWork.Repository<Invoice>().GetEntityWithSpecAsync(spec, cancellationToken);

                if (invoice == null)
                    return ApiResponse<SalesInvoiceResponse>.Failure(AppErrors.NotFound);

                var response = MapToResponse(invoice);
                return ApiResponse<SalesInvoiceResponse>.Success(AppErrors.Success, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching invoice by Id {Id}", id);
                return ApiResponse<SalesInvoiceResponse>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<PagedResponse<SalesInvoiceResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var spec = new SalesInvoiceSpecification(request);
                var countSpec = new SalesInvoiceSpecification(request);
                countSpec.DisablePagination();

                var totalCount = await _unitOfWork.Repository<Invoice>()
                    .CountBySpecAsync(countSpec, cancellationToken);

                var invoicesQuery = _unitOfWork.Repository<Invoice>().Query(spec);

                var data = invoicesQuery.Select(i => new SalesInvoiceResponse
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    InvoiceDate = i.Date.ToDateTime(TimeOnly.MinValue),
                    CustomerId = i.CustomerId,
                    CustomerName = i.Customer != null ? i.Customer.Name : string.Empty,
                    QuotationId = i.QuotationId,
                    QuotationNumber = i.Quotation != null ? i.Quotation.QuotationNumber : null,
                    RevenueTypeId = i.RevenueTypeId,
                    RevenueTypeName = i.RevenueType != null ? i.RevenueType.NameAR : string.Empty,
                    TaxId = i.TaxId,
                    TaxName = i.Tax != null ? i.Tax.Name : null,
                    TaxPercentage = i.Tax != null ? i.Tax.Percentage : (decimal?)null,
                    IsTaxIncluded = i.IsTaxIncluded,
                    Items = i.Items.Select(it => new SalesInvoiceItemResponse
                    {
                        ItemId = it.ItemId,
                        ItemName = it.Item != null ? it.Item.Name : string.Empty,
                        Quantity = it.Quantity,
                        UnitPrice = it.UnitPrice,
                        LineTotal = it.Quantity * it.UnitPrice
                    }).ToList(),
                    TotalAmount = i.Items.Sum(it => it.Quantity * it.UnitPrice)
                }).ToList();

                var paged = new PagedResponse<SalesInvoiceResponse>(data, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SalesInvoiceResponse>>.Success(AppErrors.Success, paged);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching invoices");
                return ApiResponse<PagedResponse<SalesInvoiceResponse>>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(int id, SalesInvoiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Invoice>()
                    .Query(x => x.Id == id && !x.IsDeleted)
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existing == null)
                    return ApiResponse<string>.Failure(AppErrors.NotFound);

                existing.Date = DateOnly.FromDateTime(request.InvoiceDate);
                existing.CustomerId = request.CustomerId;
                existing.QuotationId = request.QuotationId;
                existing.RevenueTypeId = request.RevenueTypeId;
                existing.TaxId = request.TaxId;
                existing.IsTaxIncluded = request.IsTaxIncluded;

                existing.Items.Clear();
                foreach (var item in request.Items)
                {
                    existing.Items.Add(new InvoiceItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }

                _unitOfWork.Repository<Invoice>().Update(existing);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApiResponse<string>.Success(AppErrors.UpdateSuccess, existing.InvoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice {Id}", id);
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id, cancellationToken);
                if (existing == null)
                    return ApiResponse<string>.Failure(AppErrors.NotFound);

                existing.IsDeleted = true;
                _unitOfWork.Repository<Invoice>().Update(existing);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice {Id}", id);
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
            }
        }

        // Helpers
        private SalesInvoiceResponse MapToResponse(Invoice invoice)
        {
            return new SalesInvoiceResponse
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.Date.ToDateTime(TimeOnly.MinValue),
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer?.Name ?? string.Empty,
                QuotationId = invoice.QuotationId,
                QuotationNumber = invoice.Quotation?.QuotationNumber,
                RevenueTypeId = invoice.RevenueTypeId,
                RevenueTypeName = invoice.RevenueType?.NameAR ?? string.Empty,
                TaxId = invoice.TaxId,
                TaxName = invoice.Tax?.Name,
                TaxPercentage = invoice.Tax?.Percentage,
                IsTaxIncluded = invoice.IsTaxIncluded,
                Items = invoice.Items.Select(it => new SalesInvoiceItemResponse
                {
                    ItemId = it.ItemId,
                    ItemName = it.Item?.Name ?? string.Empty,
                    Quantity = it.Quantity,
                    UnitPrice = it.UnitPrice,
                    LineTotal = it.Quantity * it.UnitPrice
                }).ToList(),
                TotalAmount = invoice.Items.Sum(it => it.Quantity * it.UnitPrice)
            };
        }

        private async Task<string> GenerateInvoiceNumber(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year;
            var count = await _unitOfWork.Repository<Invoice>()
                .CountAsync(x => x.Date.Year == year, cancellationToken);

            return $"INV-{year}-{(count + 1):D5}";
        }
    }
}
