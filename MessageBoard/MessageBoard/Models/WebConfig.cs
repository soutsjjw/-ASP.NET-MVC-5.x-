namespace MessageBoard.Models
{
    public class WebConfig
    {
        public WebConfig()
        {
            Pagination = new PaginationContent();
            MailServer = new MailServerContent();
            Jwt = new JwtContent();
            DateTimeFormat = new DateTimeFormatContent();
        }

        public PaginationContent Pagination { get; set; }
        public MailServerContent MailServer { get; set; }
        public JwtContent Jwt { get; set; }
        public DateTimeFormatContent DateTimeFormat { get; set; }

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

        public class JwtContent
        {
            public string SecretKey { get; set; }
            public int ExpireMinutes { get; set; }
            public string CookieName { get; set; }
        }

        public class DateTimeFormatContent
        {
            public string DateTime { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
        }
    }
}
