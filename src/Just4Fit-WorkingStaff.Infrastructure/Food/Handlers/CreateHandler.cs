namespace Just4Fit_WorkingStaff.Infrastructure.Food.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Just4Fit_WorkingStaff.Core.Food.Repositories;
using Just4Fit_WorkingStaff.Infrastructure.Food.Commands;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand>
{
    private readonly IFoodRepository foodRepository;

    public CreateHandler(IFoodRepository foodRepository) => this.foodRepository = foodRepository;

    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Food);

        await this.foodRepository.CreateAsync(request.Food);
    }
}