using Microsoft.AspNetCore.Mvc;
using PushNotifications.Send;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PushNotificationFirebaseController : ControllerBase
    {
        private readonly FireBaseNotification _notificationSend;

        public PushNotificationFirebaseController(FireBaseNotification notificationSend)
        {
            _notificationSend = notificationSend;
        }

        [HttpGet(Name = "SendNotification")]
        public string SendNotification()
        {
            var notificationResponse = _notificationSend.SendNotification();

            return notificationResponse;
        }
    }
}
