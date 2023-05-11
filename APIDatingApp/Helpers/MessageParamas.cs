namespace APIDatingApp.Helpers
{
    public class MessageParamas : PaginationParams
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}