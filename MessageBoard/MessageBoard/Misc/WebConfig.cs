namespace MessageBoard.Misc
{
    public class WebConfig
    {
        public WebConfig()
        {
            Pagination = new PaginationContent();
            MailServer = new MailServerContent();
        }

        public PaginationContent Pagination { get; set; }
        public MailServerContent MailServer { get; set; }

        public class PaginationContent
        {
            public int PageSize { get; set; }
            public string PageCountAndCurrentLocationFormat { get; set; }
        }

        public class MailServerContent
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
        }
    }
}
