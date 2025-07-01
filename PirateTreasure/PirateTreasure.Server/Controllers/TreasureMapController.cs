using MediatR;
using Microsoft.AspNetCore.Mvc;
using PirateTreasure.Application.Features.TreasureMaps.Commands.CreateTreasureMap;
using PirateTreasure.Application.Features.TreasureMaps.Commands.SolveTreasureMap;
using PirateTreasure.Application.Features.TreasureMaps.Queries.GetAllTreasureMaps;
using PirateTreasure.Application.Features.TreasureMaps.Queries.GetTreasureMap;

namespace PirateTreasure.Server.Controllers
{
    [ApiController]
    [Route("api/treasure-map")]
    public class TreasureMapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TreasureMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/treasure-maps
        [HttpPost]
        public async Task<IActionResult> Create(CreateTreasureMapCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        // GET: api/treasure-maps
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTreasureMapsQuery());
            return Ok(result);
        }

        // GET: api/treasure-maps/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTreasureMapByIdQuery(id));
            return Ok(result);
        }

        // POST: api/treasure-maps/{id}/solve
        [HttpPost("{id}/solve")]
        public async Task<IActionResult> Solve(Guid id)
        {
            var result = await _mediator.Send(new SolveTreasureMapCommand(id));
            return Ok(result);
        }
    }
}