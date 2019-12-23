using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace FluentCommands.Builders
{
    //: describe class, add method documentation for public members
    [Obsolete("Phasing this out...")]
    public class ClientBuilder
    {
        private readonly string _token = "";
        private readonly TelegramBotClient? _clientStorage;
        private System.Net.Http.HttpClient? _httpClient;
        private System.Net.IWebProxy? _webProxy;

        private ClientBuilder(string token) => _token = token;
        private ClientBuilder(TelegramBotClient client) => _clientStorage = client;

        public static ClientBuilder Create(string token) => new ClientBuilder(token);
        public static ClientBuilder WithHttpClient(string token, System.Net.Http.HttpClient httpClient) => new ClientBuilder(token) { _httpClient = httpClient };
        public static ClientBuilder WithWebProxy(string token, System.Net.IWebProxy webProxy) => new ClientBuilder(token) { _webProxy = webProxy };

        public static implicit operator ClientBuilder(string token) => new ClientBuilder(token);
        public static implicit operator ClientBuilder(TelegramBotClient client) => new ClientBuilder(client);

        internal TelegramBotClient Build()
        {
            if (_clientStorage is { }) return _clientStorage;

            if (_httpClient is { }) return new TelegramBotClient(_token, _httpClient);
            else if (_webProxy is { }) return new TelegramBotClient(_token, _webProxy);
            else return new TelegramBotClient(_token);
        }
    }
}
