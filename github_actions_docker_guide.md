# Guide: Building and Testing a .NET Application with Docker (Locally & GitHub Actions)

Containerization ensures that your application is built and tested in an identical environment on both your local development machine and the CI/CD server.

---

## 1. Preparing the Dockerfile

Create a file named `Dockerfile` (without any file extension) in the root directory of your repository. This setup uses a **multi-stage build** to compile, test, and package the application.

```dockerfile
# Stage 1: Build and Test Environment (Includes .NET SDK)
FROM [mcr.microsoft.com/dotnet/sdk:8.0](https://mcr.microsoft.com/dotnet/sdk:8.0) AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["CalculatorApp/CalculatorApp.csproj", "CalculatorApp/"]
COPY ["CalculatorApp.Tests/CalculatorApp.Tests.csproj", "CalculatorApp.Tests/"]
RUN dotnet restore "CalculatorApp/CalculatorApp.csproj"

# Copy remaining source code
COPY . .

# Build stage
RUN dotnet build "CalculatorApp/CalculatorApp.csproj" -c Release -o /app/build

# Stage 2: Run Tests during Build Process
FROM build AS testrunner
WORKDIR /src/CalculatorApp.Tests
RUN dotnet test --logger:trx

# Stage 3: Publish Application
FROM build AS publish
RUN dotnet publish "CalculatorApp/CalculatorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Runtime Image (Lightweight environment without SDK)
FROM [mcr.microsoft.com/dotnet/runtime:8.0](https://mcr.microsoft.com/dotnet/runtime:8.0) AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculatorApp.dll"]

```

> **Note:** Update directory and file names (`CalculatorApp`, `CalculatorApp.Tests`) according to your actual `.csproj` file locations.

---

## 2. Local Execution and Testing

Ensure **Docker Desktop** (or Podman) is running on your machine.

1. **Open a terminal** in the root directory of your project.
2. **Build the Docker image** (this executes compilation and unit tests):
```bash
docker build -t calculator-app .

```


*If any unit test fails, the Docker build process will immediately abort and display the error logs.*
3. **Run the container** (to test application functionality):
```bash
docker run --rm -it calculator-app

```



---

## 3. GitHub Actions Configuration

Create or update the file **`.github/workflows/docker-build.yml`** in your repository:

```yaml
name: Build and Test in Docker

on:
  push:
    branches: [ "main", "master" ]
  pull_request:
    branches: [ "main", "master" ]

jobs:
  docker-build-test:
    runs-on: ubuntu-latest

    steps:
    # 1. Checkout repository source code
    - name: Checkout repository
      uses: actions/checkout@v4

    # 2. Set up Docker Buildx
    - name: Set up Docker Buildx
      uses: docker/setup-buildx-action@v3

    # 3. Build image and execute tests inside container
    - name: Build Docker image and run tests
      run: |
        docker build -t calculator-app:test .

```

---

## Approach Comparison

| Approach | Advantages | Disadvantages |
| --- | --- | --- |
| **Direct Runner (.NET SDK)** | Faster execution on GitHub Actions (better dependency caching). | Requires matching .NET SDK installed both locally and on CI environments. |
| **Docker Container** | 100% identical environment locally and on GitHub; isolated from system dependencies. | Slightly slower builds due to Docker image layer handling. |

