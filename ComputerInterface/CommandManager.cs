using System.Collections.Generic;
using ComputerInterface;
using Zenject;
using Photon.Pun;
using UnityEngine;
using System;
using System.Text;
using GorillaNetworking;

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
                if (NetworkSystem.Instance.InRoom) //inspired by lunakittyyys easyping mod (https://github.com/lunakittyyy/EasyPing)
                {
                    return $"Ping is: {PhotonNetwork.GetPing()}ms \nYour region: {PhotonNetwork.CloudRegion.Replace("/*", "").ToUpper()}";
                }
                else
                {
                    return "Not in a room!";
                }
            }));

            RegisterCommand(new Command(name: "fps", argumentTypes: null, args => //frames per second
            {
                return "FPS (for this frame) is: " + (1f / Time.deltaTime);
            }));

            RegisterCommand(new Command(name: "playerid", argumentTypes: null, args =>
            {
                return "Your player ID is: " + NetworkSystem.Instance.LocalPlayer.UserId;
            }));

            RegisterCommand(new Command(name: "credit", argumentTypes: null, args =>
            {
                return "Commands made by kino \nping command inspired by lunakittyyys easyping mod | github.com/lunakittyyy/EasyPing";
            }));

            RegisterCommand(new Command(name: "time", argumentTypes: null, args =>
            {
                return "Your local date and time is: " + DateTime.UtcNow;
            }));

            RegisterCommand(new Command(name: "appversion", argumentTypes: null, args =>
            {
                return "Current app version is: " + GorillaComputer.instance.version;
            }));

            RegisterCommand(new Command(name: "master", argumentTypes: null, args =>
            {
                if (NetworkSystem.Instance.InRoom)
                {
                    if (NetworkSystem.Instance.IsMasterClient)
                        return "You're master!";
                    else
                        return "You aren't master!";
                }
                else
                {
                    return "Not in a room!"; //gotta be in a room lil bro, lock in!!!
                }
            }));

            RegisterCommand(new Command(name: "players", argumentTypes: null, args =>
            {
                return "There are " + NetworkSystem.Instance.GlobalPlayerCount() + " globally online.";
            }));

            RegisterCommand(new Command(name: "help", argumentTypes: null, args =>
            {
                return "Command List: (see 'help2' for more commands)\nfps\nplayerid\nping\ntime\nplayers";
            }));

            RegisterCommand(new Command(name: "help2", argumentTypes: null, args =>
            {
                return "Command List:\nplayers\nmaster\nappversion\ntime\ncredit";
            })); 

            RegisterCommand(new Command(name: "help3", argumentTypes: null, args =>
            {
                return "Command List:\njoinrng";
            })); // for help commands, 5 per help.

            static string rngsstring() // creates a 5 char string
            {
                System.Random rng = new System.Random();
                const string chars = "QWERTYUIOPASDFGHJKLZXCVBNM";
                var _string = new StringBuilder(5);

                for (int i = 0; i < 5; i++)
                {
                    _string.Append(chars[rng.Next(chars.Length)]);
                }

                return _string.ToString();
            }
            RegisterCommand(new Command(name: "joinrng", argumentTypes: null, args =>
            {
                if (NetworkSystem.Instance.InRoom)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(rngsstring(), JoinType.Solo); // joining that random string that we created
                    return $"Leaving {NetworkSystem.Instance.CurrentRoom} and joining random room";
                }
                else
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(rngsstring(), JoinType.Solo);
                    return "Joining random room";
                }
            }));

            RegisterCommand(new Command(name: "creation", argumentTypes: null, args =>
            {
                // lol i saw that comment 
                return "still figuring this one out, sorry!";
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
