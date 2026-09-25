# Git Tutorial Part 3: Team Collaboration via GitHub Organization (`UAI-VUT-Brno`)

In this final tutorial, you will transition from individual development to **team collaboration** using the department's GitHub organization: **`UAI-VUT-Brno`**. You will work in small groups of 3 to 4 students on a shared private repository, practice the **Pull Request (PR) workflow**, and perform code reviews.

---

## Step 1: Requesting a Repository from the Instructor

Before coding as a team, your designated **Team Leader** must request a private repository within the `UAI-VUT-Brno` organization.

### What the Team Leader Needs to Prepare:
1. Choose a unique project name (e.g., `calculator-team-alpha`).
2. Collect the **exact GitHub usernames** of all 3–4 team members.

### Template Message / Email for the Teacher:
> **Subject:** Request for Project Repository - Team Alpha (`UAI-VUT-Brno`)
>
> Dear Instructor,
> 
> Our project team would like to request a private repository within the `UAI-VUT-Brno` organization for our C# Calculator project.
> 
> * **Proposed Repository Name:** `calculator-team-alpha`
> * **Team Leader (GitHub Username):** `@leader-username`
> * **Team Members (GitHub Usernames):**
>   * `@member1-username`
>   * `@member2-username`
>   * `@member3-username`
> 
> Please configure the repository with a **Pull Request workflow** so we can practice code reviews. Thank you!

Once the instructor creates the repository and adds your team with write permissions, you are ready to start.

---

## Step 2: Cloning the Shared Repository

Every team member will clone the newly created private repository to their local machine.

1. Go to the repository URL provided by your teacher (e.g., `https://github.com/UAI-VUT-Brno/calculator-team-alpha`).
2. Click the green **Code** button and copy the HTTPS URL.
3. Open your terminal and clone the repository:

```bash
git clone https://github.com/UAI-VUT-Brno/calculator-team-alpha.git
cd calculator-team-alpha
```

---

## Step 3: Branching Strategy for Teams

**Rule #1 of Team Collaboration:** *Never commit directly to the `main` branch!* The `main` branch must always remain clean, stable, and working.

Every feature or task must be developed on its own **feature branch**.

1. Ensure your local `main` branch is up to date:
   ```bash
   git checkout main
   git pull origin main
   ```

2. Create and switch to your personal feature branch (use a descriptive name):
   ```bash
   git checkout -b feature/trigonometry-sin-cos
   ```

---

## Step 4: Making Changes, Committing, and Pushing

1. Implement your assigned feature in `Program.cs` (e.g., adding trigonometric functions).
2. Check your changes and stage the files:
   ```bash
   git status
   git add Program.cs
   ```
3. Commit your changes with a clear message:
   ```bash
   git commit -m "Implement sin and cos functions in CalculatorEngine"
   ```
4. Push your feature branch to the remote organization repository (`origin`):
   ```bash
   git push -u origin feature/trigonometry-sin-cos
   ```

---

## Step 5: Creating a Pull Request (PR)

Once your feature branch is pushed to GitHub, you need to ask your teammates to review your code before merging it into `main`.

1. Go to your repository page on GitHub (`https://github.com/UAI-VUT-Brno/calculator-team-alpha`).
2. You will see a yellow banner prompting: **"Compare & pull request"** for your recently pushed branch. Click it.
3. **Configure the Pull Request:**
   * **base:** `main` $\leftarrow$ **compare:** `feature/trigonometry-sin-cos`
   * Give your PR a clear title and description explaining what was implemented.
4. **Assign Reviewers:** On the right sidebar, under **Reviewers**, select 1 or 2 teammates to review your code.
5. Click **Create pull request**.

---

## Step 6: Code Review and Discussion

Code review is a core professional practice where teammates check for bugs, readability, and logic issues.

1. **The Reviewer's Role:**
   * A teammate opens the **Pull Requests** tab on GitHub and clicks your PR.
   * Go to the **Files changed** tab to inspect your code line by line.
   * Click the `+` icon on any line to leave a comment or suggestion.
   * If everything looks good, go to **Review changes** $\rightarrow$ select **Approve** $\rightarrow$ **Submit review**.

2. **Addressing Feedback (if needed):**
   * If a reviewer finds a bug, make the fix on your local machine.
   * Stage, commit, and push again:
     ```bash
     git add Program.cs
     git commit -m "Fix parameter bug pointed out in review"
     git push
     ```
   * Your changes will automatically update inside the active Pull Request.

---

## Step 7: Merging the Pull Request into `main`

Once your Pull Request has received the required approvals and all checks pass:

1. The author or reviewer clicks the green **Merge pull request** button on GitHub.
2. Confirm with **Confirm merge**.
3. **Clean up:** Delete the remote feature branch by clicking **Delete branch** on GitHub.

### Syncing Local Work After Merge

Now that your feature is part of `main` on GitHub, all other team members must update their local repositories:

```bash
# Switch to main branch
git checkout main

# Pull the latest merged changes from GitHub
git pull origin main

# Delete your local obsolete feature branch (optional)
git branch -d feature/trigonometry-sin-cos
```

---

## 💡 Team Workflow Cheat Sheet

| Action | Who Does It? | Command / Action |
| :--- | :--- | :--- |
| **Request Repo** | Team Leader | Email/Message teacher with member usernames |
| **Clone Project** | All Members | `git clone <repo-url>` |
| **Create Branch** | Developer | `git checkout -b feature/my-task` |
| **Push Branch** | Developer | `git push -u origin feature/my-task` |
| **Open PR** | Developer | GitHub Web UI $\rightarrow$ "Compare & pull request" |
| **Code Review** | Teammate | GitHub Web UI $\rightarrow$ Files changed $\rightarrow$ Approve |
| **Merge PR** | Developer / Lead | GitHub Web UI $\rightarrow$ "Merge pull request" |
| **Update Local** | All Members | `git checkout main && git pull origin main` |