using mogaERP.Domain.Contracts.SalesModule.SalesQuotation;
using mogaERP.Domain.Interfaces.SalesModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mogaERP.Services.Services.SalesModule
{

    public class SalesQuotationService(IUnitOfWork unitOfWork) : ISalesQuotationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
                var existing = await _unitOfWork.Repository<SalesQuotation>().GetByIdAsync(id, cancellationToken);
                if (existing == null)
                    return ApiResponse<string>.Failure(AppErrors.NotFound, "Quotation not found!");

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
                    return ApiResponse<string>.Failure(AppErrors.NotFound, "Quotation not found!");

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

        private async Task<string> GenerateQuotationNumber(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year;
            var count = await _unitOfWork.Repository<SalesQuotation>()
                .CountAsync(x => x.Date.Year == year, cancellationToken);

            return $"SQ-{year}-{(count + 1):D5}";
        }

      

        public Task<ApiResponse<List<SalesQuotationResponse>>> GetAllAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<SalesQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }        
    }
}
