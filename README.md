# Permission API

API RESTful para solicitar, modificar y consultar permisos de empleados.

## Tecnologías
- .NET 8
- MediatR
- Serilog
- Elasticsearch
- Kafka (Producer)
- Docker

## Endpoints

<img width="1799" height="535" alt="image" src="https://github.com/user-attachments/assets/32afe2e7-601a-4c37-9c63-552855e37c48" />


## Docker - Compose
```yaml
services:

  kafka:
    image: apache/kafka:latest
    container_name: kafka
    ports:
      - "9092:9092"
      - "9093:9093"
      - "29092:29092"
    environment:
      KAFKA_NODE_ID: 1
      KAFKA_PROCESS_ROLES: broker,controller
      KAFKA_LISTENERS: PLAINTEXT://0.0.0.0:9092,EXTERNAL://0.0.0.0:29092,CONTROLLER://0.0.0.0:9093
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9092,EXTERNAL://localhost:29092
      KAFKA_CONTROLLER_QUORUM_VOTERS: 1@localhost:9093
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: CONTROLLER:PLAINTEXT,PLAINTEXT:PLAINTEXT,EXTERNAL:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
      KAFKA_CONTROLLER_LISTENER_NAMES: CONTROLLER
      KAFKA_LOG_DIRS: /var/lib/kafka/data
      KAFKA_AUTO_CREATE_TOPICS_ENABLE: "true"
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_LOG_RETENTION_HOURS: 168
      KAFKA_GROUP_INITIAL_REBALANCE_DELAY_MS: 0
      CLUSTER_ID: "Mk3OEYBSD34fcwNTJENDM2Qk"
    volumes:
      - ./data:/var/lib/kafka/data
      
      
  kafka-ui:
    image: provectuslabs/kafka-ui:latest
    ports:
      - "8080:8080"
    environment:
      KAFKA_CLUSTERS_0_NAME: "Local"
      KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS: "kafka:9092"
 

  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.13.4
    container_name: elasticsearch
    environment:
      - discovery.type=single-node
      - xpack.security.enabled=true
      - xpack.security.transport.ssl.enabled=false
      - ELASTIC_USERNAME=elastic
      - ELASTIC_PASSWORD=fisuncp
    ports:
      - "9200:9200"
    volumes:
      - ./esdata:/usr/share/elasticsearch/data
    restart: unless-stopped

```
## ✅ 6. Buenas prácticas (estándares)

- ✅ **Separación de capas**: Application, Core, Infrastructure, WebApi
- ✅ **Tests unitarios** en carpeta dedicada
- ✅ **Logs y errores gestionados con Serilog o ILogger**
- ✅ **Indexación a Elasticsearch debe estar encapsulada en repositorio**
- ✅ **Kafka**
- ✅ **Docker listos para levantar servicios externos (Kafka, Elastic)**
