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
using System.Reflection;
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
            string user = Context.GetHttpContext().Request.Query["access_token"];
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
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user))
            {
                var userInfo = _infoGameService[user];

                if (userInfo.State > StateReadyGame.Offer) return false;

                if (await _httpClientService.Start(user, enemy))
                {
                    userInfo.Enemy = enemy;

                    //parallel change state
                    if (userInfo.State > StateReadyGame.Offer) return false;

                    userInfo.State = StateReadyGame.Offer;
                    return true;
                }
            }
            return false;
        }

        public async Task Left()
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user))
            {
                await _httpClientService.Left(user);
                _infoGameService.Remove(user);
            }
        }

        public bool AddShip(ShipInfo shipInfo)
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user) && _infoGameService[user].State == StateReadyGame.Prepare)
            {
                if (shipInfo.TypeShip == TypeShips.Military)
                {
                    if (shipInfo.Act2 != 0) return false;

                    MilitaryShipModel ship = new MilitaryShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1);

                    if (_infoGameService[user].GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir))
                        return true;

                }
                else if (shipInfo.TypeShip == TypeShips.Support)
                {
                    if (shipInfo.Act2 != 0) return false;

                    SupportShipModel ship = new SupportShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1);

                    if (_infoGameService[user].GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir))
                        return true;

                }
                else if (shipInfo.TypeShip == TypeShips.Hybrid)
                {
                    HybridShipModel ship = new HybridShipModel(shipInfo.Len, shipInfo.Speed, shipInfo.R_act, shipInfo.Act1, shipInfo.Act2);

                    if (_infoGameService[user].GameField.AddShip(ship, shipInfo.X, shipInfo.Y, shipInfo.Dir))
                        return true;
                }
            }
            return false;
        }

        public async Task<bool> Ready()
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user) && _infoGameService.ShipsExist(user))
            {
                foreach (CoorModel coor in _infoGameService.GetCoors(user))
                {
                    CoorViewModel coorViewModel = coor.MapToCoorViewModel();
                    string infoShip = Serializer.Serialize(coorViewModel);

                    infoShip = await _httpClientService.AddShip(user, infoShip);

                    if (infoShip == null) return false;
                    coorViewModel = Serializer.Deserialize<CoorViewModel>(infoShip, _nameAssemblyModel);

                    coor.Id = coorViewModel.Id;
                    coor.Ship.Id = coorViewModel.Ship.Id;

                }
                if (await _httpClientService.Ready(user))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Move(InfoAct infoMove)
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user) && _infoGameService[user].State == StateReadyGame.Started)
            {
                if(await _httpClientService.Move(user, infoMove.Num, infoMove.X, infoMove.Y))
                {
                    _gameService.Move(user, infoMove.Num, infoMove.X, infoMove.Y);
                    return true;
                }
            }
            return false;
        }

        public async Task<StateShot> Shot(InfoAct infoShot)
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user) && _infoGameService[user].State == StateReadyGame.Started)
            {
                var res = await _httpClientService.Shot(user, infoShot.Num, infoShot.X, infoShot.Y);

                //Save shot's res to local

                return res;
                
            }
            return StateShot.Miss;
        }

        public async Task<bool> Fix(InfoAct infoFix)
        {
            string user = _infoGameService.GetUserByConnectionId(Context.ConnectionId);
            if (!String.IsNullOrEmpty(user) && _infoGameService[user].State == StateReadyGame.Started)
            {
                if(await _httpClientService.Fix(user, infoFix.Num, infoFix.X, infoFix.Y))
                {
                    return true;
                }
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
