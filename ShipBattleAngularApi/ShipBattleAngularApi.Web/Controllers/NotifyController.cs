using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleAngularApi.Web.Extensions;
using ShipBattleAngularApi.Web.Hubs;
using ShipBattleAngularApi.Web.Hubs.Interfaces;
using ShipBattleAngularApi.Web.Models;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly string _messageStartGame = "Game has started!";
        private readonly string _messageYourTurn = "Your turn!";
        private readonly string _messageGameFinished = "Game finished!";
        private readonly string _messageWinGame = "You win!";
        private readonly string _messageFailGame = "You failed!";
        private readonly string _messageHitShip = "Player hitted in your ship!";
        private readonly string _messageDiedShip = "Player killed your ship!";

        private readonly IHubContext<HubSR, IClientApi> _hubContext;
        private readonly IInfoGameService _infoGameService;
        private readonly IGameService _gameService;
        

        public NotifyController(IHubContext<HubSR, IClientApi> hubContext, IInfoGameService infoGameService, IGameService gameService)
        {
            _hubContext = hubContext;
            _infoGameService = infoGameService;
            _gameService = gameService;
        }

        [HttpPost("sendAll")]
        public async Task SendAllAsync([FromBody]string msg)
        {
            if (String.IsNullOrEmpty(msg)) return;

            await _hubContext.Clients.All.ReceiveMessage(msg, false);
        }

        [HttpPost("send/{user}")]
        public async Task SendAsync([FromRoute]string user, [FromBody]string msg)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(msg)) return;

            if (_infoGameService.Exists(user))
            {
                string conId = _infoGameService.GetConnectionId(user);
                await _hubContext.Clients.Client(conId).ReceiveMessage(msg, false);
            }
        }

        [HttpPost("prepareGame/{user}/{enemy}")]
        public async Task PrepareGameAsync([FromRoute]string user, [FromRoute]string enemy, [FromBody]InfoGameFieldViewModel infoGameField)
        {
            if (_infoGameService.Exists(user))
            {

                GameFieldViewModel gameFieldViewModel = Serializer.Deserialize<GameFieldViewModel>(infoGameField.FieldGame);
                GameFieldModel gameFieldModel = gameFieldViewModel.MapToGameFieldModel();

                if (_gameService.PrepareGame(user, gameFieldModel, infoGameField.MyQueue))
                {
                    int idField = gameFieldViewModel.Id;
                    string conId = _infoGameService.GetConnectionId(user);

                    await _hubContext.Clients.Client(conId).PrepareGame(idField, enemy);
                }
            }
        }

        [HttpHead("startedGame/{user}")]
        public async Task StartedGameAsync([FromRoute]string user)
        {
            if (String.IsNullOrEmpty(user)) return;

            if (_gameService.StartedGame(user))
            {
                await SendAsync(user, _messageStartGame);
                if (_infoGameService[user].MyQueue)
                {
                    await SendAsync(user, _messageYourTurn);
                }
            }
        }

        [HttpHead("finishGame/{user}/{win}")]
        public async Task FinishGameAsync([FromRoute]string user, [FromRoute]bool win)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                _gameService.ResetGame(user);

                string conId = _infoGameService.GetConnectionId(user);

                string msg = _messageGameFinished;
                if (win)
                {
                    msg += "\n" + _messageWinGame;
                }
                else
                {
                    msg += "\n" + _messageFailGame;
                }

                await _hubContext.Clients.Client(conId).ReceiveMessage(msg, false);
            }
        }

        [HttpPost("hitShip/{user}")]
        public async Task HitShipAsync([FromRoute]string user, [FromBody]InfoHit infoHit)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                _gameService.HitShip(user, infoHit.NumShoted, infoHit.DamageShoted, infoHit.DiedShip);

                string msg;
                if (infoHit.DiedShip)  msg = _messageDiedShip;
                else msg = _messageHitShip;

                string conId = _infoGameService.GetConnectionId(user);

                await _hubContext.Clients.Client(conId).Hit(msg, infoHit.NumShoted, infoHit.DamageShoted, infoHit.DiedShip);
            }
        }

        [HttpPost("fixShip/{user}")]
        public async Task FixShipAsync([FromRoute]string user, [FromBody]InfoFix infoFix)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                _gameService.FixShip(user, infoFix.NumShip, infoFix.Broken);

                string conId = _infoGameService.GetConnectionId(user);

                await _hubContext.Clients.Client(conId).Fix(infoFix.NumShip, infoFix.Broken);
            }
        }
    }
}