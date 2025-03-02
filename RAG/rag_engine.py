from typing import Dict, Optional
from langchain.prompts import PromptTemplate
from langchain.chains import RetrievalQA
from langchain_openai import ChatOpenAI
from document_processor import DocumentProcessor
from config import get_settings

settings = get_settings()


def creat_prompt(t) -> PromptTemplate:
    return PromptTemplate(template=t, input_variables=["context", "question"])


template = """<|im_start|>system\nSử dụng thông tin sau đây để trả lời câu hỏi. Nếu bạn không biết câu trả lời, hãy nói không biết, đừng cố tạo ra câu trả lời\n
    {context}<|im_end|>\n<|im_start|>user\n{question}<|im_end|>\n<|im_start|>assistant"""
prompt = creat_prompt(template)


class RAGEngine:
    def __init__(self):
        # Initialize vector store
        self.vector_store = DocumentProcessor.load_vector_store()
        
        # Initialize LLM
        self.llm = ChatOpenAI(
            model_name="gpt-4",
            api_key=settings.openai_api_key,
            temperature=0.7
        )
        
        # Initialize retrieval chain
        self.qa_chain = RetrievalQA.from_chain_type(
            llm=self.llm,
            chain_type="stuff",
            retriever=self.vector_store.as_retriever(
                search_kwargs={"k": 3},
                max_tokens_limit=1024
            ),
            return_source_documents=False,
            chain_type_kwargs={'prompt': prompt}
        )
        
        # Initialize Redis cache
        # self.cache = redis.Redis(
        #     host=settings.redis_host,
        #     port=settings.redis_port,
        #     db=settings.redis_db
        # )
    
    async def get_answer(self, question: str) -> Dict[str, str]:
        """Get answer for a question using RAG."""
        try:
            # Check cache first
            # cache_key = f"qa:{question}"
            # cached_response = self.cache.get(cache_key)
            #
            # if cached_response:
            #     return json.loads(cached_response)
            
            # Get answer from LLM
            response = self.qa_chain.invoke(question)
            
            # Format response
            result = {
                "question": question,
                "answer": response["result"],
                "source": "rag"
            }
            
            # Cache the response
            # self.cache.setex(
            #     cache_key,
            #     3600,  # Cache for 1 hour
            #     json.dumps(result)
            # )
            
            return result
            
        except Exception as e:
            return {
                "question": question,
                "error": str(e),
                "source": "error"
            }