namespace Project2UI.Models;

public class SaveImageRequest
{
    public  string UserId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string CapturedBy { get; set; }
    public DateTimeOffset CapturedDate { get; set; }
    public string Tags { get; set; }
    
}