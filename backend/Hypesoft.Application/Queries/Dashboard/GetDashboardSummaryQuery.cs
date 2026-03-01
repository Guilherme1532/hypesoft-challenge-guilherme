using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries.Dashboard;

public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;