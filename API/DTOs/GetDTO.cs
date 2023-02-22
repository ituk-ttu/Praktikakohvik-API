namespace PkAPI.DTOs;

public class GetDTO
{
    public bool? DisplayMap { get; set; }
    public List<GetFirmDTO>? Firms { get; set; }
}
