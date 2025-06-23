using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class AwsCredentials
{
    public string awsAccessKey;
    public string awsSecretKey;
}

public class TextToSpeechOpenAI : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private string awsAccessKey;
    private string awsSecretKey;

    private void Awake()
    {
        LoadAwsCredentials();
    }

    private void LoadAwsCredentials()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "aws_credentials.json");

#if UNITY_ANDROID && !UNITY_EDITOR
        // Android (Quest) 读取 StreamingAssets 用 UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Get(path);
        request.SendWebRequest();
        while (!request.isDone) { }

        if (string.IsNullOrEmpty(request.error))
        {
            AwsCredentials creds = JsonUtility.FromJson<AwsCredentials>(request.downloadHandler.text);
            awsAccessKey = creds.awsAccessKey;
            awsSecretKey = creds.awsSecretKey;
        }
        else
        {
            Debug.LogError("Failed to load AWS credentials on Android: " + request.error);
        }
#else
        // Editor 和 PC/macOS 可直接读取文件
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            AwsCredentials creds = JsonUtility.FromJson<AwsCredentials>(json);
            awsAccessKey = creds.awsAccessKey;
            awsSecretKey = creds.awsSecretKey;
        }
        else
        {
            Debug.LogError("AWS credentials file not found: " + path);
        }
#endif
    }

    public void PlayResponseAudio(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            StartCoroutine(PlayAudioCoroutine(text));
        }
    }

    private IEnumerator PlayAudioCoroutine(string message)
    {
        // Polly 语音合成请求
        Task synthesizeTask = MakeAudioRequest(message);
        yield return new WaitUntil(() => synthesizeTask.IsCompleted);

        // 播放音频
        using (var webRequest = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var operation = webRequest.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }

            var clip = DownloadHandlerAudioClip.GetContent(webRequest);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private async Task MakeAudioRequest(string message)
    {
        var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = message,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Ruth,
            OutputFormat = OutputFormat.Mp3,
        };

        var response = await client.SynthesizeSpeechAsync(request);
        WriteIntoFile(response.AudioStream);
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
