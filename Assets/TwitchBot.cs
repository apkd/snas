using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using S = UnityEngine.SerializeField;

public sealed class TwitchBot : MonoBehaviour
{
    [S] AnimateText text;
    [S] ChatMenu chatMenu;

    [S] int textCount = 5;

    public Client Client { get; private set; }

    void OnEnable()
    {
        Client = new Client();

        var texts = new Queue<string>(capacity: textCount);

        Client.Initialize(
            credentials: new ConnectionCredentials(
                twitchUsername: Secrets.TWITCH_USERNAME,
                twitchOAuth: Secrets.TWITCH_OAUTH_TOKEN
            ),
            channel: Secrets.TWITCH_USERNAME
        );

        Client.OnError
            += (sender, args) => Debug.LogException(args.Exception, this);

        Client.OnIncorrectLogin
            += (sender, args) => Debug.LogException(args.Exception, this);

        Client.OnConnectionError
            += (sender, args) => Debug.LogError(args.Error.Message, this);

        Client.OnLog
            += (sender, args) => Debug.Log(args.Data, this);

        Client.OnDisconnected
            += (sender, args) => Debug.Log("Disconnected", this);

        Client.OnReconnected
            += (sender, args) => Debug.Log("Reconnected", this);

        Client.OnConnected += (sender, args) =>
        {
            Debug.Log($"The bot {args.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(args.AutoJoinChannel))
                Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {args.AutoJoinChannel}");
        };

        Client.OnJoinedChannel += (sender, args) =>
        {
            Debug.Log($"The bot {args.BotUsername} just joined the channel: {args.Channel}");
            // client.SendMessage(args.Channel, "I just joined the channel! PogChamp");
        };

        Client.OnMessageReceived += (sender, args) =>
        {
            if (texts.Count >= textCount)
                texts.Dequeue();

            var message = args.ChatMessage.Message;

            // strip html
            message = Regex.Replace(
                input: message,
                pattern: "<.*?>",
                replacement: ""
            );

            texts.Enqueue(message);
            chatMenu.SetLines(texts.ToArray());
        };

        Client.Connect();
    }

    void OnDisable()
    {
        Client.Disconnect();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Client.SendMessage(
    //             channel: Secrets.TWITCH_USERNAME,
    //             message: "I pressed the space key within Unity."
    //         );
    //     }
    // }
}
