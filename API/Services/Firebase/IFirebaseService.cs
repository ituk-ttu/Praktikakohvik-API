using PkAPI.Models;

namespace PkAPI.Services.Firebase;

public interface IFirebaseService
{
    List<Firm?> GetFirms();
    Firm? GetFirm(string id);
    string PostFirm(Firm newFirm);
    void PutFirm(string id, Firm updatedFirm);
    void DeleteFirm(string id);
    Map? GetMap();
    void UpdateMapStatus(bool status);
}
