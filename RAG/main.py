import logging
from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel

from rag_engine import RAGEngine
from document_processor import DocumentProcessor

# Configure logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

# Initialize FastAPI app
app = FastAPI(
    title="RAG Chatbot API",
    description="A RAG-based chatbot that answers questions using PDF documents",
    version="1.0.0"
)

# Add CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Initialize RAG engine
rag_engine = RAGEngine()


class Question(BaseModel):
    text: str


class ProcessPDFRequest(BaseModel):
    directory_path: str


@app.post("/process-pdfs")
async def process_pdfs(request: ProcessPDFRequest):
    """Process PDFs and create/update vector store."""
    try:
        processor = DocumentProcessor()
        chunks = processor.process_directory(request.directory_path)
        processor.create_vector_store(chunks)

        return {"message": "PDFs processed successfully", "chunks_created": len(chunks)}
    except Exception as e:
        logger.error(f"Error processing PDFs: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/ask")
async def ask_question(question: Question):
    """Get answer for a question using RAG."""
    try:
        logger.info(f"Received question: {question.text}")
        response = await rag_engine.get_answer(question.text)
        logger.info(f"Generated response for question: {question.text}")
        return response
    except Exception as e:
        logger.error(f"Error processing question: {str(e)}")
        raise HTTPException(status_code=500, detail=str(e))

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
