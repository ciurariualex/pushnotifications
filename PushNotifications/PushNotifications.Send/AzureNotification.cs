using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Azure.NotificationHubs;

namespace PushNotifications.Send
{
    public class AzureNotification
    {
        public static AzureNotification Instance = new AzureNotification();

        public NotificationHubClient Hub { get; set; }

        private AzureNotification()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://NotificationHubForVoIPTelio.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=TxRLUvqNlgTYUqPRmbqZ1xmTMD3Da+zyaI9yopRr5xc=", "https://NotificationHubForVoIPTelio.servicebus.windows.net:443/");
        }
    }
}
