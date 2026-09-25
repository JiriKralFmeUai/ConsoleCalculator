# Git Tutorial Part 2: GitHub Integration and Remote Synchronization

This tutorial covers connecting your existing local Git repository to **GitHub**. You will learn how to set up an account, push local code to a remote server, synchronize work across multiple machines, inspect remote changes, and resolve merge conflicts.

---

## 1. Setting Up a GitHub Account

If you do not have a GitHub account yet, follow these steps:

1. Go to [github.com](https://github.com/) and click **Sign up**.
2. Enter your email, create a password, and choose a unique username.
3. Complete the verification puzzle and click **Create account**.
4. Verify your email address using the code sent to your inbox.

---

## 2. Authentication: Personal Access Token (PAT) / SSH

When pushing code from your command line to GitHub, password authentication is disabled. You must use a **Personal Access Token (PAT)** or an **SSH Key**.

### Creating a Personal Access Token (PAT):
1. Log in to GitHub, click your profile picture in the top-right corner, and go to **Settings**.
2. Scroll down to **Developer settings** (at the bottom of the left sidebar).
3. Select **Personal access tokens** -> **Tokens (classic)**.
4. Click **Generate new token (classic)**.
5. Set a note (e.g., `ConsoleCalculator Token`), set an expiration date, and check the **`repo`** scope box.
6. Click **Generate token** and **copy the token immediately** (you will not be able to see it again). Use this token as your password when Git prompts you in the terminal.

---

## 3. Creating a Remote Repository on GitHub

1. Go to GitHub and click the **`+`** icon in the top right corner $\rightarrow$ **New repository**.
2. Set the repository name (e.g., `ConsoleCalculator`).
3. Set visibility to **Public** or **Private**.
4. ⚠️ **IMPORTANT:** Do **NOT** initialize the repository with a `README`, `.gitignore`, or `License`. Your local folder already contains these files! Creating them on GitHub will cause initial merge conflicts.
5. Click **Create repository**.

---

## 4. Connecting Your Local Git to GitHub (`origin`)

After creating the repository, GitHub displays commands to push an existing repository. Run them in your project directory:

```bash
# 1. Ensure your default branch is named 'main'
git branch -M main

# 2. Add the remote GitHub repository URL under the alias 'origin'
git remote add origin https://github.com/YOUR-USERNAME/ConsoleCalculator.git

# 3. Verify that the remote URL is correctly attached
git remote -v
```

### Pushing Local Commits to GitHub

Push your local `main` branch to GitHub and set it as the default upstream tracking branch (`-u`):

```bash
git push -u origin main
```

*(If prompted for credentials, enter your GitHub username and paste your Personal Access Token as the password.)*

---

## 5. Working Across Multiple Machines

### Scenario: Cloning the Repository on a Second Computer

To work on the project from a different computer:

```bash
git clone https://github.com/YOUR-USERNAME/ConsoleCalculator.git
cd ConsoleCalculator
```

---

## 6. Synchronizing Remote Changes

When working across multiple computers (e.g., school computer vs. home laptop), always synchronize before starting and after finishing your work.

### Step 6.1: Check for Remote Changes (`git fetch`)

To inspect what changed on GitHub without modifying your local working files:

```bash
# Fetch latest references from GitHub
git fetch origin

# Compare your local main branch with remote origin/main
git status
```

If GitHub has commits you don't have locally, Git will report:
`Your branch is behind 'origin/main' by X commits, and can be fast-forwarded.`

To view the remote commits before merging them:

```bash
git log HEAD..origin/main --oneline
```

### Step 6.2: Download and Integrate Changes (`git pull`)

To fetch and immediately merge remote changes into your active branch:

```bash
git pull origin main
```

> **Best Practice Rule:** Always run `git pull` before starting new work on a different machine to avoid out-of-sync code.

---

## 7. Handling Merge Conflicts

A **merge conflict** happens when changes are made to the **same line of the same file** both locally and on GitHub, and Git cannot automatically decide which version to keep.

### Step 7.1: Simulating a Conflict

1. On Computer A (or directly on GitHub.com), line 10 in `Program.cs` is changed to:
   `Console.WriteLine("Header Version A");` and committed/pushed.
2. On Computer B (locally), line 10 in `Program.cs` is changed to:
   `Console.WriteLine("Header Version B");` and committed locally.
3. On Computer B, you try to pull:

```bash
git pull origin main
```

Git will output a conflict warning:
```text
CONFLICT (content): Merge conflict in Program.cs
Automatic merge failed; fix conflicts and then commit the result.
```

### Step 7.2: Identifying and Resolving the Conflict

1. Check which files have conflicts:
   ```bash
   git status
   ```

2. Open `Program.cs` in your code editor. Git marks conflicting lines like this:

   ```csharp
   <<<<<<< HEAD (Your Local Changes)
   Console.WriteLine("Header Version B");
   =======
   Console.WriteLine("Header Version A");
   >>>>>>> origin/main (Remote Changes from GitHub)
   ```

3. **Manually edit the file:** Remove the Git marker symbols (`<<<<<<<`, `=======`, `>>>>>>>`) and edit the code to the desired final state:

   ```csharp
   Console.WriteLine("Header Version B - Final Clean Code");
   ```

4. Stage the resolved file:
   ```bash
   git add Program.cs
   ```

5. Complete the merge commit:
   ```bash
   git commit -m "Fix merge conflict in Program.cs application header"
   ```

6. Push the resolved code back to GitHub:
   ```bash
   git push origin main
   ```

---

## 💡 GitHub Workflow Summary

| Action | Command |
| :--- | :--- |
| **Add Remote** | `git remote add origin <URL>` |
| **Push First Time** | `git push -u origin main` |
| **Push Updates** | `git push` |
| **Inspect Remote** | `git fetch origin` |
| **Pull Remote Changes** | `git pull` |
| **Clone Repository** | `git clone <URL>` |
| **Check Remotes** | `git remote -v` |