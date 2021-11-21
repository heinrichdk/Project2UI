namespace Project2UI.Models;

public class Image
{
        public  string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateCaptured { get; set; }
        public string CapturedBy { get; set; }
        public string Tags { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public string Type { get; set; } = "";

}