namespace Twitter.Data.Model;

public class Blocked : BaseModel
{
    public string ApplicationUserId { get; set; }

    public string ToUserId { get; set; }
}