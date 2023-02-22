using PkAPI.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkAPI.Services.Firebase;

namespace PkAPI.Controllers;

[ApiController]
[Route("api/map")]
public class MapController : ControllerBase
{
    private IFirebaseService _firebaseService;
    private IAuthorizationService _authService;
    public MapController(
        IFirebaseService firebase,
        IAuthorizationService auth) 
    {
        _authService = auth;
        _firebaseService = firebase;
    }

    [HttpPut("{status}")]
    public ActionResult UpdateMap(bool status)
    {
        if (!_authService.IsAuthorized(HttpContext.Request.Headers)) 
        {
            return Unauthorized();
        }

        _firebaseService.UpdateMapStatus(status);
        return Ok();
    }
}
