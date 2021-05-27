FROM rabbitmq:3.8-management

COPY ./Broker/definitions.json /etc/rabbitmq/definitions.json

RUN apt-get update
RUN apt-get install -y curl 