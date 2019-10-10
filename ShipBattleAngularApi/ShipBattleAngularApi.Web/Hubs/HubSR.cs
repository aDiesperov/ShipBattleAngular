using Microsoft.AspNetCore.SignalR;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleAngularApi.Web.Enums;
using ShipBattleAngularApi.Web.Extensions;
using ShipBattleAngularApi.Web.Hubs.Interfaces;
using ShipBattleAngularApi.Web.Mappers;
using ShipBattleAngularApi.Web.Models;
using ShipBattleApi.Models.Enums;
using ShipBattleApi.Models.Models;
using System;
using System.Threading.Tasks;

namespace ShipBattleAngularApi.Web.Hubs
{
    public class HubSR : Hub<IClientApi>
    {

        private readonly string _messageYouAreGaming = "You are gaming yet!";
        private readonly string _messageWelcome = "Welcome to the Game!";

        private readonly IGameService _gameService;
        private readonly IInfoGameService _infoGameService;
        private readonly IHttpClientService _httpClientService;
        private readonly string _nameAssemblyModel = "ShipBattleAngularApi.Web";

        public HubSR(IInfoGameService infoGameService, IHttpClientService httpClientService, IGameService gameService)
        {
            _gameService = gameService;
            _infoGameService = infoGameService;
            _httpClientService = httpClientService;
        }

        public override async Task OnConnectedAsync()
        {
            string user = Context.GetHttpContext().Request.Cookies["name"];
            if (!String.IsNullOrEmpty(user))
            {
                string pathBase = Context.GetHttpContext().Request.Host.Value;
                if (_infoGameService.Exists(user))
                {
                    await Clients.Caller.ReceiveMessage(_messageYouAreGaming, true);
                }
                else if (await _httpClientService.Join(user, pathBase))
                {
                    _infoGameService.Add(user, Context.ConnectionId);
                    await Clients.Caller.ReceiveMessage(_messageWelcome, false);
                }
                else
                {
                    await Clients.Caller.ReceiveMessage(_messageYouAreGaming, true);
                }
            }

            await base.OnConnectedAsync();
        }

        public async Task<bool> Start(string enemy)
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State < StateReadyGame.Prepare)
            {
                if (await _httpClientService.Start(_infoGameService.GetName(userInfo), enemy))
                {
                    userInfo.Enemy = enemy;

                    //parallel change state
                    if (userInfo.State < StateReadyGame.Prepare)
                    {
                        userInfo.State = StateReadyGame.Offer;
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task Left()
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null)
            {
                string user = _infoGameService.GetName(userInfo);
                await _httpClientService.Left(user);
                _infoGameService.Remove(user);
            }
        }

        public bool AddShip(ShipInfo shipInfo)
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Prepare)
            {
                ShipModel ship;

                switch (shipInfo.TypeShip)
                {
                    case TypeShips.Military:
                        if (shipInfo.Act2 != 0) return false;

                        ship = new MilitaryShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1);

                        if (userInfo.GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir)) return true;
                        break;
                    case TypeShips.Support:
                        if (shipInfo.Act2 != 0) return false;

                        ship = new SupportShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1);

                        if (userInfo.GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir)) return true;
                        break;
                    case TypeShips.Hybrid:
                        ship = new HybridShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1, shipInfo.Act2);

                        if (userInfo.GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir)) return true;
                        break;
                }
            }
            return false;
        }

        public async Task<int> Ready()
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Prepare && _infoGameService.ShipsExist(userInfo))
            {
                string user = _infoGameService.GetName(userInfo);

                foreach (CoorModel coor in userInfo.GameField.Coors)
                {
                    CoorViewModel coorViewModel = coor.MapToCoorViewModel();
                    string infoShip = Serializer.Serialize(coorViewModel);

                    infoShip = await _httpClientService.AddShip(user, infoShip);

                    if (infoShip == null) return 0;
                    coorViewModel = Serializer.Deserialize<CoorViewModel>(infoShip, _nameAssemblyModel);

                    coor.Id = coorViewModel.Id;
                    coor.Ship.Id = coorViewModel.Ship.Id;
                }

                int res = await _httpClientService.Ready(user);
                if (res == 1)
                    userInfo.State = StateReadyGame.Ready;

                return res;
            }
            return 0;
        }

        public async Task<bool> NextStep()
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Started && userInfo.MyQueue)
            {
                string user = _infoGameService.GetName(userInfo);

                if (await _httpClientService.NextStep(user))
                {
                    userInfo.MyQueue = false;
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Move(InfoAct infoMove)
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Started)
            {
                if (await _httpClientService.Move(_infoGameService.GetName(userInfo), infoMove.Num, infoMove.X, infoMove.Y))
                {
                    _gameService.Move(userInfo, infoMove.Num, infoMove.X, infoMove.Y);
                    return true;
                }
            }
            return false;
        }

        public async Task<StateShot> Shot(InfoAct infoShot)
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Started)
            {
                var res = await _httpClientService.Shot(_infoGameService.GetName(userInfo), infoShot.Num, infoShot.X, infoShot.Y);

                //Save shot's res to local

                return res;

            }
            return StateShot.Miss;
        }

        public async Task<bool> Fix(InfoAct infoFix)
        {
            var userInfo = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (userInfo != null && userInfo.State == StateReadyGame.Started)
            {
                if (await _httpClientService.Fix(_infoGameService.GetName(userInfo), infoFix.Num, infoFix.X, infoFix.Y))
                    return true;
            }
            return false;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Left();
            await base.OnDisconnectedAsync(exception);
        }

    }
}
