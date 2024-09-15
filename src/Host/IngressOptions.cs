namespace Host;

public record IngressOptions
{
    public const string Section = "IngressOptions";
    public string Uri { get; init; }
}