using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Project2UI.Models;
using RestSharp;
using Azure.Storage.Blobs;

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
}