namespace MiniMessengerServer
{
    internal class CommandItem
    {
        private CommandGetData _messages;

        public CommandItem(CommandGetData messages) => this._messages = messages;
    }
}