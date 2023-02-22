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
            ShortName = firm.ShortName,
            EnglishDescription = firm.EnglishDescription,
            EstonianDescription = firm.EstonianDescription,
            GridMapColumn = firm.GridMapColumn,
            GridMapRow = firm.GridMapRow
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
            ShortName = newFirm.ShortName,
            EnglishDescription = newFirm.EnglishDescription,
            EstonianDescription = newFirm.EstonianDescription,
            GridMapColumn = newFirm.GridMapColumn,
            GridMapRow = newFirm.GridMapRow
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
        oldFirm.ShortName = newFirm.ShortName ?? oldFirm.ShortName;
        oldFirm.EnglishDescription = newFirm.EnglishDescription;
        oldFirm.EstonianDescription = newFirm.EstonianDescription;
        oldFirm.GridMapColumn = newFirm.GridMapColumn;
        oldFirm.GridMapRow = newFirm.GridMapRow;
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
