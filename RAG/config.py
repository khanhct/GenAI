from pydantic import BaseModel
from functools import lru_cache
import os
from dotenv import load_dotenv

load_dotenv()


class Settings(BaseModel):
    # OpenAI settings
    openai_api_key: str = os.getenv("OPENAI_API_KEY", "")

    # Vector store settings
    vector_store_path: str = ".\\vector-db\\faiss_index"

    # Document processing settings
    chunk_size: int = 500
    chunk_overlap: int = 50

    # Redis settings (for caching)
    redis_host: str = "localhost"
    redis_port: int = 6379
    redis_db: int = 0

    model_config = {
        'env_file': '.env'
    }


@lru_cache()
def get_settings():
    return Settings()
