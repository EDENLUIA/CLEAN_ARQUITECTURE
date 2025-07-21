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


## Ejecución local
```bash
docker-compose up -d
dotnet run --project Audit.WebApi
Autor
Eden Luis Ancieta

yaml
Copiar
Editar

---

## ✅ 6. Buenas prácticas (estándares)

- ✅ **Commits limpios**: escribe mensajes claros (`feat:`, `fix:`, `test:`)
  - `feat: agregar endpoint para modificar permisos`
- ✅ **Separación de capas**: Application, Domain, Infrastructure, WebApi
- ✅ **Tests unitarios en carpeta dedicada** (`Audit.Tests`)
- ✅ **Logs y errores gestionados con Serilog o ILogger**
- ✅ **Indexación a Elasticsearch debe estar encapsulada en repositorio**
- ✅ **Docker listos para levantar servicios externos (Kafka, Elastic)**
