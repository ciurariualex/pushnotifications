package com.example.pushnotificationsandroidapp

import android.app.Notification
import android.content.Context
import android.util.Log
import com.google.firebase.messaging.RemoteMessage
import com.microsoft.windowsazure.messaging.notificationhubs.NotificationListener


class CustomNotificationListener : NotificationListener {

    override fun onPushNotificationReceived(p0: Context?, p1: RemoteMessage?) {
        val x = "asd";
    }
}