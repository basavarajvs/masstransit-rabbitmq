# MassTransit RabbitMQ PoC

This project is a simple Proof of Concept demonstrating a decoupled architecture using MassTransit and RabbitMQ.

## Use Case

The project simulates a common web application scenario: a user uploads an image, and a background worker processes it to generate a thumbnail. This is a perfect use case for a message queue to prevent blocking the user's request.

- **Producer**: A console application that simulates sending an `ImageToProcess` message.
- **Consumer**: A console application (worker service) that listens for `ImageToProcess` messages and 'processes' them.

## Prerequisites

- .NET 8 SDK
- Docker (or a running instance of RabbitMQ)

## Configuration

The RabbitMQ connection settings are stored in `appsettings.json` in both the `Producer` and `Consumer` projects. You can modify these files to match your RabbitMQ instance.

```json
{
  "RabbitMq": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

## How to Run

1.  **Start RabbitMQ:**

    The easiest way is with Docker:
    ```bash
    docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

2.  **Run the Consumer:**

    Open a terminal and run the consumer application:
    ```bash
    dotnet run --project Consumer/Consumer.csproj
    ```

3.  **Run the Producer:**

    Open a second terminal and run the producer application:
    ```bash
    dotnet run --project Producer/Producer.csproj
    ```

You will see the Consumer window log that it has received the message from the Producer.