using System.Threading.Tasks;
using IRSI.Services.Dashboard.Application.AutoUpdate.Commands;
using Microsoft.AspNetCore.Mvc;

namespace IRSI.Services.Dashboard.Controllers
{
    public class AutoUpdateController : ApiControllerBase
    {
        [HttpPost]
        public async Task PostUpdateRelease(UpdateReleaseCommand command)
        {
            await Mediator.Send(command);
        }
    }
}