using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Project2UI.Models;
using RestSharp;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Project2UI.Components;

public class ImageComponent
{
    private readonly RestClient _client;
    private readonly IConfiguration _configuration;

    public ImageComponent(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new RestClient(configuration.GetConnectionString("ProjectApi"));
    }

    public async Task<IRestResponse<Project2Response<string>>> SaveImage(SaveImageRequest saveImageRequest)
    {
        var request = new RestRequest("image/save", Method.POST).AddJsonBody(saveImageRequest);

        var response = await _client.ExecuteAsync<Project2Response<string>>(request);

        return response;
    }


    private Project2Response<BlobContainerClient> InitBlobContainer(string containerName)
    {
        var response = new Project2Response<BlobContainerClient>();

        try
        {
            var conn = _configuration.GetConnectionString("blobUrl");
            BlobServiceClient blobServiceClient = new BlobServiceClient(conn);
            var container = blobServiceClient.GetBlobContainerClient(containerName);
            if (container == null)
                response.Message = "Could not find Blob container";
            response.Result = container;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Message = "Error encounter with storage system";
        }

        return response;
    }

    public async Task<Project2Response> SaveImage(IBrowserFile file, string imageId)
    {
        var response = new Project2Response();
        try
        {
            var containerName = "images";
            var containerResponse = InitBlobContainer(containerName);
            if (containerResponse.Success)
            {
                var containerClient = containerResponse.Result;
                var type = file.ContentType.Split("/")[1];
                var blobResponse = await containerClient.UploadBlobAsync(
                    $"{imageId}.{type}", file.OpenReadStream());
                response.Success = true;
            }
            else
            {
                response.Message = containerResponse.Message;
            }
        }
        catch (Exception e)
        {
            response.Message = "Error encounter with storage system";
        }

        return response;
    }

    public async Task<Project2Response> DownloadFile(Image image)
    {
        var response = new Project2Response();
        try
        {
            var destFileName = $"{image.Name}.{image.Type}";
            var container = InitBlobContainer("images");
            var containerClient = container.Result;
            BlobClient blob = containerClient.GetBlobClient($"{image.Id}.{image.Type}");

            BlobDownloadInfo download = await blob.DownloadAsync();
            
            var path =  Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (FileStream downloadFileStream = File.OpenWrite(path +"\\"+destFileName))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }

            response.Success = true;
        }
        catch (Exception e)
        {
            response.Message = "Error encounter with storage system";
        }

        return response;
    }


    public async Task<Project2Response> DeleteImageFromBlob(string fileName)
    {
        var response = new Project2Response();
        try
        {
            var containerName = "images";
            var containerResponse = InitBlobContainer(containerName);
            if (containerResponse.Success)
            {
                var containerClient = containerResponse.Result;
                var blobResponse = await containerClient.DeleteBlobAsync(fileName);
                response.Success = true;
            }
            else
            {
                response.Message = containerResponse.Message;
            }
        }
        catch (Exception e)
        {
            response.Message = "Error encounter with storage system";
        }

        return response;
    }

    public async Task<IRestResponse<Project2Response>> UpdateImage(Image image)
    {
        var request = new RestRequest("image/update-image", Method.POST).AddJsonBody(image);

        var response = await _client.ExecuteAsync<Project2Response>(request);

        return response;
    }


    public async Task<IRestResponse<Project2Response<List<UserImage>>>> GetUserImageByUser(string userId)
    {
        var request = new RestRequest($"image/images-by-user/{userId}", Method.GET);

        var response = await _client.ExecuteAsync<Project2Response<List<UserImage>>>(request);

        return response;
    }

    public async Task<IRestResponse<Project2Response>> DeleteImage(string imageId)
    {
        var request = new RestRequest($"image/delete-image/{imageId}", Method.POST);

        var response = await _client.ExecuteAsync<Project2Response>(request);

        return response;
    }
}