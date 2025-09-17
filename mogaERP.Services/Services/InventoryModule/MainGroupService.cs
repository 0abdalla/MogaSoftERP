using mogaERP.Domain.Contracts.InventoryModule.Items;
using mogaERP.Domain.Interfaces.InventoryModule;

namespace mogaERP.Services.Services.InventoryModule;
public class MainGroupService(IUnitOfWork unitOfWork) : IMainGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApiResponse<string>> CreateMainGroupAsync(MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var mainGroup = new MainGroup
            {
                Name = request.Name,
            };

            await _unitOfWork.Repository<MainGroup>().AddAsync(mainGroup, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.AddSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<string>> DeleteMainGroupAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var mainGroup = await _unitOfWork.Repository<Domain.Entities.MainGroup>().GetByIdAsync(id, cancellationToken);
            if (mainGroup == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);
            mainGroup.IsDeleted = true;
            _unitOfWork.Repository<MainGroup>().Update(mainGroup);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ApiResponse<string>.Success(AppErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }

    public async Task<ApiResponse<IReadOnlyList<MainGroupResponse>>> GetAllMainGroupsAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<MainGroup>()
         .Query(mg => !mg.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(mg => mg.Name.ToLower().Contains(search));
        }

        var mainGroups = await query
            .Select(mg => new MainGroupResponse
            {
                Id = mg.Id,
                Name = mg.Name,
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<MainGroupResponse>>.Success(AppErrors.Success, mainGroups);

    }

    public async Task<ApiResponse<MainGroupResponse>> GetMainGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var mainGroup = await _unitOfWork.Repository<MainGroup>()
            .Query(mg => mg.Id == id && !mg.IsDeleted)
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Select(mg => new MainGroupResponse
            {
                Id = mg.Id,
                Name = mg.Name,

                CreatedBy = mg.CreatedBy.UserName ?? string.Empty,
                UpdatedBy = mg.UpdatedBy.UserName ?? string.Empty,
                CreatedById = mg.CreatedById,

                CreatedOn = mg.CreatedOn,
                UpdatedOn = mg.UpdatedOn,
                UpdatedById = mg.UpdatedById,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (mainGroup == null)
            return ApiResponse<MainGroupResponse>.Failure(AppErrors.NotFound);

        return ApiResponse<MainGroupResponse>.Success(AppErrors.Success, mainGroup);

    }

    public async Task<ApiResponse<string>> UpdateMainGroupAsync(int id, MainGroupRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var mainGroup = await _unitOfWork.Repository<Domain.Entities.MainGroup>().GetByIdAsync(id, cancellationToken);
            if (mainGroup == null)
                return ApiResponse<string>.Failure(AppErrors.NotFound);

            mainGroup.Name = request.Name;

            _unitOfWork.Repository<MainGroup>().Update(mainGroup);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<string>.Success(AppErrors.UpdateSuccess);
        }
        catch (Exception)
        {
            return ApiResponse<string>.Failure(AppErrors.TransactionFailed);
        }

    }
}
