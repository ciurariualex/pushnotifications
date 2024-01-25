using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using NAudio.Wave; 

class Program
{
    static WaveInEvent waveSource = null;
    static MemoryStream audioStream = new MemoryStream();

    private static void StartRecording(IWebSocketConnection socket)
    {
        waveSource = new WaveInEvent();
        waveSource.WaveFormat = new WaveFormat(44100, 1);

        waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(WaveSource_DataAvailable);
        waveSource.RecordingStopped += (sender, e) => WaveSource_RecordingStopped(sender, e, socket);

        waveSource.StartRecording();
        Task.Delay(5000).ContinueWith(t => waveSource.StopRecording());
    }


    private static void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
    {
        audioStream.Write(e.Buffer, 0, e.BytesRecorded);
    }

    private static void WaveSource_RecordingStopped(object sender, StoppedEventArgs e, IWebSocketConnection socket)
    {
        var waveFormat = waveSource.WaveFormat;

        if (waveSource != null)
        {
            waveSource.Dispose();
            waveSource = null;
        }

        string tempFilePath = Path.GetTempFileName();
        SaveAsWav(audioStream, tempFilePath, waveFormat);

        SendWavFileAsync(socket, tempFilePath).Wait();

        audioStream.SetLength(0);
        File.Delete(tempFilePath); 
    }

    private static void SaveAsWav(MemoryStream input, string outputFilePath, WaveFormat format)
    {
        input.Seek(0, SeekOrigin.Begin);
        using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
        {
            WaveFileWriter.WriteWavFileToStream(fileStream, new RawSourceWaveStream(input, format));
        }
    }

    private static async Task SendAudioStreamAsync(IWebSocketConnection socket, MemoryStream stream)
    {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        await socket.Send(buffer);
    }

    static async Task Main(string[] args)
    {
        var server = new WebSocketServer("ws://0.0.0.0:8001");

        server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                Console.WriteLine("Connection opened");
            };
            socket.OnClose = () => Console.WriteLine("Connection closed");
            socket.OnBinary = async (bytes) =>
            {
                Console.WriteLine("Mesaj primit: " + Encoding.UTF8.GetString(bytes));
                Console.WriteLine("Trimitem datele!");
                SendWavFileAsync(socket, "test.wav").Wait();
            };
            socket.OnMessage = async (message) =>
            {
                Console.WriteLine("Mesaj primit: " + message);
                if (message == "play_audio")
                {
                    SendWavFileAsync(socket, "test.wav").Wait();
                } 
                else if (message == "live")
                {
                    StartRecording(socket);
                } else if (message == "play_video")
                {
                    SendVideoFileAsync(socket, "test.mp4").Wait();
                }
            };
        });

        Console.WriteLine("Server started on ws://localhost:8001");
        Console.ReadLine();
    }

    private static async Task SendWavFileAsync(IWebSocketConnection socket, string filePath)
    {
        Console.WriteLine("Trimitem datele!");
        byte[] audioData = File.ReadAllBytes(filePath);
        Console.WriteLine(audioData.Length);
        await socket.Send(audioData);
    }

    private static async Task SendVideoFileAsync(IWebSocketConnection socket, string filePath)
    {
        Console.WriteLine("Trimitem video!");

        byte[] videoData = File.ReadAllBytes(filePath);
        await socket.Send(videoData);
    }
}
