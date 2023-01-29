using System.ComponentModel.DataAnnotations;

namespace PkAPI.DTOs;

public class PutFirmDTO
{
    [StringLength(100)]
    [Display(Name = "name")]
    public string? Name { get; set; }
    [Display(Name = "image")]
    public IFormFile? Image { get; set; }
    [StringLength(3000)]
    [Display(Name = "English description")]
    public string? EnglishDescription { get; set; }
    [StringLength(3000)]
    [Display(Name = "Estonian description")]
    public string? EstonianDescription { get; set; }
}
