using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Project2UI.Components;
using Project2UI.Models;

namespace Project2UI.Services;

public class ImageService
{
    private readonly ImageComponent _imageComponent;

    public ImageService(ImageComponent imageComponent)
    {
        _imageComponent = imageComponent;
    }
    
    public async Task<Project2Response> SaveImage(SaveImageRequest saveImageRequest, IBrowserFile file)
    {

        var response = new Project2Response();
        string imageId = "";

        try
        {
            var restResponse = await _imageComponent.SaveImage(saveImageRequest);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var idResponse =
                        JsonConvert.DeserializeObject<Project2Response<string>>(restResponse.Content);
                    if (idResponse is {Success: true})
                    {
                        response.Success = true;
                        imageId = idResponse.Result;
                        await _imageComponent.SaveImage(file, imageId);
                    }
                    else
                        response.Message = idResponse.Message;
                    break;
                default:
                    response.Message = "An error has occured saving the image";
                 break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occured saving the image";
        }

        return response;    
        
        
    }
}