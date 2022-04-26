namespace Twitter.Data.Model;

public class Follow : BaseModel
{
    public string ApplicationUserId { get; set; }

    public string ToUserId { get; set; }
}