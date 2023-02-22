using System.ComponentModel.DataAnnotations;

namespace PkAPI.DTOs;

public class PutFirmDTO
{
    [StringLength(3000)]
    [Display(Name = "name")]
    public string? Name { get; set; }
    [StringLength(3, MinimumLength = 3)]
    [Display(Name = "Short name")]
    public string? ShortName { get; set; }
    [Display(Name = "image")]
    public IFormFile? Image { get; set; }
    [StringLength(3000)]
    [Display(Name = "English description")]
    public string? EnglishDescription { get; set; }
    [StringLength(3000)]
    [Display(Name = "Estonian description")]
    public string? EstonianDescription { get; set; }
    [StringLength(10)]
    public string? GridMapColumn { get; set; }
    [StringLength(10)]
    public string? GridMapRow { get; set; }
}
