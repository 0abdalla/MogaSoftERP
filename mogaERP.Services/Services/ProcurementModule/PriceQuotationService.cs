using mogaERP.Domain.Contracts.ProcurementModule.PriceQuotation;
using mogaERP.Domain.Interfaces.ProcurementModule;

namespace mogaERP.Services.Services.ProcurementModule;
public class PriceQuotationService(IUnitOfWork unitOfWork) : IPriceQuotationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreatePriceQuotationAsync(PriceQuotationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchaseRequestExists = await _unitOfWork.Repository<PurchaseRequest>()
                                         .Query(pr => pr.Id == request.PurchaseRequestId && !pr.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            if (purchaseRequestExists == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "طلب الشراء غير موجود");
            else if (purchaseRequestExists != null && purchaseRequestExists.Status != PurchaseStatus.Approved)
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed, "طلب الشراء غير مأكد");

            var quotation = new PriceQuotation
            {
                QuotationNumber = await GenerateQuotationNumber(cancellationToken),
                QuotationDate = request.QuotationDate,
                SupplierId = request.SupplierId,
                Notes = request.Notes,
                Status = QuotationStatus.Pending,
                PurchaseRequestId = request.PurchaseRequestId,
                Items = request.Items.Select(i => new PriceQuotationItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Notes = i.Notes,

                }).ToList()
            };


            await _unitOfWork.Repository<PriceQuotation>().AddAsync(quotation, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess, quotation.Id.ToString());

        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }
    public async Task<ApiResponse<string>> DeletePriceQuotationAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var priceQuotation = await _unitOfWork.Repository<PriceQuotation>().GetByIdAsync(id, cancellationToken);

            if (priceQuotation == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            priceQuotation.IsDeleted = true;
            _unitOfWork.Repository<PriceQuotation>().Update(priceQuotation);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PagedResponse<PriceQuotationResponse>>> GetAllPriceQuotationsAsync(SearchRequest searchRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var spec = new PriceQuotationSpecification(searchRequest);

            var query = _unitOfWork.Repository<PriceQuotation>().Query(spec);

            var countSpec = new PriceQuotationSpecification(searchRequest);
            countSpec.DisablePagination(); // To get the total count without pagination
            var totalCount = await _unitOfWork.Repository<PriceQuotation>().CountBySpecAsync(countSpec, cancellationToken);


            var quotations = await query
                 .Select(x => new PriceQuotationResponse
                 {
                     Id = x.Id,
                     QuotationNumber = x.QuotationNumber,
                     QuotationDate = x.QuotationDate,
                     SupplierId = x.SupplierId,
                     SupplierName = x.Supplier.Name,
                     Notes = x.Notes,
                     Status = x.Status.ToString(),
                     TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                     PurchaseRequestId = x.PurchaseRequestId,
                     PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                     Items = x.Items.Select(i => new PriceQuotationItemResponse
                     {
                         ItemId = i.ItemId,
                         Name = i.Item.Name,
                         Quantity = i.Quantity,
                         UnitPrice = i.UnitPrice,
                         Total = i.Quantity * i.UnitPrice,
                         Notes = i.Notes,
                         Unit = i.Item.Unit.Name ?? "",
                     }).ToList()
                 })
                 .ToListAsync(cancellationToken);

            var pagedResponse = new PagedResponse<PriceQuotationResponse>(
                quotations, totalCount, searchRequest.PageNumber, searchRequest.PageSize);

            return ApiResponse<PagedResponse<PriceQuotationResponse>>.Success(AppErrors.Success, pagedResponse);
        }
        catch (Exception)
        {
            return ApiResponse<PagedResponse<PriceQuotationResponse>>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<PriceQuotationResponse>> GetPriceQuotationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new PriceQuotationSpecification(id);

        var quotation = await _unitOfWork.Repository<PriceQuotation>().Query(spec)
             .Select(x => new PriceQuotationResponse
             {
                 Id = x.Id,
                 QuotationNumber = x.QuotationNumber,
                 QuotationDate = x.QuotationDate,
                 SupplierId = x.SupplierId,
                 SupplierName = x.Supplier.Name,
                 Notes = x.Notes,
                 Status = x.Status.ToString(),
                 TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                 PurchaseRequestId = x.PurchaseRequestId,
                 PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                 Items = x.Items.Select(i => new PriceQuotationItemResponse
                 {
                     ItemId = i.ItemId,
                     Name = i.Item.Name,
                     Quantity = i.Quantity,
                     UnitPrice = i.UnitPrice,
                     Total = i.Quantity * i.UnitPrice,
                     Notes = i.Notes,
                     Unit = i.Item.Unit.Name ?? "",

                 }).ToList(),

                 CreatedBy = x.CreatedBy.UserName,
                 CreatedById = x.CreatedById,
                 CreatedOn = x.CreatedOn,
                 UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : null,
                 UpdatedById = x.UpdatedById,
                 UpdatedOn = x.UpdatedOn,
             })
             .FirstOrDefaultAsync(cancellationToken);

        if (quotation == null)
            return ApiResponse<PriceQuotationResponse>.Failure(AppErrors.NotFound);

        return ApiResponse<PriceQuotationResponse>.Success(AppErrors.Success, quotation);

    }

    public async Task<ApiResponse<string>> UpdatePriceQuotationAsync(int id, PriceQuotationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingPriceQuotation = await _unitOfWork.Repository<PriceQuotation>()
                .Query(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingPriceQuotation == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "عرض السعر غير موجود");

            var purchaseRequestExists = await _unitOfWork.Repository<PurchaseRequest>()
                .Query(pr => pr.Id == request.PurchaseRequestId && !pr.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchaseRequestExists == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound, "طلب الشراء غير موجود");
            else if (purchaseRequestExists.Status != PurchaseStatus.Approved)
                return ApiResponse<string>.Failure(AppErrors.TransactionFailed, "طلب الشراء غير مأكد");

            existingPriceQuotation.QuotationDate = request.QuotationDate;
            existingPriceQuotation.SupplierId = request.SupplierId;
            existingPriceQuotation.Notes = request.Notes;
            existingPriceQuotation.Status = QuotationStatus.Pending;
            existingPriceQuotation.PurchaseRequestId = request.PurchaseRequestId;

            existingPriceQuotation.Items.Clear();
            foreach (var item in request.Items)
            {
                existingPriceQuotation.Items.Add(new PriceQuotationItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Notes = item.Notes
                });
            }

            _unitOfWork.Repository<PriceQuotation>().Update(existingPriceQuotation);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess, existingPriceQuotation.Id.ToString());
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<string>> SubmitPriceQuotationByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotations = await _unitOfWork.Repository<PriceQuotation>()
                .Query(x => !x.IsDeleted && x.PurchaseRequestId == purchaseRequestId)
                .Include(x => x.Items)
                .ToListAsync(cancellationToken);

            if (quotations.Count == 0)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            var quotationsWithTotal = quotations
                .Select(q => new
                {
                    Quotation = q,
                    TotalAmount = q.Items.Sum(i => i.Quantity * i.UnitPrice)
                })
                .ToList();

            var minQuotation = quotationsWithTotal
                .OrderBy(q => q.TotalAmount)
                .First();

            foreach (var q in quotationsWithTotal)
            {
                q.Quotation.Status = q == minQuotation ? QuotationStatus.Approved : QuotationStatus.Rejected;
                _unitOfWork.Repository<PriceQuotation>().Update(q.Quotation);
            }

            // Update PurchaseRequest with selected PriceQuotationId
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .Query(x => x.Id == purchaseRequestId)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchaseRequest != null)
            {
                purchaseRequest.AwardedQuotationId = minQuotation.Quotation.Id;
                _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.Success, "تم التحديث بنجاح");
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<IReadOnlyList<PriceQuotationResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default)
    {

        try
        {
            var query = _unitOfWork.Repository<PriceQuotation>()
                  .Query(x => !x.IsDeleted && x.Status == QuotationStatus.Approved)
                  .Include(x => x.Supplier)
                  .Include(x => x.Items)
                  .Include(x => x.PurchaseRequest);

            //var totalCount = await query.CountAsync(cancellationToken);

            var quotations = await query
                  .OrderByDescending(x => x.QuotationDate)
                  .Select(x => new PriceQuotationResponse
                  {
                      Id = x.Id,
                      QuotationNumber = x.QuotationNumber,
                      QuotationDate = x.QuotationDate,
                      SupplierId = x.SupplierId,
                      SupplierName = x.Supplier.Name,
                      Notes = x.Notes,
                      Status = x.Status.ToString(),
                      TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                      PurchaseRequestId = x.PurchaseRequestId,
                      PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                      Items = x.Items.Select(i => new PriceQuotationItemResponse
                      {
                          ItemId = i.Id,
                          Name = i.Item.Name,
                          Quantity = i.Quantity,
                          UnitPrice = i.UnitPrice,
                          Total = i.Quantity * i.UnitPrice,
                          Notes = i.Notes
                      }).ToList()
                  })
                  .ToListAsync(cancellationToken);

            return ApiResponse<IReadOnlyList<PriceQuotationResponse>>.Success(AppErrors.Success, quotations);

        }
        catch (Exception)
        {
            return ApiResponse<IReadOnlyList<PriceQuotationResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }

    public async Task<ApiResponse<IReadOnlyList<PriceQuotationResponse>>> GetAllByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<PriceQuotation>()
                .Query(x => !x.IsDeleted && x.PurchaseRequestId == purchaseRequestId)
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                    .ThenInclude(i => i.Item)
                        .ThenInclude(i => i.Unit)
                .Include(x => x.PurchaseRequest);


            //var totalCount = await query.CountAsync(cancellationToken);

            var quotations = await query
                .OrderByDescending(x => x.Id)
                .Select(x => new PriceQuotationResponse
                {
                    Id = x.Id,
                    QuotationNumber = x.QuotationNumber,
                    QuotationDate = x.QuotationDate,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    PurchaseRequestId = x.PurchaseRequestId,
                    PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                    Items = x.Items.Select(i => new PriceQuotationItemResponse
                    {
                        ItemId = i.ItemId,
                        Name = i.Item.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.Quantity * i.UnitPrice,
                        Notes = i.Notes,
                        Unit = i.Item.Unit.Name
                    }).ToList()

                }).ToListAsync(cancellationToken);


            return ApiResponse<IReadOnlyList<PriceQuotationResponse>>.Success(AppErrors.Success, quotations);
        }
        catch (Exception)
        {
            return ApiResponse<IReadOnlyList<PriceQuotationResponse>>.Failure(AppErrors.TransactionFailed);
        }
    }


    private async Task<string> GenerateQuotationNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PriceQuotation>()
            .CountAsync(x => x.QuotationDate.Year == year, cancellationToken);
        return $"PQ-{year}-{(count + 1):D5}";
    }

}
