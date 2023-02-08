using System.ComponentModel.DataAnnotations;

namespace PkAPI.DTOs;

public class PostFirmDTO
{
    [Required]
    [MaxLength(50)]
    [Display(Name = "name")]
    public string? Name { get; set; }
    [Required]
    [Display(Name = "image")]
    public IFormFile? Image { get; set; }
    [MaxLength(3000)]
    [Display(Name = "English description")]
    public string? EnglishDescription { get; set; }
    [MaxLength(3000)]
    [Display(Name = "Estonian description")]
    public string? EstonianDescription { get; set; }
}
