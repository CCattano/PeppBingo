﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class GameController : BaseController<IMapper, IGameAdapter>
    {
        private readonly ILiveDataCache _liveDataCache;

        public GameController(
            IMapper mapper,
            IGameAdapter adapter,
            ILiveDataCache liveDataCache
        ) : base(mapper, adapter)
        {
            _liveDataCache = liveDataCache;
        }

        [HttpGet]
        public ActionResult<int?> GetActiveBoardID()
        {
            int? activeBoardID = _liveDataCache.GetActiveBoardID();
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

        [HttpGet]
        public ActionResult<string> GetCurrentResetID()
        {
            string resetEventID = _liveDataCache.GetResetEventID();
            return Ok(JsonSerializer.Serialize(resetEventID));
        }
    }
}
