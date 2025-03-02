import os
from typing import List
from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_core.documents import Document
from langchain_openai import OpenAIEmbeddings
from langchain_community.vectorstores import FAISS
from config import get_settings
from langchain.document_loaders import DirectoryLoader
from dotenv import load_dotenv, find_dotenv

settings = get_settings()

load_dotenv(find_dotenv())


class DocumentProcessor:
    def __init__(self):
        self.text_splitter = RecursiveCharacterTextSplitter(
            chunk_size=settings.chunk_size,
            chunk_overlap=settings.chunk_overlap
        )
        self.embeddings = OpenAIEmbeddings(
            api_key=settings.openai_api_key
        )
    
    def process_directory(self, directory_path: str) -> List[Document]:
        """Process all PDF files in a directory."""
        loader = DirectoryLoader(
            directory_path,
            glob="*.pdf",  # Use ** to match files in subdirectories on Windows
            loader_cls=PyPDFLoader,
            show_progress=True,
            use_multithreading=True  # Better performance on Windows
        )
        documents = loader.load()
        return self.text_splitter.split_documents(documents)

    def create_vector_store(self, chunks: List[str]) -> FAISS:
        """Create and save FAISS vector store from text chunks."""
        vector_store = FAISS.from_documents(chunks, self.embeddings)
        vector_store.save_local(settings.vector_store_path)
        return vector_store
    
    @staticmethod
    def load_vector_store() -> FAISS:
        """Load existing FAISS vector store."""
        # Check if vector store exists
        if not os.path.exists(settings.vector_store_path):
            os.makedirs(settings.vector_store_path)
            # Create an empty vector store if none exists
            empty_store = FAISS.from_texts([""], OpenAIEmbeddings(api_key=settings.openai_api_key))
            empty_store.save_local(settings.vector_store_path)
            
        embeddings = OpenAIEmbeddings(api_key=settings.openai_api_key)
        return FAISS.load_local(settings.vector_store_path, embeddings, allow_dangerous_deserialization=True)
