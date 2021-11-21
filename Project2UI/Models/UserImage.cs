using System.Net.Mime;
using System.Text.Json.Serialization;

namespace Project2UI.Models;

public class UserImage
{

    public UserImage()
    {
        SmallResponses = new List<UserSmallResponse>();
    }
    public string ImageId { get; set; }
    public bool IsUploader { get; set; }
    public Image Image { get; set; }
    public User User { get; set; }
    [JsonIgnore] public bool IsExpanded { get; set; }

    [JsonIgnore]
    public string ImageSource =>
        "https://cmpg323project224323667.blob.core.windows.net/images/" + ImageId + "." + Image.Type;
    [JsonIgnore]
    public List<UserSmallResponse> SmallResponses { get; set; }
}