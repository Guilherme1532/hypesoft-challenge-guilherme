using Hypesoft.Application.Commands.Categories;
using Hypesoft.Domain.Repositories;
using MediatR;

namespace Hypesoft.Application.Handlers.Categories;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var existing = await _repository.GetByIdAsync(request.Id, ct);
        if (existing == null)
            return false;
        
        await _repository.DeleteAsync(request.Id, ct);
        return true;
    }
}