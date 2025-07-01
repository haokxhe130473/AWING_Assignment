using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateTreasure.Application.Features.TreasureMaps.Commands.SolveTreasureMap
{
    public class SolveTreasureMapCommand : IRequest<double>
    {
        public Guid TreasureMapId { get; set; }

        public SolveTreasureMapCommand(Guid treasureMapId)
        {
            TreasureMapId = treasureMapId;
        }
    }

}
