# v1.0.0 Git and Release Commands

This document records the original commit and release workflow, organized by feature. The release is already published; do not repeat initialization or recreate the existing tag or release.

Requirements: Git, the .NET SDK, GitHub CLI (`gh`), and a local game installation with Modding API installed. Authenticate Git and GitHub CLI with an account that has write access to the repository.

## Connect to the Remote and Preserve Its History

These commands apply only to the original setup: no local `.git` directory and a remote repository containing only the initial LICENSE commit.

```powershell
Set-Location 'E:\Haruko386-UnOffical\HK-no-freeze-frame-mod'
git init -b main
git remote add origin https://github.com/Haruko386/Hollow-Knight-No-Freeze-Frame-MOD.git
git fetch origin --tags
git reset --mixed origin/main
git restore --source origin/main --worktree -- LICENSE
```

## Commit by Feature

```powershell
# Project setup: target framework, game references, and ignored local artifacts
git add .gitignore src/NoFreezeFrame/NoFreezeFrame.csproj
git commit -m "chore(project): configure Hollow Knight 1.5.78 mod project"

# Feature 1: block four freeze entry points, restore on disable, and guard repeated initialization
git add src/NoFreezeFrame/NoFreezeFrame.cs
git commit -m "feat(hitstop): block all global freeze entry points"

# Feature 2: English/Chinese menus with an API-managed persistent toggle
git add src/NoFreezeFrame/NoFreezeFrame.Menu.cs
git commit -m "feat(menu): add localized persistent freeze toggle"

# Installation, scope, validation status, and release documentation
git add README.md docs/releases/v1.0.0.md docs/GIT-COMMANDS.md
git commit -m "docs: document installation and v1.0.0 release"

# Build, ZIP packaging, and optional installation
git add build.ps1
git commit -m "build: add release packaging and installation script"
```

## Build, Push, Tag, and Publish

```powershell
.\build.ps1
git diff --check
git status --short
git log --oneline -6

git push -u origin main
git tag -a v1.0.0 -m "NoFreezeFrame v1.0.0"
git push origin v1.0.0

gh release create v1.0.0 `
  artifacts/NoFreezeFrame-1.0.0.zip `
  artifacts/NoFreezeFrame/NoFreezeFrame.dll `
  --repo Haruko386/Hollow-Knight-No-Freeze-Frame-MOD `
  --verify-tag `
  --title "NoFreezeFrame v1.0.0" `
  --notes-file docs/releases/v1.0.0.md `
  --latest

gh release view v1.0.0 `
  --repo Haruko386/Hollow-Knight-No-Freeze-Frame-MOD `
  --json url,tagName,isDraft,isPrerelease,assets

git ls-remote origin refs/heads/main refs/tags/v1.0.0 'refs/tags/v1.0.0^{}'
git status --short
```

`--verify-tag` requires the remote tag to exist, preventing GitHub from implicitly selecting another commit. The release archive contains only the mod DLL, README, and license. Game DLLs, local tools, decompiled files, and test caches are excluded from Git.
