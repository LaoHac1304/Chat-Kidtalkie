using ChatKid.PushNotification.Notification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.PushNotification.Notification
{
    public interface IPushNotificationService
    {
        public Task<ResponseModel> PushNotification(NotificationModel notificationModel); 
    }
}
