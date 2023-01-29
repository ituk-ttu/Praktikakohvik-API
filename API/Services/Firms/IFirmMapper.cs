using PkAPI.Models;
using PkAPI.DTOs;

namespace PkAPI.Services.Firms;

public interface IFirmMapper
{
    List<GetFirmsDTO> MapModelsToGetDTOs(List<Firm> firms);
    GetFirmDTO MapModelToGetDTO(Firm firm);
    Firm MapPostDTOToNewModel(PostFirmDTO newFirm);
    Firm MapPutDTOToOldModel(Firm oldFirm, PutFirmDTO newFirm);
}
