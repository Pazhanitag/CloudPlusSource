using System;

namespace CloudPlus.Constants
{
    public static class NotificationServiceConstants
    {
        public static string RabbitMqUri = System.Configuration.ConfigurationManager.AppSettings["RabbitMqUri"];
        public static string CompensateSuffix = "compensate";

        // Events
        public static string RoutingSlipEventObserverRoute = "routing-slip-event-observer";
        public static Uri RoutingSlipEventObserverUri = new Uri($"{RabbitMqUri}{RoutingSlipEventObserverRoute}");

        // Send Email Notification
        public static string QueueSendEmailNotification = "send-email-notification_flow";
        public static Uri SendEmailNotificationUri => new Uri($"{RabbitMqUri}{QueueSendEmailNotification}");
    }
}
