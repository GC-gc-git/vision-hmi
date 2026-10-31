# vision-hmi · 工业视觉上位机工程模板

C# / WinForm 工业上位机工程实践记录。目标岗位：**视觉应用 / 视觉集成工程师**。

---

## W1 单元：日志体系与全局异常兜底

工业上位机的"基础设施单元"——不接相机、不接 PLC，先把**日志**和**异常兜底**这两件最影响现场稳定性的事做扎实。

| 工程 | 主题 | 核心产出 |
|---|---|---|
| `w1-异常体系` | Day1 · 异常取证与日志落地 | 从"程序崩了不知道为什么"到留下可追溯证据 |
| `w1-day2-日志级别及统一格式` | Day2 · 五级日志与统一格式 | Debug/Info/Warn/Error/Fatal + 统一输出格式 |
| `w1-day2-线程安全` | Day2 · 并发写入 | `lock` 保证多线程写日志不丢行、不串行 |
| `w1-day3-错误码表` | Day3 · 业务错误码体系 | 错误码（是什么错）+ 日志级别（多严重）两套正交体系 |
| `w1-day4-全局异常兜底` | Day4 · 三层兜底 | UI 线程 `ThreadException` / 进程级 `UnhandledException` / `await` 自接 |
| `w1-day5-日志库选型+NLog 实战` | Day5 · 三方对比后定案 | 手写 lock 版 vs 机构队列组件 vs NLog 5.4.0 vs Log4Net |
| `w1-day6-w1总结` | Day6 · 最终形态 | 错误码 + NLog + 双层兜底 + 配置自愈，全部合并 |

**`w1-day6-w1总结` 是 W1 的最终落地形态**，W2 起步时直接复用它。

---

## 选型结论：手写 → 队列 → NLog

| 维度 | 手写 lock 版 | 机构队列组件 | **NLog 5.4.0** | Log4Net |
|---|---|---|---|---|
| 多线程安全 | ✓ lock | ✓ 专用写线程 | ✓ 内置 AsyncWrapper | ✓ |
| 队列满不丢日志 | — | ✗ 无 Flush，尾部全丢 | ✓ `overflowAction=Block` | ✓ |
| 退出前冲刷 | 手动 | ✗ 缺机制 | ✓ `LogManager.Flush()` | ✓ |
| 运行时改级别 | 改代码重编 | 改代码重编 | ✓ **热重载，不重编** | ✗ |
| 归档 / 分文件 | 手工拼文件名 | — | ✓ `archiveAboveSize` | ✓ |
| 内部错误可诊断 | — | — | ✓ `InternalLogger` | 部分 |

**最终方案不是二选一**：业务语义层（错误码、级别、格式、文案）手写，写入机制层（异步、归档、缓冲、诊断）交给 NLog。

---

## 实测结论（本仓库全部数字均为实测，非文档摘抄）

| 场景 | 结果 |
|---|---|
| 并发写日志 | 10 线程 × 500 条 = **5000 条，落盘 5000，零丢失** |
| 队列满（`Discard`） | 写 500 条只落盘 10 条，**静默丢 490 条**（无任何提示） |
| 崩溃时丢日志 | 后台线程崩溃 → 20000 条只落盘 11094（**丢 45%**） |
| **崩溃处理器内调 `Flush()`** | 11094 → **20006，全部救回** |
| 正常关闭窗体 | 需在 `Application.Run()` **返回之后**调 `Flush()`（WinForm 与 Console 不同） |
| 配置被删 / 损坏（启动时） | **日志全丢，程序退出码 0，零提示** ← 最危险 |
| 配置被删 / 损坏（运行中） | 已写的 500 条一条不少，NLog 继续用旧配置 |
| 内部日志 | XML 写 `internalLogFile` → **0 个文件**；编程接口 `InternalLogger.LogFile` → 17 行完整堆栈 |

### 踩过的坑

1. **同名 `Logger` 类遮蔽** —— 同命名空间里自写的 `Logger` 会遮蔽 `NLog.Logger`，报 `CS0029`。用 `ILogger` 或改名。
2. **`log.Error("Error", msg)` 丢消息** —— 命中 `Error(string, params object[])` 重载，`msg` 被当格式化参数丢弃。必须只传一个参数。
3. **`try { } finally { }` 后接 `catch { }`** —— C# 语法错误，正确顺序是 `try → catch → finally`。
4. **配置自愈必须自己实现** —— NLog 不会自动生成默认配置。要在启动时校验 `NLog.config`，缺失或损坏就写一份默认的。

---

## 目录结构

```
├─ VisionHMI/W1/            7 个独立练习工程（每个都能单独编译运行）
│  ├─ W1.slnx               解决方案（相对路径，可整包迁移）
│  └─ packages/             NuGet 包（已 gitignore，用 NuGet 还原）
├─ 学习日志/                7 张任务卡 HTML（含完整实测数据与坑位说明）
├─ W1学习记录.md            学习笔记、验收标准、面试题自检
└─ .gitignore
```

## 如何运行

```bash
# 1. 还原 NuGet 包（Day5 / Day6 需要 NLog 5.4.0）
nuget restore VisionHMI/W1/W1.slnx
# 或者用 Visual Studio 打开 W1.slnx，VS 会自动还原

# 2. 打开解决方案
dotnet sln VisionHMI/W1/W1.slnx list
```

各工程输出 `bin/Debug/logs/{日期}.log`，日志按 `${longdate}|${level}|${message}` 格式落盘。

## 技术栈

- .NET Framework 4.8 / C# / WinForms
- NLog 5.4.0（异步写入、热重载、归档）
- Visual Studio 2022

## 学习记录

- [W1 学习记录](W1学习记录.md) —— 单元验收标准、坑位清单、面试题自检
- `学习日志/` 目录 —— 每个单元的任务卡，含完整实测数据与源码说明
