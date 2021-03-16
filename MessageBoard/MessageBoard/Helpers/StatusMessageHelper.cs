using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Helpers
{
    public class StatusMessageHelper
    {
        private readonly string StatusMessage = "StatusMessage";

        public static string RenderMessage()
        {
            if (!HttpContextHelper.Current.Session.Keys.Contains(nameof(StatusMessage))) return null;

            string alertContent = "";
            var alerts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Content>>(Get());
            var contentType = "success";
            var contentTitle = "";
            foreach (var alert in alerts)
            {
                switch (alert.ContentType)
                {
                    case ContentType.Danger:
                        contentType = "danger";
                        contentTitle = $"<h5><i class='icon fas fa-ban'></i> {alert.Title}</h5>";
                        break;
                    case ContentType.Info:
                        contentType = "info";
                        contentTitle = $"<h5><i class='icon fas fa-info'></i> {alert.Title}</h5>";
                        break;
                    case ContentType.Warning:
                        contentType = "warning";
                        contentTitle = $"<h5><i class='icon fas fa-exclamation-triangle'></i> {alert.Title}</h5>";
                        break;
                    default:
                        contentType = "success";
                        contentTitle = $"<h5><i class='icon fas fa-check'></i> {alert.Title}</h5>";
                        break;
                }
                contentTitle = string.IsNullOrWhiteSpace(alert.Title) ? "" : contentTitle;

                alertContent += $@"
<div class='alert alert-{contentType} alert-dismissible' role='alert'>
    <button type='button' class='close' data-dismiss='alert' aria-hidden='true'>×</button>
    {contentTitle}
    {alert.Message}
</div>";
            }
            Clear();
            return alertContent;
        }

        /// <summary>
        /// Clears session["Alerts"] object
        /// </summary>
        public static void Clear()
        {
            HttpContextHelper.Current.Session.Remove(nameof(StatusMessage));
        }

        public static void AddMessage(string title = "", string message = "", ContentType contentType = ContentType.Success)
        {
            var content = new Content()
            {
                Message = message,
                ContentType = contentType,
                Title = title
            };
            AddMessage(content);
        }

        public static void AddMessage(Content content)
        {
            var messages = new List<Content>();

            if (HttpContextHelper.Current.Session.Keys.Contains(nameof(StatusMessage)))
            {
                messages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Content>>(Get());
            }
            messages.Add(content);

            Set(JsonConvert.SerializeObject(messages));
        }

        private static string Get()
        {
            if (!HttpContextHelper.Current.Session.Keys.Contains(nameof(StatusMessage)))
                return null;
            return HttpContextHelper.Current.Session.GetString(nameof(StatusMessage));
        }

        private static void Set(string notification)
        {
            HttpContextHelper.Current.Session.SetString(nameof(StatusMessage), notification);
        }

        public enum ContentType
        {
            Danger,
            Info,
            Warning,
            Success
        }

        public class Content
        {
            public string Message { get; set; }
            public ContentType ContentType { get; set; } = ContentType.Success;
            public string Title { get; set; }
        }
    }
}
