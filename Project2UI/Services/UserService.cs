using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Project2UI.Components;
using Project2UI.Models;

namespace Project2UI.Services;

public class UserService
{
    private readonly BrowserStorageService _browserStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly UserComponent _userComponent;

    public UserService(IConfiguration config,
        BrowserStorageService browserStorageService,
        AuthenticationStateProvider authenticationStateProvider,
        UserComponent userComponent)
    {
        _browserStorageService = browserStorageService;
        _authenticationStateProvider = authenticationStateProvider;
        _userComponent = userComponent;
    }

    public async Task<Project2Response> SignUp(SignUpRequest signUpRequest)
    {
        var response = new Project2Response();

        try
        {
            var restResponse = await _userComponent.SignUp(signUpRequest);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var authResponse =
                        JsonConvert.DeserializeObject<Project2Response<SignInResponse>>(restResponse.Content);
                    if (authResponse is {Success: true})
                    {
                        response.Success = true;
                        var userStateModel = new UserStateModel
                        {
                            UserId = authResponse.Result.Id
                        };
                            
                        await _browserStorageService.SetItem("Project2", userStateModel);

                    }
                    else
                        response.Message = authResponse.Message;
                    break;
                default:
                    response.Message = "An error has occurred while trying to sign up.";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occurred while trying to sign up.";
        }

        return response;
    }
    

    public async Task<Project2Response> SignIn(SignInRequest signInRequest)
    {
        var response = new Project2Response();

        try
        {
            var restResponse = await _userComponent.SignIn(signInRequest);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var authResponse =
                        JsonConvert.DeserializeObject<Project2Response<SignInResponse>>(restResponse.Content);
                    if (authResponse is {Success: true})
                    {
                        response.Success = true;
                        var userStateModel = new UserStateModel
                        {
                            UserId = authResponse.Result.Id
                        };
                            
                        await _browserStorageService.SetItem("Project2", userStateModel);

                    }
                    else
                        response.Message = authResponse.Message;
                    break;
                default:
                    response.Message = "An error has occurred while trying to sign in.";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occurred while trying to sign in.";
        }

        return response;
    }
}