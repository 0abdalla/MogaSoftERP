using Microsoft.Extensions.Logging;
using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;
using mogaERP.Domain.Interfaces.SalesModule;

namespace mogaERP.Services.Services.SalesModule
{

    public class SalesQuotationService(IUnitOfWork unitOfWork, ILogger<SalesQuotation> logger) : ISalesQuotationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<SalesQuotation> _logger = logger;

        public async Task<ApiResponse<string>> CreateAsync(SalesQuotationRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                //var customerExists = await _unitOfWork.Repository<Customer>()
                //    .AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

                //if (!customerExists)
                //    return ApiResponse<string>.Failure(AppErrors.NotFound, "Customer not found!");

                var quotation = new SalesQuotation
                {
                    QuotationNumber = await GenerateQuotationNumber(cancellationToken),
                    Date = request.QuotationDate,
                    CustomerId = request.CustomerId,
                    Items = request.Items.Select(i => new QuotationItem
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                await _unitOfWork.Repository<SalesQuotation>().AddAsync(quotation, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApiResponse<string>.Success(AppErrors.AddSuccess, quotation.QuotationNumber);
            }
            catch (Exception)
            {
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(int id, SalesQuotationRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var existing = await _unitOfWork.Repository<SalesQuotation>()
                    .Query(x => x.Id == id && !x.IsDeleted)
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existing == null)
                    return ApiResponse<string>.Failure(AppErrors.NotFound);

                existing.Date = request.QuotationDate;
                existing.CustomerId = request.CustomerId;

                existing.Items.Clear();

                foreach (var item in request.Items)
                {
                    existing.Items.Add(new QuotationItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }

                _unitOfWork.Repository<SalesQuotation>().Update(existing);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApiResponse<string>.Success(AppErrors.UpdateSuccess, existing.QuotationNumber);
            }
            catch (Exception)
            {
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existing = await _unitOfWork.Repository<SalesQuotation>().GetByIdAsync(id, cancellationToken);
                if (existing == null)
                    return ApiResponse<string>.Failure(AppErrors.NotFound);

                existing.IsDeleted = true;
                _unitOfWork.Repository<SalesQuotation>().Update(existing);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
            }
        }

        public async Task<ApiResponse<PagedResponse<SalesQuotationResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var spec = new SalesQuotationSpecification(request);
                var countSpec = new SalesQuotationSpecification(request);
                countSpec.DisablePagination();

                var totalCount = await _unitOfWork.Repository<SalesQuotation>()
                    .CountBySpecAsync(countSpec, cancellationToken);

                var quotations = _unitOfWork.Repository<SalesQuotation>()
                    .Query(spec);

                var data = quotations.Select(q => new SalesQuotationResponse
                {
                    Id = q.Id,
                    QuotationNumber = q.QuotationNumber,
                    QuotationDate = q.Date,
                    CustomerId = q.CustomerId,
                    CustomerName = q.Customer != null ? q.Customer.Name : string.Empty,
                    Items = q.Items.Select(i => new SalesQuotationItemResponse
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item != null ? i.Item.Name : string.Empty,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()


                }).ToList();

                var pagedResponse = new PagedResponse<SalesQuotationResponse>(data, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<SalesQuotationResponse>>.Success(AppErrors.Success, pagedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales quotations");
                return ApiResponse<PagedResponse<SalesQuotationResponse>>.Failure(
                    AppErrors.TransactionFailed);
            }
        }
        public async Task<ApiResponse<SalesQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var spec = new SalesQuotationSpecification(id);

                var quotation = await _unitOfWork.Repository<SalesQuotation>()
                    .GetEntityWithSpecAsync(spec, cancellationToken);

                if (quotation == null)
                {
                    return ApiResponse<SalesQuotationResponse>.Failure(AppErrors.NotFound);
                }

                var response = new SalesQuotationResponse
                {
                    Id = quotation.Id,
                    QuotationNumber = quotation.QuotationNumber,
                    QuotationDate = quotation.Date,
                    CustomerId = quotation.CustomerId,
                    CustomerName = quotation.Customer != null ? quotation.Customer.Name : string.Empty,

                    Items = quotation.Items.Select(i => new SalesQuotationItemResponse
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item != null ? i.Item.Name : string.Empty,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                return ApiResponse<SalesQuotationResponse>.Success(AppErrors.Success, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching SalesQuotation by Id {Id}", id);
                return ApiResponse<SalesQuotationResponse>.Failure(AppErrors.TransactionFailed);
            }
        }


        private async Task<string> GenerateQuotationNumber(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year;
            var count = await _unitOfWork.Repository<SalesQuotation>()
                .CountAsync(x => x.Date.Year == year, cancellationToken);

            return $"SQ-{year}-{(count + 1):D5}";
        }
    }
}
