using Project2UI.Models;
using RestSharp;

namespace Project2UI.Components;

public class UserComponent
{
    private readonly RestClient _client;
    private readonly IConfiguration _configuration;

    public UserComponent(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new RestClient(configuration.GetConnectionString("ProjectApi"));
    }
    
    
    public async Task<IRestResponse<Project2Response<SignInResponse>>> SignIn(SignInRequest signInRequest )
    {
        var request = new RestRequest("user/sign-in", Method.POST).AddJsonBody(signInRequest);

        var response = await _client.ExecuteAsync<Project2Response<SignInResponse>>(request);
           
        return response;
    }
    
    public async Task<IRestResponse<Project2Response<SignInResponse>>> SignUp(SignUpRequest signUpRequest )
    {
        var request = new RestRequest("user/sign-up", Method.POST).AddJsonBody(signUpRequest);

        var response = await _client.ExecuteAsync<Project2Response<SignInResponse>>(request);
           
        return response;
    }
}