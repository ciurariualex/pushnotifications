package com.example.pushnotificationsandroidapp

import android.content.ContentValues.TAG
import android.os.Bundle
import android.text.TextUtils
import android.util.Log
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import com.example.pushnotificationsandroidapp.ui.theme.PushNotificationsAndroidAppTheme
import com.google.android.gms.tasks.Task
import com.google.firebase.messaging.FirebaseMessaging
import com.microsoft.windowsazure.messaging.notificationhubs.NotificationHub


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            PushNotificationsAndroidAppTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Greeting("Android")
                }
            }
        }

        NotificationHub.setListener(CustomNotificationListener())
        NotificationHub.start(this.application, "NotificationHubForVoIPTelio", "Endpoint=sb://NotificationHubForVoIPTelio.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=TxRLUvqNlgTYUqPRmbqZ1xmTMD3Da+zyaI9yopRr5xc=")
    }
}

@Composable
fun Greeting(name: String, modifier: Modifier = Modifier) {
    FirebaseMessaging.getInstance().token.addOnSuccessListener { token: String ->
        if (!TextUtils.isEmpty(token)) {
            Log.d(TAG, "retrieve token successful : $token")
        } else {
            Log.w(TAG, "token should not be null...")
        }
    }.addOnFailureListener { e: Exception? -> }.addOnCanceledListener {}
        .addOnCompleteListener { task: Task<String> ->
            Log.v(
                TAG,
                "This is the token : " + task.result
            )
        }
    Text(
        text = "Hello $name!",
        modifier = modifier
    )
}

@Preview(showBackground = true)
@Composable
fun GreetingPreview() {
    PushNotificationsAndroidAppTheme {
        Greeting("Android")
    }
}