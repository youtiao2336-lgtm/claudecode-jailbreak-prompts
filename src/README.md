# 源码与构建说明

项目基于 Windows Forms 和 .NET Framework 4.x，无第三方运行时依赖。

## 构建

在仓库根目录执行：

```powershell
.\build.ps1 -Target All -Verify
```

输出：

- `ccprompt-gui.exe`：图形界面入口
- `ccprompt.exe`：命令行入口

## 文件职责

| 文件 | 职责 |
|---|---|
| `ProgramGUI.cs` | GUI 启动入口与资源目录定位 |
| `MainForm.cs` | 主窗口、目标选择、注入和回滚操作 |
| `PromptEditorForm.cs` | 提示词新建、编辑与删除窗口 |
| `Ui.cs` | 主题、圆角分区卡片和圆角按钮控件 |
| `ProgramCLI.cs` | CLI 启动入口与控制台编码 |
| `Cli.cs` | CLI 参数和命令处理 |
| `PromptLib.cs` | GUI/CLI 共用的提示词、备份、注入、回滚及检测逻辑 |

GUI 编译需引用 `System.Drawing.dll` 和 `System.Windows.Forms.dll`；CLI 仅引用 `System.dll`。完整参数以根目录 `build.ps1` 为准。

