using System.Security.Claims;
using Election2023.Application.Interfaces.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Election2023.ServerApp.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private ClaimsPrincipal currentUser = new(new ClaimsIdentity());

        public ClaimsPrincipal GetUser() => currentUser;

        public void SetUser(ClaimsPrincipal user)
        {
            if (currentUser != user)
                currentUser = user;
        }

        public string UserId => currentUser.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Initiator";

        public IReadOnlyCollection<KeyValuePair<string, string>> Claims 
            => currentUser.Claims.Select(p => new KeyValuePair<string, string>(p.Type, p.Value)).ToList().AsReadOnly();
    }

    internal sealed class UserCircuitHandler : CircuitHandler, IDisposable
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ICurrentUserService _currentUserService;

        public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider, ICurrentUserService currentUserService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _currentUserService = currentUserService;
        }

        private void AuthenticationChanged(Task<AuthenticationState> task)
        {
            _ = UpdateAuthentication(task);

            async Task UpdateAuthentication(Task<AuthenticationState> task)
            {
                try
                {
                    var state = await task;
                    _currentUserService.SetUser(state.User);
                }
                catch
                {
                }
            }
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _authenticationStateProvider.AuthenticationStateChanged += AuthenticationChanged;
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            var user = (await _authenticationStateProvider.GetAuthenticationStateAsync())?.User;
            if (user?.Identity is not null && user.Identity.IsAuthenticated)
                _currentUserService.SetUser(user);
        }

        public void Dispose() 
            => _authenticationStateProvider.AuthenticationStateChanged -= AuthenticationChanged;
    }
}

