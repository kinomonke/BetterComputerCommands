using BepInEx;
using ComputerInterface.Behaviours;
using ComputerInterface.Enumerations;
using ComputerInterface.Extensions;
using ComputerInterface.Interfaces;
using ComputerInterface.Models;
using ComputerInterface.Models.UI;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BetterComputerCommands
{
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERS)]
    public class Plugin : BaseUnityPlugin
    {
        void Start() {
            var harmony = Harmony.CreateAndPatchAll(GetType().Assembly, Constants.GUID);
        }

        public class GameSettingsEntry : IComputerModEntry
        {
            public string EntryName => Constants.NAME;
            public Type EntryViewType => typeof(BCCView);
        }

        public class BCCView : ComputerView
        {
            private readonly UITextInputHandler _textInputHandler = new();
            private int _currentPage = 0;
            private string _output = "";

            private VRRig _rig;
            private static Dictionary<string, string> datePool = new();
            private readonly string[][] _commandPages = new string[][]
            {
                new string[]
                {
                    "ping:<color=#ffffff50><size=50>displays your latency.</size></color>",
                    "fps: <color=#ffffff50><size=50>displays your current framerate.</size></color>",
                    "time: <color=#ffffff50><size=50>displays your local time.</size></color>"
                },
                new string[]
                {
                    "playerid: <color=#ffffff50><size=50>displays your player ID. [Do Not Share!]</size></color>",
                    "appversion: <color=#ffffff50><size=50>displays Gorilla Tags' Current version.</size></color>",
                    "players: <color=#ffffff50><size=50> displays the global amount of players online.</size></color>",
                },
            };


            public override void OnShow(object[] args)
            {
                base.OnShow(args);

                if (args.Length > 0 && args[0] is VRRig rig)
                    _rig = rig;

                Redraw();
            }


            private string _lastCommandOutput = "";
            void Redraw()
            {
                StringBuilder str = new();

                str.BeginColor("ffffff50").Repeat("=", ScreenWidth).EndColor().AppendLine();

                str.BeginCenter().AppendClr(Constants.NAME, "BCC3F2FF").AppendLine();
                str.AppendClr("A mod by ", "ffffff50").Append("kinomonke.").EndAlign().AppendLine();

                str.BeginColor("ffffff50").Repeat("=", ScreenWidth).EndColor().AppendLine();

                str.BeginCenter().AppendClr($"<size=35>press the left or right arrow to show more commands!</size>", "ffffff50").
                    EndAlign().AppendLine();

                str.AppendClr($"Available commands: [Page: {_currentPage + 1}]", "9294B3").AppendLine();

                foreach (var command in _commandPages[_currentPage])
                {
                    str.Append(" - ").Append(command).AppendLine();
                }

                str.BeginColor("ffffff50").Append(">").EndColor().Append(_textInputHandler.Text).AppendClr("_", "ffffff50");

                if (!string.IsNullOrEmpty(_lastCommandOutput))
                {
                    str.AppendLine().AppendClr(_lastCommandOutput, "ffffff50");
                }

                Text = str.ToString();
            }

            public override void OnKeyPressed(EKeyboardKey key)
            {
                base.OnKeyPressed(key);

                if (_textInputHandler.HandleKey(key))
                {
                    if (_textInputHandler.Text.Length > BaseGameInterface.MaxNameLength)
                        _textInputHandler.Text = _textInputHandler.Text[..BaseGameInterface.MaxNameLength];

                    Redraw();
                    return;
                }


                switch (key)
                {
                    case EKeyboardKey.Back:
                        ReturnToMainMenu();
                        break;

                    case EKeyboardKey.Left:
                        _currentPage--;
                        if (_currentPage < 0) _currentPage = _commandPages.Length - 1;
                        Redraw();
                        break;
                    case EKeyboardKey.Right:
                        _currentPage++;
                        if (_currentPage >= _commandPages.Length) _currentPage = 0;
                        Redraw();
                        break;
                    case EKeyboardKey.Enter:
                        _lastCommandOutput = ExecuteCommand(_textInputHandler.Text.Trim());
                        _textInputHandler.Text = "";
                        Redraw();
                        break;
                }
            }

            private string ExecuteCommand(string command)
            {
                switch (command.ToLower())
                {
                    case "ping":
                        return $"Ping: {PhotonNetwork.GetPing()}";
                    case "fps":
                        return $"FPS: {(int)(1f / Time.deltaTime)}";
                    case "time":
                        return $"Local Time: {DateTime.Now:HH:mm:ss}";
                    case "playerid":
                        return $"Player ID: {PhotonNetwork.LocalPlayer.UserId}";
                    case "appversion":
                        return $"Gorilla Tag Version: {GorillaComputer.instance.version}";
                    case "players":
                        return $"Players online: {NetworkSystem.Instance.GlobalPlayerCount()}";
                    default:
                        return $"Unknown command: {command}";
                }
            }

            private string GetDate(VRRig rig)
            {
                string userId = rig.Creator.UserId;

                if (datePool.ContainsKey(userId))
                    return datePool[userId];
                else
                {
                    datePool.Add(userId, "LOADING");
                    PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = userId },
                        result =>
                        {
                            string date = result.AccountInfo.Created.ToString("MMM dd, yyyy HH:mm").ToUpper();
                            datePool[userId] = date;
                            rig.UpdateName();
                            Redraw();
                        },
                        error =>
                        {
                            datePool[userId] = "ERROR";
                            rig.UpdateName();
                            Redraw();
                        },
                        null, null);

                    return "LOADING";
                }
            }
        }
    }

    public class Constants
    {
        public const string GUID = "kinomonke.bettercomputercommands",
                            NAME = "BetterComputerCommands",
                            VERS = "2.0";
    }
}