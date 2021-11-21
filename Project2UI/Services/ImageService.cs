using System.Net;
using System.Net.Mime;
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

    public async Task<Project2Response<List<UserImage>>> GetImagesByUser(string userId)
    {
        var response = new Project2Response<List<UserImage>>();

        try
        {
            var restResponse = await _imageComponent.GetUserImageByUser(userId);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var idResponse =
                        JsonConvert.DeserializeObject<Project2Response<List<UserImage>>>(restResponse.Content);
                    if (idResponse is {Success: true})
                    {
                        response.Success = true;
                        response.Result = idResponse.Result;
                    }
                    else
                        response.Message = idResponse.Message;

                    break;
                default:
                    response.Message = "An error has occured getting user's images";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occured getting user's images";
        }

        return response;
    }

    public async Task<Project2Response> UpdateImage(Image image)
    {
        var response = new Project2Response();

        try
        {
            var restResponse = await _imageComponent.UpdateImage(image);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var idResponse =
                        JsonConvert.DeserializeObject<Project2Response>(restResponse.Content);
                    if (idResponse is {Success: true})
                    {
                        response.Success = true;
                    }
                    else
                        response.Message = idResponse.Message;

                    break;
                default:
                    response.Message = "An error has occured updating the image meta data";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occured updating the image meta data";
        }

        return response;
    }

    public async Task<Project2Response> DownloadImage(Image image)
    {
        return await _imageComponent.DownloadFile(image);
    }


    public async Task<Project2Response> DeleteImage(Image image)
    {
        var response = new Project2Response();

        try
        {
            var restResponse = await _imageComponent.DeleteImage(image.Id);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var idResponse =
                        JsonConvert.DeserializeObject<Project2Response>(restResponse.Content);
                    if (idResponse is {Success: true})
                    {
                        response.Success = true;
                        await _imageComponent.DeleteImageFromBlob(image.Id + "." + image.Type);
                    }
                    else
                        response.Message = idResponse.Message;

                    break;
                default:
                    response.Message = "An error has occured deleting the image";
                    break;
            }
        }
        catch (Exception ex)
        {
            response.Message = "An error has occured deleting the image";
        }

        return response;
    }
}