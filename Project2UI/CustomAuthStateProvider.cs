using Microsoft.AspNetCore.Components.Authorization;
using Project2UI.Models;
using System.Security.Claims;

namespace Project2UI
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private bool IsAuthenticated { get; set; }
        private bool IsAuthenticating { get; set; }

        private readonly BrowserStorageService _browserStorageService;
        public CustomAuthStateProvider(BrowserStorageService browserStorageService)
        {
            _browserStorageService = browserStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;
     

            UserStateModel userState = await _browserStorageService.GetItem<UserStateModel>("Project2");
      

            if (IsAuthenticating)
            {
                return null;
            }
            else if (IsAuthenticated)
            {

                List<Claim> userClaims = new List<Claim>();

                userClaims.Add(new Claim(ClaimTypes.Name, userState.UserId));
       

                for (int i = 0; i < userState.claims.Count; i++)
                {

                    userClaims.Add(new Claim(ClaimTypes.Role, userState.claims[i]));
                }

                identity = new ClaimsIdentity(userClaims, "WebApiAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async void UpdateAuthentication(bool isAuthenticated)
        {
            await Task.Delay(200);

            IsAuthenticating = false;
            IsAuthenticated = isAuthenticated; //isAuthenticated.Value;


            NotifyAuthenticationStateChanged();
        }
    }
}
