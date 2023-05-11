using System.Security.Claims;
using Election2023.Application.Interfaces.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Election2023.ServerApp.Services
{
    // public class CurrentUserService : ICurrentUserService
    // {
    //     private readonly AuthenticationStateProvider _authenticationStateProvider;
    //     private readonly List<KeyValuePair<string, string>> _claims = new();
    //     private string? _userId;

    //     public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
    //     {
    //         _authenticationStateProvider = authenticationStateProvider;
    //         Task.Run(GetUserData).Wait();
    //     }

    //     private async Task GetUserData()
    //     {
    //         var user = (await _authenticationStateProvider.GetAuthenticationStateAsync())?.User;
    //         if (user?.Identity is not null && user.Identity.IsAuthenticated)
    //         {
    //             _userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    //             _claims.AddRange(user.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)));
    //         }
    //     }

    //     public string? UserId => _userId;

    //     public IReadOnlyCollection<KeyValuePair<string, string>>? Claims => _claims.AsReadOnly();

    // }

    public class CurrentUserService : ICurrentUserService
    {
        private ClaimsPrincipal currentUser = new(new ClaimsIdentity());

        public ClaimsPrincipal GetUser() => currentUser;

        public void SetUser(ClaimsPrincipal user)
        {
            if (currentUser != user)
                currentUser = user;
        }

        // internal void SetUser(ClaimsPrincipal user)
        // {
        //     if (currentUser != user)
        //         currentUser = user;
        // }

        public string UserId => currentUser.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Initaitor";

        public IReadOnlyCollection<KeyValuePair<string, string>> Claims 
            => currentUser.Claims.Select(p => new KeyValuePair<string, string>(p.Type, p.Value)).ToList().AsReadOnly();
    }

    internal sealed class UserCircuitHandler : CircuitHandler
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ICurrentUserService _currentUserService;

        public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider, ICurrentUserService currentUserService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _currentUserService = currentUserService;
        }

        public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            var user = (await _authenticationStateProvider.GetAuthenticationStateAsync())?.User;
            if (user?.Identity is not null && user.Identity.IsAuthenticated)
                _currentUserService.SetUser(user);
        }
    }
}

