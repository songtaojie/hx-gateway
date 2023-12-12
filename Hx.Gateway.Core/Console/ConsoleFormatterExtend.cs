// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core;

/// <summary>
/// 控制台默认格式化程序拓展
/// </summary>
public sealed class ConsoleFormatterExtend : ConsoleFormatter, IDisposable
{
    /// <summary>
    /// 异常分隔符
    /// </summary>
    private const string EXCEPTION_SEPARATOR = "++++++++++++++++++++++++++++++++++++++++++++++++++++++++";

    /// <summary>
    /// 日志格式化选项刷新 Token
    /// </summary>
    private readonly IDisposable _formatOptionsReloadToken;

    /// <summary>
    /// 日志格式化配置选项
    /// </summary>
    private ConsoleFormatterOptions _formatterOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="formatterOptions"></param>
    public ConsoleFormatterExtend(IOptionsMonitor<ConsoleFormatterOptions> formatterOptions)
        : base("console-format")
    {
        (_formatOptionsReloadToken, _formatterOptions) = (formatterOptions.OnChange(ReloadFormatterOptions), formatterOptions.CurrentValue);
    }

    /// <summary>
    /// 写入日志
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logEntry"></param>
    /// <param name="scopeProvider"></param>
    /// <param name="textWriter"></param>
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        // 获取标准化日志消息
        string standardMessage = OutputStandardMessage(logEntry);
        // 空检查
        if (standardMessage is null) return;
        // 写入控制台
        textWriter.WriteLine(standardMessage);
    }

    /// <summary>
    /// 释放非托管资源
    /// </summary>
    public void Dispose()
    {
        _formatOptionsReloadToken?.Dispose();
    }

    /// <summary>
    /// 刷新日志格式化选项
    /// </summary>
    /// <param name="options"></param>
    private void ReloadFormatterOptions(ConsoleFormatterOptions options)
    {
        _formatterOptions = options;
    }

    /// <summary>
    /// 输出标准日志消息
    /// </summary>
    /// <param name="logEntry"></param>
    /// <param name="dateFormat"></param>
    /// <returns></returns>
    private string OutputStandardMessage<TState>(LogEntry<TState> logEntry , string dateFormat = "yyyy-MM-dd HH:mm:ss.fffffff zzz dddd")
    {
        // 获取格式化后的消息
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);

        // 空检查
        if (message is null) return null;
        // 创建日志消息
        var logDateTime = _formatterOptions.UseUtcTimestamp ? DateTime.UtcNow : DateTime.Now;
        // 创建默认日志格式化模板
        var formatString = new StringBuilder();

        // 获取日志级别对应控制台的颜色
        var logLevelColors = GetLogLevelConsoleColors(logEntry.LogLevel);

        _ = AppendWithColor(formatString, GetLogLevelString(logEntry.LogLevel), logLevelColors.Item1, logLevelColors.Item2);
        formatString.Append(": ");
        formatString.Append(logDateTime.ToString(dateFormat));
        formatString.Append(' ');
        formatString.Append(_formatterOptions.UseUtcTimestamp ? "U" : "L");
        formatString.Append(' ');
        _ = AppendWithColor(formatString, logEntry.Category, ConsoleColor.Cyan, ConsoleColor.DarkCyan);
        formatString.Append('[');
        formatString.Append(logEntry.EventId.Id);
        formatString.Append(']');
        formatString.Append(' ');
        formatString.Append($"#{Environment.CurrentManagedThreadId}");
        formatString.AppendLine();

        // 对日志内容进行缩进对齐处理
        formatString.Append(PadLeftAlign(message));

        // 如果包含异常信息，则创建新一行写入
        if (logEntry.Exception != null)
        {
            var EXCEPTION_SEPARATOR_WITHCOLOR = AppendWithColor(default, EXCEPTION_SEPARATOR, logLevelColors.Item1, logLevelColors.Item2).ToString();
            var exceptionMessage = $"{Environment.NewLine}{EXCEPTION_SEPARATOR_WITHCOLOR}{Environment.NewLine}{logEntry.Exception}{Environment.NewLine}{EXCEPTION_SEPARATOR_WITHCOLOR}";

            formatString.Append(PadLeftAlign(exceptionMessage));
        }

        // 返回日志消息模板
        return formatString.ToString();
    }
    /// <summary>
    /// 将日志内容进行对齐
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static string PadLeftAlign(string message)
    {
        var newMessage = string.Join(Environment.NewLine, message.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None)
                    .Select(line => string.Empty.PadLeft(6, ' ') + line));

        return newMessage;
    }
    /// <summary>
    /// 获取日志级别短名称
    /// </summary>
    /// <param name="logLevel">日志级别</param>
    /// <returns></returns>
    internal static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel)),
        };
    }

    /// <summary>
    /// 拓展 StringBuilder 增加带颜色写入
    /// </summary>
    /// <param name="message"></param>
    /// <param name="formatString"></param>
    /// <param name="background"></param>
    /// <param name="foreground"></param>
    /// <returns></returns>
    private static StringBuilder AppendWithColor(StringBuilder formatString, string message, ConsoleColor? foreground, ConsoleColor? background)
    {
        formatString ??= new();

        // 输出控制台前景色和背景色
        if (background.HasValue) formatString.Append(GetBackgroundColorEscapeCode(background.Value));
        if (foreground.HasValue) formatString.Append(GetForegroundColorEscapeCode(foreground.Value));

        formatString.Append(message);

        // 输出控制台前景色和背景色
        if (background.HasValue) formatString.Append("\u001b[39m\u001b[22m");
        if (foreground.HasValue) formatString.Append("\u001b[49m");

        return formatString;
    }

    /// <summary>
    /// 获取控制台日志级别对应的颜色
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    private static (ConsoleColor?, ConsoleColor?) GetLogLevelConsoleColors(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Critical => (ConsoleColor.White, ConsoleColor.Red),
            LogLevel.Error => (ConsoleColor.Black, ConsoleColor.Red),
            LogLevel.Warning => (ConsoleColor.Yellow, ConsoleColor.Black),
            LogLevel.Information => (ConsoleColor.DarkGreen, ConsoleColor.Black),
            LogLevel.Debug => (ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Trace => (ConsoleColor.Gray, ConsoleColor.Black),
            _ => (null, null),
        };
    }

    /// <summary>
    /// 输出控制台字体颜色 UniCode 码
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static string GetForegroundColorEscapeCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "\u001b[30m",
            ConsoleColor.DarkRed => "\u001b[31m",
            ConsoleColor.DarkGreen => "\u001b[32m",
            ConsoleColor.DarkYellow => "\u001b[33m",
            ConsoleColor.DarkBlue => "\u001b[34m",
            ConsoleColor.DarkMagenta => "\u001b[35m",
            ConsoleColor.DarkCyan => "\u001b[36m",
            ConsoleColor.Gray => "\u001b[37m",
            ConsoleColor.Red => "\u001b[1m\u001b[31m",
            ConsoleColor.Green => "\u001b[1m\u001b[32m",
            ConsoleColor.Yellow => "\u001b[1m\u001b[33m",
            ConsoleColor.Blue => "\u001b[1m\u001b[34m",
            ConsoleColor.Magenta => "\u001b[1m\u001b[35m",
            ConsoleColor.Cyan => "\u001b[1m\u001b[36m",
            ConsoleColor.White => "\u001b[1m\u001b[37m",
            _ => "\u001b[39m\u001b[22m",
        };
    }

    /// <summary>
    /// 输出控制台背景颜色 UniCode 码
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static string GetBackgroundColorEscapeCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "\u001b[40m",
            ConsoleColor.Red => "\u001b[41m",
            ConsoleColor.Green => "\u001b[42m",
            ConsoleColor.Yellow => "\u001b[43m",
            ConsoleColor.Blue => "\u001b[44m",
            ConsoleColor.Magenta => "\u001b[45m",
            ConsoleColor.Cyan => "\u001b[46m",
            ConsoleColor.White => "\u001b[47m",
            _ => "\u001b[49m",
        };
    }

}