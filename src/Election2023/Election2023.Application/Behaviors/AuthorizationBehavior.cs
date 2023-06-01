using System.Reflection;
using Election2023.Application.Exceptions;
using Election2023.Application.Interfaces.Services;

namespace Election2023.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehavior(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        const string notAuthorized = "You are not authorized to access this resource.";

        var authorizationAttributes = request.GetType().GetCustomAttributes<Security.AuthorizeAttribute>();

        if (authorizationAttributes.Any())
        {
            // Must be authenticated user
            var userId = _currentUserService.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException();

            // Default Role-based authorization
            var attrWithroles = authorizationAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
            if (attrWithroles.Any())
            {
                foreach(var roles in attrWithroles.Select(a => a.Roles.Split(',')))
                {
                    var authorized = false;
                    foreach(var role in roles)
                    {
                        var IsInRole = _currentUserService.GetUser().IsInRole(role.Trim());
                        if (IsInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                    
                    // User must have at least one role
                    if (!authorized)
                        throw new ForbiddenAccessException(notAuthorized);
                }
            }

            // Policy-based authorization
            var attrWithPolicies = authorizationAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
            if (attrWithPolicies.Any())
            {
                foreach(var policy in attrWithPolicies.Select(a => a.Policy))
                {
                    var authorized = false; //TODO: check if user has policy
                    if (!authorized)
                        throw new ForbiddenAccessException(notAuthorized);
                }
            }
        }

        // User is authorized or authorization not required
        return await next().ConfigureAwait(false);
    }
}