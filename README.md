The unity project of my master's thesis at University of Aizu, including the python backend code.

I will renew this README.md for adding more information at the future, if you have any question, feel free to ask in the `Issues`!

## Requirements

- Unity 2022.3
- Python 3.13

## Need to add (Before Implement)
### For Unity:

1. `aws_credentials.json` file, for AWS polly

  - Path: `Assets/StreamingAssets/aws_credentials.json`
    ```
    {
      "awsAccessKey": "Your Access Key",
      "awsSecretKey": "Your Secret Key"
    }
    ```
2. Whisper Model Dowmload: [Hugging Face Link](https://huggingface.co/ggerganov/whisper.cpp/tree/main). I used `ggml-base.en.bin` in this project.

### For Python:

- `.env` file
```
OPENAI_API_KEY= Your OpenAI Key
GOOGLE_APPLICATION_CREDENTIALS=unity-tts-stt-cfeXXXXXdXbc.json
```

