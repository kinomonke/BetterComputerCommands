using System.Collections.Generic;
using ComputerInterface;
using Zenject;
using Photon.Pun;
using UnityEngine;
using System;

namespace ComputerModExample //i wont explain what this stuff does i'm really bad at it theres decent documentation on how to use/create commands at: https://github.com/DecalFree/GorillaInterface
{
    internal class CustomCommands : IInitializable
    {
        private readonly CommandHandler _commandHandler;
        private readonly string playFabId;
        private List<CommandToken> _commandTokens;

        public CustomCommands(CommandHandler commandHandler)
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
            RegisterCommand(new Command(name: "fps", argumentTypes: null, args => //frames per second
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
                    return "Commands made by kino \nping command inspired by lunakittyyys easyping mod | github.com/lunakittyyy/EasyPing";
                }
                else
                {
                    return "Commands made by kino \nping command inspired by lunakittyyys easyping mod | github.com/lunakittyyy/EasyPing";
                }
            }));

            RegisterCommand(new Command(name: "time", argumentTypes: null, args =>
            {
                if (!PhotonNetwork.InRoom == true)
                {
                    return "Your local date and time is" + DateTime.UtcNow.ToString();
                }
                else
                {
                    return "Your local date and time is" + DateTime.UtcNow.ToString();
                }
            }));

            RegisterCommand(new Command(name: "appversion", argumentTypes: null, args =>
            {
                if (!PhotonNetwork.InRoom == true)
                {
                    return "Current app version is:" + PhotonNetwork.AppVersion.ToString();
                }
                else
                {
                    return "Current app version is:" + PhotonNetwork.AppVersion.ToString();
                }
            }));

            RegisterCommand(new Command(name: "master", argumentTypes: null, args =>
            {
                if (!PhotonNetwork.InRoom == true)
                {
                    return PhotonNetwork.IsMasterClient.ToString();
                }
                else
                {
                    return "Not in a room!"; //gotta be in a room lil bro, lock in!!!
                }
            }));

            RegisterCommand(new Command(name: "players", argumentTypes: null, args =>
            {
                if (!PhotonNetwork.InRoom == true)
                {
                    return "There are" + PhotonNetwork.CountOfPlayers.ToString() + "online.";
                }
                else
                {
                    return PhotonNetwork.CountOfPlayers.ToString();
                }
            }));
            RegisterCommand(new Command(name: "help", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "Command List: (see 'help2' for more commands)\nfps\nplayerid\nping\ntime\nplayers"; //1, 2, 3, 4, 5
                }
                else
                {
                    return "Command List: (see 'help2' for more commands)\nfps\nplayerid\nping\ntime\nplayers";
                }
            }));

            RegisterCommand(new Command(name: "help2", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "Command List:\nplayers\nmaster\nappversion\ntime\ncredit"; // 1, 2, 3, 4, 5
                }
                else
                {
                    return "Command List:\nplayers\nmaster\nappversion\ntime\ncredit";
                }
            })); // for help commands, 5 per help.
            RegisterCommand(new Command(name: "creation", argumentTypes: null, args =>
            {
                if (PhotonNetwork.InRoom == true)
                {
                    return "still figuring this one out, sorry!";
                }
                else // sorry king defaultuser0 (efaultuber0) 
                {
                    return "still figuring this one out, sorry!";
                }
            })); // when i get this it will go in help3 :)
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

        void UnregCommands()
            {
                UnregisterAllCommands();
            }
        }
    }
}
