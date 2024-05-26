using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class SpotifyBot
{
    private static readonly string TelegramBotToken = "7166757164:AAFHVOd-0kxhzKle2eOgljwdaYUXP_Hkf1Y";
    private static readonly string Endpoint = "https://localhost:7099/SpotifyAuth";
    private static readonly HttpClient HttpClient = new HttpClient();
    private static readonly TelegramBotClient Bot = new TelegramBotClient(TelegramBotToken);
    private static readonly Dictionary<long, string> UserTokens = new Dictionary<long, string>();
    private static readonly Dictionary<long, bool> UserAuthorizationState = new Dictionary<long, bool>();

    public static async Task Main()
    {
        var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );

        Console.WriteLine("Bot is working(maybe)");
        Console.ReadKey();
        cts.Cancel();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text;

        if (messageText == "/start")
        {
            var authorizationUrl = $"https://accounts.spotify.com/authorize?client_id=e3d3dc11e6de46e3bf8e69503bb38f66&response_type=code&redirect_uri=https://localhost:7099/Auth/callback&scope=user-read-private%20user-read-email%20playlist-modify-public%20playlist-modify-private";
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Please authorize Spotify: [Authorize]({authorizationUrl})",
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken,
                replyMarkup: GetMainMenu()
            );

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Please send me the authorization code after you have authorized.",
                cancellationToken: cancellationToken,
                replyMarkup: GetMainMenu()
            );
            UserAuthorizationState[chatId] = true;
        }
        else if (messageText == "/username")
        {
            if (UserTokens.TryGetValue(chatId, out var accessToken))
            {
                var response = await HttpClient.GetAsync($"https://localhost:7099/SpotifyAuth/me?accessToken={accessToken}");

                if (response.IsSuccessStatusCode)
                {
                    var userProfile = await response.Content.ReadAsStringAsync();
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"User Profile:\n{userProfile}",
                        cancellationToken: cancellationToken,
                        replyMarkup: GetMainMenu()
                    );
                }
                else
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"Failed to get user profile. Error: {errorString}",
                        cancellationToken: cancellationToken,
                        replyMarkup: GetMainMenu()
                    );
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Access token not found. Please authorize first.",
                    cancellationToken: cancellationToken,
                    replyMarkup: GetMainMenu()
                );
            }
        }
        else
        {
            if (UserAuthorizationState.TryGetValue(chatId, out var isAwaitingCode) && isAwaitingCode)
            {
                var code = messageText;

                var response = await HttpClient.GetAsync($"{Endpoint}/callback?code={code}&state=optional_state_parameter");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JObject.Parse(responseString);
                    var accessToken = responseObject.access_token;

                    UserTokens[chatId] = accessToken;
                    UserAuthorizationState[chatId] = false;

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Access received",
                        cancellationToken: cancellationToken,
                        replyMarkup: GetMainMenu()
                    );
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Failed to get access token. Please try again.",
                        cancellationToken: cancellationToken,
                        replyMarkup: GetMainMenu()
                    );
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Unknown command. Please use /start to authorize.",
                    cancellationToken: cancellationToken,
                    replyMarkup: GetMainMenu()
                );
            }
        }
    }

    private static IReplyMarkup GetMainMenu()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton("/start"),
            new KeyboardButton("/username")
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}
