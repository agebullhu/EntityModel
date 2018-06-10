# LogRecorder

## 配置

配置节:LogRecorder;

1. level:日志等级
-  None : 所有
-  Debug: 调试信息
-  Trace: 跟踪信息
-  Warning:警告
-  System :系统
-  Error:错误

2. txtPath:文本日志记录的目录. 
- 默认:当前目录(Environment.CurrentDirectory)的logs目录
> 大量的文本日志,可能导致该目录磁盘空间紧张

3. console:是否将日志输出到控制台,默认为否

4. monitor:是否开启跟踪日志,默认为否

5. sql:是否开启SQL日志,默认为否

