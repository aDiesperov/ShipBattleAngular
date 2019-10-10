using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleAngularApi.Web.Extensions;
using ShipBattleAngularApi.Web.Hubs;
using ShipBattleAngularApi.Web.Hubs.Interfaces;
using ShipBattleAngularApi.Web.Models;
using ShipBattleApi.Models.Enums;
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
            var userInfo = _infoGameService[user];
            if (userInfo != null && !String.IsNullOrEmpty(msg))
                await _hubContext.Clients.Client(userInfo.ConnectionId).ReceiveMessage(msg, false);
        }

        [HttpPost("prepareGame/{user}")]
        public async Task PrepareGameAsync([FromRoute]string user, [FromBody]InfoGameFieldViewModel infoGameField)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                GameFieldViewModel gameFieldViewModel = Serializer.Deserialize<GameFieldViewModel>(infoGameField.FieldGame);
                GameFieldModel gameFieldModel = gameFieldViewModel.MapToGameFieldModel();

                _gameService.PrepareGame(userInfo, gameFieldModel, infoGameField.MyQueue);

                await _hubContext.Clients.Client(userInfo.ConnectionId).PrepareGame(gameFieldViewModel.Id);
            }
        }

        [HttpHead("startedGame/{user}")]
        public async Task StartedGameAsync([FromRoute]string user)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                userInfo.State = StateReadyGame.Started;
                await SendAsync(user, _messageStartGame);
                if (userInfo.MyQueue)
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
                _gameService.ResetGame(userInfo);

                string msg = _messageGameFinished;
                if (win) msg += "\n" + _messageWinGame;
                else msg += "\n" + _messageFailGame;

                await _hubContext.Clients.Client(userInfo.ConnectionId).ReceiveMessage(msg, false);
            }
        }

        [HttpPost("hitShip/{user}")]
        public async Task HitShipAsync([FromRoute]string user, [FromBody]InfoHit infoHit)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                _gameService.HitShip(userInfo, infoHit.NumShoted, infoHit.DamageShoted, infoHit.DiedShip);

                string msg;
                if (infoHit.DiedShip) msg = _messageDiedShip;
                else msg = _messageHitShip;

                await _hubContext.Clients.Client(userInfo.ConnectionId).Hit(msg, infoHit.NumShoted, infoHit.DamageShoted, infoHit.DiedShip);
            }
        }

        [HttpPost("fixShip/{user}")]
        public async Task FixShipAsync([FromRoute]string user, [FromBody]InfoFix infoFix)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                _gameService.FixShip(userInfo, infoFix.NumShip, infoFix.Broken);

                await _hubContext.Clients.Client(userInfo.ConnectionId).Fix(infoFix.NumShip, infoFix.Broken);
            }
        }

        [HttpHead("offer/{user}/{enemy}")]
        public async Task Offer([FromRoute]string user, [FromRoute]string enemy)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                await _hubContext.Clients.Client(userInfo.ConnectionId).Offer(enemy);
            }
        }

        [HttpHead("nextStep/{user}")]
        public async Task NextStep([FromRoute]string user)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                userInfo.MyQueue = true;
                await _hubContext.Clients.Client(userInfo.ConnectionId).NextStep();
            }
        }
    }
}