using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace PushNotifications.Send
{
    public class FireBaseNotification
    {
        public FireBaseNotification()
        {
            AppOptions appOptions = new AppOptions()
            {
                Credential = GoogleCredential.FromFile("D:\\Work\\Assist\\PushNotifications\\Web\\private_key.json")
            };

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(appOptions);
            }
        }

        public string SendNotification()
        {
            var registrationToken = "dRzEFsykTESJ6qe9muRbhr:APA91bF9fKfsdCTnLc5S_DiKTXLDAc-HnIUE8uDTajQ8Otb3kOM3PqdAnYRrg3QHKqGhmo9qgw6FqLu_1-BoD4uMsJr6vu93P09vuMlcZLIywehWquVybFFfC-Rj9G9wO0mYxexhftpR";

            var message = new Message()
            {
                Data = new Dictionary<string, string>() { { "testData", "1" } },
                Token = registrationToken,
                Notification = new Notification()
                {
                    Title = "Test",
                    Body = "Test Body",
                }
            };

            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

            return response;
        }
    }
}
