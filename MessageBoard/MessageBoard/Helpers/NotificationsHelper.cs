using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Helpers
{
    public class NotificationsHelper
    {
        private readonly string Notifications = "Notifications";

        public static string RenderNotifications()
        {
            if (!HttpContextHelper.Current.Session.Keys.Contains(nameof(Notifications))) return null;
            string jsBodyOpen = "<script>$(function() {";
            string jsBody = "";
            string jsBodyClose = "});</script>";
            
            var notifications = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notification>>(Get());
            foreach (var note in notifications)
            {
                if (note.NotificationType == NotificationType.Error)
                {
                    jsBody += $"toastr.error('{note.Message}', '{note.Title}')";
                }
                else if (note.NotificationType == NotificationType.Success)
                {
                    jsBody += $"toastr.success('{note.Message}', '{note.Title}')";
                }
            }
            Clear();
            return string.Join(" ", new string[] { jsBodyOpen, jsBody, jsBodyClose });
        }

        /// <summary>
        /// Clears session["Notifications"] object
        /// </summary>
        public static void Clear()
        {
            HttpContextHelper.Current.Session.Remove(nameof(Notifications));
        }

        public static void AddNotification(Notification notification)
        {
            var note = new Notification()
            {
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                Title = notification.Title
            };
            var notifications = new List<Notification>();

            if (HttpContextHelper.Current.Session.Keys.Contains(nameof(Notifications)))
            {
                notifications = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notification>>(Get());
            }
            notifications.Add(note);

            Set(JsonConvert.SerializeObject(notifications));
        }

        private static string Get()
        {
            if (!HttpContextHelper.Current.Session.Keys.Contains(nameof(Notifications)))
                return null;
            return HttpContextHelper.Current.Session.GetString(nameof(Notifications));
        }

        private static void Set(string notification)
        {
            HttpContextHelper.Current.Session.SetString(nameof(Notifications), notification);
        }

        public enum NotificationType
        {
            Success,
            Error,
            Warning,
            Notice
        }

        public class Notification
        {
            public string Message { get; set; }
            public NotificationType NotificationType { get; set; } = NotificationType.Success;
            public string Title { get; set; }
        }
    }
}
