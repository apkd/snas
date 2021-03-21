using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using S = UnityEngine.SerializeField;

public sealed class TwitchBot : MonoBehaviour
{
    [S] AnimateText text;

    Client client;

    void OnEnable()
    {
        client = new Client();

        client.Initialize(
            credentials: new ConnectionCredentials(
                twitchUsername: Secrets.TWITCH_USERNAME,
                twitchOAuth: Secrets.TWITCH_OAUTH_TOKEN
            ),
            channel: Secrets.TWITCH_USERNAME
        );

        client.OnConnected += (sender, args) =>
        {
            Debug.Log($"The bot {args.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(args.AutoJoinChannel))
                Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {args.AutoJoinChannel}");
        };

        client.OnJoinedChannel += (sender, args) =>
        {
            Debug.Log($"The bot {args.BotUsername} just joined the channel: {args.Channel}");
            client.SendMessage(args.Channel, "I just joined the channel! PogChamp");
        };

        client.OnMessageReceived += (sender, args) =>
        {
            text.SetNewText(args.ChatMessage.Message);
            Debug.Log($"Message received from {args.ChatMessage.Username}: {args.ChatMessage.Message}");
        };

        client.OnChatCommandReceived += (sender, args) =>
        {
            if (args.Command.CommandText == "hello")
            {
                client.SendMessage(args.Command.ChatMessage.Channel, $"Hello {args.Command.ChatMessage.DisplayName}!");
            }
            else if (args.Command.CommandText == "about")
            {
                client.SendMessage(args.Command.ChatMessage.Channel, "I am a Twitch bot running on TwitchLib!");
            }
            else
            {
                client.SendMessage(args.Command.ChatMessage.Channel, $"Unknown chat command: {args.Command.CommandIdentifier}{args.Command.CommandText}");
            }
        };

        client.Connect();
    }

    void OnDisable()
    {
        client.Disconnect();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.SendMessage(
                channel: Secrets.TWITCH_USERNAME, 
                message: "I pressed the space key within Unity."
            );
        }
    }
}
