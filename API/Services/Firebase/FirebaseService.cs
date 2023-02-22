using FireSharp.Interfaces;
using Newtonsoft.Json.Linq;
using FireSharp.Config;
using Newtonsoft.Json;
using PkAPI.Models;

namespace PkAPI.Services.Firebase;

public class FirebaseService : IFirebaseService
{
    private IFirebaseClient _firebaseClient;
    public FirebaseService()
    {
        _firebaseClient = new FireSharp.FirebaseClient(
            new FirebaseConfig
            {
                AuthSecret = Environment.GetEnvironmentVariable("AUTH"), 
                BasePath = Environment.GetEnvironmentVariable("URL")
            }
        );
    }

    public List<Firm?> GetFirms()
    {
        var response = _firebaseClient.Get("Firms");
        var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
        var firms = new List<Firm?>();
        if (data != null)
        {
            foreach (var item in data)
            {
                var firm = JsonConvert.DeserializeObject<Firm>(((JProperty)item).Value.ToString());
                if (firm != null) firms.Add(firm);
            }
        }
        return firms;
    }

    public Firm? GetFirm(string id)
    {
        var response = _firebaseClient.Get("Firms/" + id);
        return JsonConvert.DeserializeObject<Firm>(response.Body);
    }

    public string PostFirm(Firm newFirm)
    {
        var pushResponse = _firebaseClient.Push("Firms/", newFirm);
        newFirm.Id = pushResponse.Result.name;
        _firebaseClient.Set("Firms/" + newFirm.Id, newFirm);
        return newFirm.Id;
    }

    public void PutFirm(string id, Firm updatedFirm)
        => _firebaseClient.Set("Firms/" + id, updatedFirm);

    public void DeleteFirm(string id) 
        => _firebaseClient.Delete("Firms/" + id);

    public Map? GetMap()
    {
        var response = _firebaseClient.Get("Map");
        return JsonConvert.DeserializeObject<Map>(response.Body);
    }

    public void UpdateMapStatus(bool status)
    {
        var map = new Map() { Status = status };
        
        var dbMap = GetMap();
        if (dbMap == null)
        {
            var pushResponse = _firebaseClient.Push("Map/", map);
            map.Id = pushResponse.Result.name;
        }
        _firebaseClient.Set("Map/" + dbMap?.Id ?? map.Id, map);
    }
}
