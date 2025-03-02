# RAG-based Chatbot

This is a RAG (Retrieval-Augmented Generation) based chatbot that answers questions by retrieving relevant information from stored PDF documents.

## Features

- PDF document processing and vectorization
- Efficient similarity search using FAISS
- OpenAI GPT-4 integration for natural language responses
- FastAPI-based REST API
- Redis caching for improved performance
- Async request handling
- Comprehensive error handling and logging

## Setup

1. Create a virtual environment and activate it:
```bash
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
```

2. Install dependencies:
```bash
pip install -r requirements.txt
```

3. Create a `.env` file with your OpenAI API key:
```
OPENAI_API_KEY=your_api_key_here
```

4. Install and start Redis server (for caching)

## Usage

1. Start the server:
```bash
python main.py
```

2. Process PDF documents:
```bash
curl --location 'http://localhost:8000/process-pdfs' \
--header 'Content-Type: application/json' \
--data '{
    "directory_path": ".\\pdfs"
}'

curl -X POST "http://localhost:8000/process-pdfs" \
     -H "Content-Type: application/json" \
     -d '{"directory_path": "path/to/pdf/directory"}'
```

3. Ask questions:
```bash
curl --location 'http://localhost:8000/ask' \
--header 'Content-Type: application/json' \
--data '{
    "text": "Sử dụng các số 1, 1, 3 và 6 và bốn toán tử cơ bản +, -, /,*, tạo ra một biểu thức để đạt kết quả chính xác bằng 24."
}'

curl -X POST "http://localhost:8000/ask" \
     -H "Content-Type: application/json" \
     -d '{"text": "your question here"}'
```

## API Endpoints

- `POST /process-pdfs`: Process PDF documents and create vector store
- `POST /ask`: Ask a question and get an answer based on the processed documents

## Architecture

The system consists of several components:
- Document Processor: Handles PDF loading and text processing
- RAG Engine: Manages vector store and question-answering
- FastAPI Application: Provides REST API endpoints
- Redis Cache: Caches responses for improved performance

## Error Handling

The system includes comprehensive error handling and logging:
- Input validation using Pydantic models
- Exception handling for PDF processing and question-answering
- Detailed logging for debugging and monitoring

## Performance Optimization

- Async request handling for improved concurrency
- Redis caching to speed up repeated queries
- FAISS vector store for efficient similarity search
- Configurable chunk size and overlap for text splitting 