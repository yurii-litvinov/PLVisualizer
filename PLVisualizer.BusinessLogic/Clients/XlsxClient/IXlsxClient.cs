using Microsoft.AspNetCore.Http;
using PlVisualizer.Api.Dto.Tables;

namespace PLVisualizer.BusinessLogic.Clients.XlsxClient;

public interface IXlsxClient
{
    XlsxTableRow[] TableRows { get; set; }
    void SetFile(IFormFile file);
}