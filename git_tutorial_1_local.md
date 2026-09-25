# Local Git Tutorial: Version Control Fundamentals

This tutorial guides you through using Git locally on your machine—without needing GitHub or GitLab. You will learn how to track your C# Calculator project, manage revisions, work with branches, and undo mistakes.

---

## Prerequisites

Open your terminal or command prompt and ensure Git is installed:

```bash
git --version
```

Navigate into your C# Calculator project directory:

```bash
cd path/to/ConsoleCalculator
```

---

## 1. Local Configuration

Before making commits, configure your name and email address. Using the `--local` flag applies these settings only to the current repository.

```bash
# Set your name and email for this local repository
git config --local user.name "Your Name"
git config --local user.email "your.email@example.com"

# Verify your configuration
git config --local --list
```

---

## 2. Initializing a Repository

Turn your project folder into a Git-tracked repository:

```bash
git init
```

This creates a hidden `.git` folder that stores version history.

---

## 3. Ignoring Unnecessary Files (`.gitignore`)

.NET projects build output folders (`bin/`, `obj/`) that should **not** be tracked in version control. Create a `.gitignore` file to skip them:

```bash
# On Linux/macOS:
dotnet new gitignore

# Or manually create a .gitignore file with these entries:
```

Content for `.gitignore`:
```text
bin/
obj/
.vs/
*.user
```

---

## 4. Staging and Committing initial Code

### Step 4.1: Check Status
Check which files Git detects:

```bash
git status
```

### Step 4.2: Stage Files
Move your project files to the **Staging Area** (Index):

```bash
# Stage specific files
git add Program.cs README.md .gitignore

# Or stage all tracked/untracked changes in current directory
git add .
```

### Step 4.3: Commit
Save the staged snapshot into the repository history with a descriptive message:

```bash
git commit -m "Initial commit: Add C# console calculator base version"
```

---

## 5. Reviewing History and Inspecting Changes

### View Commit History
To see all recorded commits:

```bash
# Standard history list
git log

# One-line compact history
git log --oneline --graph --all
```

### Make a Change and Inspect Differences
Open `Program.cs` and edit a line (e.g., update the application header text).

Check status and inspect exact changes:

```bash
# See modified files
git status

# View line-by-line differences before staging
git diff
```

Commit the updated change:

```bash
git add Program.cs
git commit -m "Update application header message"
```

---

## 6. Branching: Working on Features Safely

Branches allow you to implement new features (like adding trigonometric functions) without breaking the stable main code.

### Step 6.1: Create and Switch to a Feature Branch

```bash
# Create and switch to a new branch named 'feature/trigonometry'
git checkout -b feature/trigonometry

# (On newer Git versions, you can also use:)
# git switch -c feature/trigonometry
```

Verify your active branch:

```bash
git branch
```

### Step 6.2: Implement the Feature and Commit
Open `Program.cs` and enable trigonometric functions inside `RegisterBuiltInFunctions()`:

```csharp
RegisterFunction("sin", args => Math.Sin(args[0] * Math.PI / 180.0));
RegisterFunction("cos", args => Math.Cos(args[0] * Math.PI / 180.0));
```

Check changes and commit on the feature branch:

```bash
git diff
git add Program.cs
git commit -m "Add sin and cos functions to calculator"
```

---

## 7. Merging Feature Branches

Once your feature is complete and tested, merge it back into the `main` (or `master`) branch.

```bash
# 1. Switch back to main branch
git checkout main

# 2. Merge feature branch into main
git merge feature/trigonometry

# 3. View the combined history
git log --oneline --graph
```

Optionally delete the feature branch after merging:

```bash
git branch -d feature/trigonometry
```

---

## 8. Undoing Changes and Fixing Mistakes

Git provides several ways to undo mistakes depending on where you are in your workflow.

### Scenario A: Discard Unstaged Local Changes
If you modified `Program.cs` but haven't staged it yet and want to throw away changes:

```bash
git restore Program.cs
```

### Scenario B: Unstage a File
If you ran `git add Program.cs` by mistake but haven't committed yet:

```bash
git restore --staged Program.cs
```

### Scenario C: Fix the Very Last Commit Message or Content
If you forgot a file or made a typo in the last commit message:

```bash
# Make your file changes, stage them, and amend the previous commit:
git add Program.cs
git commit --amend -m "Corrected commit message"
```

### Scenario D: Revert a Commit (Safe Undo)
Creates a **new commit** that reverses the changes of a previous commit:

```bash
# Revert the latest commit
git revert HEAD
```

### Scenario E: Reset History (Soft vs. Hard)

> ⚠️ **Caution:** Reset alters commit history.

```bash
# Soft Reset: Move history back 1 commit, but keep your local file changes staged
git reset --soft HEAD~1

# Hard Reset: Move history back 1 commit and DESTROY all uncommitted local changes
git reset --hard HEAD~1
```

---

## 💡 Quick Reference Summary

| Task | Command |
| :--- | :--- |
| **Initialize Repo** | `git init` |
| **Check Status** | `git status` |
| **Stage Changes** | `git add <file>` |
| **Commit Changes** | `git commit -m "Message"` |
| **View Log** | `git log --oneline` |
| **Create Branch** | `git checkout -b <branch-name>` |
| **Switch Branch** | `git checkout <branch-name>` |
| **Merge Branch** | `git merge <branch-name>` |
| **Discard Unstaged Changes** | `git restore <file>` |
| **Undo Last Commit (keep files)** | `git reset --soft HEAD~1` |