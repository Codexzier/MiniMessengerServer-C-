namespace MiniMessengerServer.Data
{
    public class Response
    {
        public bool Success { get; set; }
        public object Content { get; set; }

        internal static Response CreateFail(string message) => new Response { Success = false, Content = message };
    }
}
