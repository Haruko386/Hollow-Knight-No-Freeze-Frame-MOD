# v1.0.0 提交与发布指令

以下记录本次按功能拆分提交的完整流程，供查看与复用。当前仓库发布完成后，不要再重复初始化、创建同名标签或同名 Release。

前置条件：Git、.NET SDK、GitHub CLI（`gh`），以及已安装 Modding API 的游戏。Git 与 GitHub CLI 需要已登录有仓库写入权限的账号。

## 连接远程并保留初始历史

仅适用于本次开始时“本地尚无 `.git`、远程仅有初始 LICENSE”的状态：

```powershell
Set-Location 'E:\Haruko386-UnOffical\HK-no-freeze-frame-mod'
git init -b main
git remote add origin https://github.com/Haruko386/Hollow-Knight-No-Freeze-Frame-MOD.git
git fetch origin --tags
git reset --mixed origin/main
git restore --source origin/main --worktree -- LICENSE
```

## 按功能提交

```powershell
# 项目配置：目标框架、游戏程序集引用、忽略本地构建与反编译文件
git add .gitignore src/NoFreezeFrame/NoFreezeFrame.csproj
git commit -m "chore(project): configure Hollow Knight 1.5.78 mod project"

# feat 1：四个全局冻结入口、关闭恢复、重复初始化保护
git add src/NoFreezeFrame/NoFreezeFrame.cs
git commit -m "feat(hitstop): block all global freeze entry points"

# feat 2：中英文设置菜单、API 管理开关与持久化
git add src/NoFreezeFrame/NoFreezeFrame.Menu.cs
git commit -m "feat(menu): add localized persistent freeze toggle"

# 安装说明、适用范围、验证状态与发布记录
git add README.md docs/releases/v1.0.0.md docs/GIT-COMMANDS.md
git commit -m "docs: document installation and v1.0.0 release"

# 构建、ZIP 打包与可选安装
git add build.ps1
git commit -m "build: add release packaging and installation script"
```

## 构建、推送、打标签、发布

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

`--verify-tag` 要求远程标签已经存在，避免 GitHub 隐式选取别的提交。发布包只包含本模组 DLL、使用说明及许可证；游戏 DLL、本地工具、反编译资料与测试缓存均不上传到 Git。
