# Guide: Setting up CI/CD for C# Projects using GitHub Actions

This guide walks you step-by-step through configuring a **CI/CD pipeline** (Continuous Integration) for your C# console application using GitHub Actions. This automated process ensures that every time you push code or open a pull request, your project compiles and all unit tests run automatically.

---

## 1. Creating the Workflow File

In the root directory of your repository, create the folder structure and file named exactly as follows:

```text
.github/
└── workflows/
    └── build-and-test.yml
```

---

## 2. Configuration File (`build-and-test.yml`)

Copy and paste the following content into your `build-and-test.yml` file:

```yaml
name: Build and Test C# Application

# Trigger the workflow on push or pull request events for specified branches
on:
  push:
    branches: [ "main", "master" ]
  pull_request:
    branches: [ "main", "master" ]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    # 1. Checkout the project source code
    - name: Checkout repository
      uses: actions/checkout@v4

    # 2. Set up the .NET SDK environment
    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x' # Adjust to match the .NET version used in your project

    # 3. Restore NuGet packages and dependencies
    - name: Restore dependencies
      run: dotnet restore

    # 4. Build the project in Release configuration
    - name: Build
      run: dotnet build --configuration Release --no-restore

    # 5. Run unit tests
    - name: Test
      run: dotnet test --configuration Release --no-build --verbosity normal
```

---

## 3. Explanation of Key Components

| Workflow Component | Description and Purpose |
| :--- | :--- |
| **`on`** | Defines the triggers for the workflow. In this case, it runs on any push or pull request to the `main` or `master` branches. |
| **`actions/checkout@v4`** | Fetches your repository code into the virtual environment (runner) where the workflow executes. |
| **`actions/setup-dotnet@v4`** | Installs the specified .NET SDK version. **Be sure to update `dotnet-version` to match your project target (e.g., `8.0.x` or `9.0.x`).** |
| **`dotnet restore`** | Downloads all required NuGet packages and dependencies defined in your project files. |
| **`dotnet build`** | Compiles the project. The `--no-restore` flag speeds up execution by skipping package restoration already completed. |
| **`dotnet test`** | Automatically discovers and runs all unit test projects (xUnit, NUnit, MSTest) within your solution. |

---

## 4. Verifying the Pipeline

1. Commit and push the `.github/workflows/build-and-test.yml` file to your GitHub repository:
   ```bash
   git add .github/workflows/build-and-test.yml
   git commit -m "ci: add GitHub Actions workflow for build and test"
   git push
   ```
2. Navigate to the **Actions** tab in your GitHub repository interface.
3. Select the running workflow to monitor progress. A green checkmark indicates that both compilation and tests succeeded.