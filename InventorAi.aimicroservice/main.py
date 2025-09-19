from fastapi import FastAPI

app = FastAPI()

@app.get("/")
def home():
    return {"message": "AI Service Ready now"}

@app.get("/hello/{name1}")
def say_hello1(name1: str):
    return {"message": f"Hello, {name1}! Welcome to the AI POS system."}