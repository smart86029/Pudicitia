# Architecture

## Vertical

- Client
- API Gateway
- Microservices

### Client

- Frontend project

### API Gateway

- Authentication
- Aggregate data from multiple microservices
- Caching
- Service discovery

### Microservices

- Implement business logic
- Separate bounded context
- Disallow communication between microservices
- Using message queue with a pub/sub model, asynchronous receive events to sync data
