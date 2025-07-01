using MediatR;
using PirateTreasure.Application.Features.TreasureMaps.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateTreasure.Application.Features.TreasureMaps.Queries.GetAllTreasureMaps
{
    public class GetAllTreasureMapsQuery : IRequest<List<TreasureMapDto>>
    {
    }
}
