namespace AhlanFeekum.PropertyMedias
{
    public class PropertyMediaDto : PropertyMediaDtoBase
    {
        //Write your custom code here...
        public byte[] FileContent { get; set; }
        public string? OldFileName { get; set; }
        public bool FileNewUpload { get; set; } = false;
    }
}