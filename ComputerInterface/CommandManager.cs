using System.Collections.Generic;
using ComputerInterface;
using Zenject;
using Photon.Pun;
using UnityEngine;

namespace ComputerModExample
{
    internal class PingCommand : IInitializable
    {
        private readonly CommandHandler _commandHandler;
        private List<CommandToken> _commandTokens;

        public PingCommand(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Initialize()
        {
            _commandTokens = new List<CommandToken>();

            RegisterCommand(new Command(name: "ping", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true) //inspired by lunakittyyys easyping mod (https://github.com/lunakittyyy/EasyPing)
                {
                    return "Ping is: " + PhotonNetwork.GetPing() + "ms \n Your reigon: " + PhotonNetwork.CloudRegion.Replace("/*", "").ToUpper();
                }
                else
                {
                    return "Not in a room!";
                }
            }));
            RegisterCommand(new Command(name: "fps", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "FPS (for this frame) is: " + (1f / Time.deltaTime).ToString();
                }
                else
                {
                    return "FPS (for this frame) is: " + (1f / Time.deltaTime).ToString();
                }
            }));
            RegisterCommand(new Command(name: "playerid", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "Your PlayerID is: " + (PhotonNetwork.LocalPlayer.UserId).ToString();
                }
                else
                {
                    return "Your PlayerID is: " + (PhotonNetwork.LocalPlayer.UserId).ToString();
                }
            }));
            RegisterCommand(new Command(name: "credit", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "Commands made by kino \nping command inspired by lunakittyyys easyping mod - github.com/lunakittyyy/EasyPing\n";
                }
                else
                {
                    return "Commands made by kino \nping command inspired by lunakittyyys easyping mod - github.com/lunakittyyy/EasyPing";
                }
            }));
            void RegisterCommand(Command cmd)
            {
                var token = _commandHandler.AddCommand(cmd);
                _commandTokens.Add(token);
            }

            void UnregisterAllCommands()
            {
                foreach (var token in _commandTokens)
                {
                    token.UnregisterCommand();
                }
            }

            void Dispose()
            {
                UnregisterAllCommands();
            }
        }
    }
}
