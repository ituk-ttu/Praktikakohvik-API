namespace PkAPI.Models;

public class Firm
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public Image? Image { get; set; }    
    public string? EnglishDescription { get; set; }
    public string? EstonianDescription { get; set; }
    public string? GridMapColumn { get; set; }
    public string? GridMapRow { get; set; }
}
