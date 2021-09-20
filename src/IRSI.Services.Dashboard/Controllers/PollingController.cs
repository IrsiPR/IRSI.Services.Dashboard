using System.Threading.Tasks;
using IRSI.Services.Dashboard.Application.Polling.Commands;
using Microsoft.AspNetCore.Mvc;

namespace IRSI.Services.Dashboard.Controllers
{
    public class PollingController : ApiControllerBase
    {
        [HttpPost]
        public async Task Post(GeneratePollingCommand command)
        {
            await Mediator.Send(command);
        }
    }
}