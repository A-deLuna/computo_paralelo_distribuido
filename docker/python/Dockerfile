FROM python
COPY requirements.txt /
RUN pip install -r requirements.txt
COPY main.py /
ENTRYPOINT python main.py
