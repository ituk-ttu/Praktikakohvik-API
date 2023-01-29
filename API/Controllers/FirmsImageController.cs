using Microsoft.AspNetCore.Mvc;
using PkAPI.Services.Firebase;

namespace PkAPI.Controllers;

[ApiController]
[Route("api/firms/{Id}/image")]
public class FirmsImageController : ControllerBase
{
    private IFirebaseService _firebaseService;
    public FirmsImageController(IFirebaseService firebaseService) 
    {
        _firebaseService = firebaseService;
    }

    [HttpGet("{time}")]
    public ActionResult GetImage(string Id, string time)
    {
        var firm = _firebaseService.GetFirm(Id);
        if (firm == null) 
        {
            return NotFound();
        }

        return File(firm.Image!.Bytes!, firm.Image.Type!);
    }
}
