FROM rabbitmq:3.8-management

COPY ./Broker/definitions.json /etc/rabbitmq/definitions.json

# healthcheck
RUN apt-get update
RUN apt-get install -y curl 

# Rabbit Plugins
RUN rabbitmq-plugins enable rabbitmq_shovel rabbitmq_shovel_management