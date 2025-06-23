from flask import Flask, request, jsonify
import time
import os
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

API_KEY = os.getenv("OPENAI_API_KEY")
ASSISTANT_ID = os.getenv("ASSISTANT_ID")

if not API_KEY:
    raise ValueError("API key not set properly. Please check your .env file.")

app = Flask(__name__)
client = OpenAI(api_key=API_KEY)

# Global variable to store thread ID
THREAD_ID = None

def get_openai_response(query):
    global THREAD_ID

    try:
        # If no thread ID exists, create a new thread
        if THREAD_ID is None:
            thread = client.beta.threads.create()
            THREAD_ID = thread.id
        else:
            thread = client.beta.threads.retrieve(thread_id=THREAD_ID)

        # Send user message to the thread
        client.beta.threads.messages.create(
            thread_id=THREAD_ID,
            role="user",
            content=query
        )

        # Run assistant
        run = client.beta.threads.runs.create(thread_id=THREAD_ID, assistant_id=ASSISTANT_ID)

        # Wait for the response to complete
        start_time = time.time()
        timeout = 30  # Maximum wait time: 30 seconds
        while run.status not in ["completed", "failed", "cancelled"]:
            if time.time() - start_time > timeout:
                return "Timeout: No response received from OpenAI"
            time.sleep(0.5)
            run = client.beta.threads.runs.retrieve(thread_id=THREAD_ID, run_id=run.id)

        if run.status != "completed":
            return "OpenAI API run failed"

        # Retrieve the latest message from the conversation
        messages = client.beta.threads.messages.list(thread_id=THREAD_ID)
        latest_message = messages.data[0] if messages.data else None

        if latest_message and latest_message.content:
            content = latest_message.content[0]
            if hasattr(content, 'text'):
                return content.text.value
            return "Unable to parse the response from OpenAI"

        return "No message received"

    except Exception as e:
        return f"Error: {str(e)}"

@app.route('/ask_openai', methods=['POST'])
def ask_openai():
    data = request.json
    query = data.get('query', '')

    if not query:
        return jsonify({'error': 'Query is empty'}), 400

    response = get_openai_response(query)
    return jsonify({'response': response})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, threaded=True)
