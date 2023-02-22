using PkAPI.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkAPI.Services.Firebase;
using PkAPI.Services.Firms;
using PkAPI.Models;
using PkAPI.DTOs;

namespace PkAPI.Controllers;

[ApiController]
[Route("api/firms")]
public class FirmsController : ControllerBase
{
    private IFirmMapper _mapper;
    private long _allowedFileSizeInBytes;
    private IFirebaseService _firebaseService;
    private IAuthorizationService _authService;
    public FirmsController(
        IFirmMapper mapper,
        IFirebaseService firebase,
        IAuthorizationService auth) 
    {
        _mapper = mapper;
        _authService = auth;
        _firebaseService = firebase;
        _allowedFileSizeInBytes = 10485760;
    }

    [HttpGet]
    public ActionResult GetFirms()
    {
        var firms = _firebaseService.GetFirms();

        if (firms == null || firms.Count == 0) return Ok(Array.Empty<GetFirmsDTO>());

        var firmDTOs = firms.Where(x => x != null).Select(x => _mapper.MapModelToGetDTO(x!));
        var dto = new GetDTO() 
        {
            DisplayMap = _firebaseService.GetMap()?.Status ?? false,
            Firms = firmDTOs.OrderBy(f => f.Name).ToList()
        };
        return Ok(dto);
    }

    [HttpGet("{Id}")]
    public ActionResult GetFirm(string Id)
    {
        var firm = _firebaseService.GetFirm(Id);

        if (firm == null)
        {
            return NotFound();
        }

        var firmDTO = _mapper.MapModelToGetDTO(firm);
        return Ok(firmDTO);
    }

    [HttpPost]
    public ActionResult PostFirm([FromForm] PostFirmDTO newFirm)
    {
        if (!_authService.IsAuthorized(HttpContext.Request.Headers)) 
        {
            return Unauthorized();
        }

        var firms = _firebaseService.GetFirms();
        if (firms.Count() >= 50)
        {
            ModelState.AddModelError("Firms", "The maximum number of firms has been reached.");
        }
        validateName(newFirm.Name!, firms);
        validateImage(newFirm.Image!);
        validateDescription(newFirm.EnglishDescription, newFirm.EstonianDescription);
        validatePlacement(newFirm.GridMapRow, newFirm.GridMapColumn, firms);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var firm = _mapper.MapPostDTOToNewModel(newFirm);
        var id = _firebaseService.PostFirm(firm);

        return GetFirm(id);
    }

    [HttpPut("{Id}")]
    public ActionResult UpdateFirm([FromRoute] string Id, [FromForm] PutFirmDTO formFirm)
    {
        if (!_authService.IsAuthorized(HttpContext.Request.Headers)) 
        {
            return Unauthorized();
        }

        var oldFirm = _firebaseService.GetFirm(Id);
        if (oldFirm == null)
        {
            return NotFound();
        }

        var firms = _firebaseService.GetFirms();

        if (formFirm.Name != null && formFirm.Name != oldFirm.Name)
        {
            validateName(formFirm.Name, firms);
        }
        if (formFirm.Image != null)
        {
            validateImage(formFirm.Image);
        }
        if (formFirm.GridMapColumn != null 
            && formFirm.GridMapRow != null 
            && oldFirm.GridMapColumn != formFirm.GridMapColumn 
            && oldFirm.GridMapRow != formFirm.GridMapRow)
        {
            validatePlacement(formFirm.GridMapRow!, formFirm.GridMapColumn!, firms);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedFirm = _mapper.MapPutDTOToOldModel(oldFirm, formFirm);
        _firebaseService.PutFirm(Id, updatedFirm);

        return GetFirm(Id);
    }

    [HttpDelete("{Id}")]
    public ActionResult DeleteFirm(string Id)
    {
        if (!_authService.IsAuthorized(HttpContext.Request.Headers)) 
        {
            return Unauthorized();
        }

        var firm = _firebaseService.GetFirm(Id);
        if (firm == null)
        {
            return NotFound();
        }

        _firebaseService.DeleteFirm(Id);

        return GetFirms();
    }

    private void validateImage(IFormFile file)
    {
        var fileType = Path.GetExtension(file.FileName);
        if (fileType != ".png" && fileType != ".jpg" && fileType != ".svg") 
        {
            ModelState.AddModelError("Image", "Invalid file type. Only .png, .jpg and .svg are allowed.");
        }
        else if (file.Length > 0 && _allowedFileSizeInBytes < file.Length)
        {
            var allowedFileSizeInMegabytes = Math.Round((double)_allowedFileSizeInBytes / 1024 / 1024);
            ModelState.AddModelError("Image", 
                $"File size exceeds the maximum allowed file size of {allowedFileSizeInMegabytes} MB.");
        }
    }

    private void validateName(string name, List<Firm?> firms)
    {
        if (firms.Any() && firms!.Any(x => x!.Name!.ToLower() == name.ToLower())) 
        {
            ModelState.AddModelError("Name", "This firm already exists.");
        }
    }

    private void validateDescription(string? englishDescription, string? estonianDescription)
    {
        if (englishDescription == null && estonianDescription == null)
        {
            ModelState.AddModelError("EstonianDescription", "A description is required in at least one language.");
        }
    }

    private void validatePlacement(string? row, string? column, List<Firm?> firms)
    {
        if (
            row != null && column != null 
            && firms.Any(x => x!.GridMapRow?.ToLower() == row.ToLower()) 
            && firms.Any(x => x!.GridMapColumn?.ToLower() == column.ToLower())) 
        {
            ModelState.AddModelError("GridMapRow", "This table is already taken.");
        }
    }
}
