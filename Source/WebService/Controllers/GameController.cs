using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;
using Pepp.Web.Apps.Bingo.Infrastructure.Managers;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class GameController : BaseController<IMapper, IGameAdapter>
    {
        private readonly ILiveControlsManager _manager;

        public GameController(
            IMapper mapper,
            IGameAdapter adapter,
            ILiveControlsManager manager
        ) : base(mapper, adapter) => _manager = manager;

        [HttpGet]
        public ActionResult<int?> GetActiveBoardID()
        {
            int? activeBoardID = _manager.GetActiveBoardID();
            return Ok(activeBoardID);
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetBoardNameByBoardID([FromQuery] int boardID)
        {
            BoardBE boardBE = await base.Adapter.GetBoard(boardID);
            string response = JsonSerializer.Serialize(boardBE.Name);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameTileBM>>> GetActiveBoardTilesByBoardID([FromQuery] int boardID)
        {
            List<BoardTileBE> boardTileBEs = await base.Adapter.GetAllBoardTiles(boardID);
            List<GameTileBM> gameTileBMs = boardTileBEs
                ?.Where(tile => tile.IsActive)
                .Select(tile => base.Mapper.Map<GameTileBM>(tile)).ToList();
            return gameTileBMs;
        }
    }
}
