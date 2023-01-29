using PkAPI.Models;
using PkAPI.DTOs;

namespace PkAPI.Services.Firms;

public class FirmMapper : IFirmMapper
{
    public List<GetFirmsDTO> MapModelsToGetDTOs(List<Firm> firms)
    {
        var firmDTOs = new List<GetFirmsDTO>();
        foreach (var firm in firms)
        {
            firmDTOs.Add(new()
            {
                Id = firm.Id,
                Name = firm.Name
            });
        }
        return firmDTOs;
    }

    public GetFirmDTO MapModelToGetDTO(Firm firm)
    {
        return new()
        {
            Id = firm.Id,
            Name = firm.Name,
            EnglishDescription = firm.EnglishDescription,
            EstonianDescription = firm.EstonianDescription
        };
    }

    public Firm MapPostDTOToNewModel(PostFirmDTO newFirm)
    {
        return new()
        {
            Image = new() 
            { 
                Bytes = getFileBytes(newFirm.Image!), 
                Type = newFirm.Image!.ContentType 
            },
            Name = newFirm.Name,
            EnglishDescription = newFirm.EnglishDescription,
            EstonianDescription = newFirm.EstonianDescription
        };
    }

    public Firm MapPutDTOToOldModel(Firm oldFirm, PutFirmDTO newFirm)
    {
        if (newFirm.Image != null)
        {
            oldFirm.Image = new() 
            { 
                Bytes = getFileBytes(newFirm.Image!), 
                Type = newFirm.Image!.ContentType 
            };
        }
        oldFirm.Name = newFirm.Name ?? oldFirm.Name;
        oldFirm.EnglishDescription = newFirm.EnglishDescription ?? oldFirm.EnglishDescription;
        oldFirm.EstonianDescription = newFirm.EstonianDescription ?? oldFirm.EstonianDescription;
        return oldFirm;
    }

    private byte[] getFileBytes(IFormFile file)
    {
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
