using Hypesoft.Application.Commands.Categories;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Categories;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _repository;

    public UpdateCategoryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var existing = await _repository.GetByIdAsync(request.Id, ct);
        if (existing == null)
            return false;
        
        existing.SetName(request.Name);
        
        await _repository.UpdateAsync(existing, ct);
        return true;
    }
}