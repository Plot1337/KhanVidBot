using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace KhanVidBot.ConsoleUI;

internal class Logger
{
    public static readonly string[] _title = new string[] {
        ":::    ::: :::     ::: :::::::::  ",
        ":+:   :+:  :+:     :+: :+:    :+: ",
        "+:+  +:+   +:+     +:+ +:+    +:+ ",
        "+#++:++    +#+     +:+ +#++:++#+  ",
        "+#+  +#+    +#+   +#+  +#+    +#+ ",
        "#+#   #+#    #+#+#+#   #+#    #+# ",
        "###    ###     ###     #########  "
    };

    public static void WriteTitle()
    {
        Console.Clear();
        Console.ResetColor();
        Console.WriteLine();

        foreach (string i in _title)
            Console.WriteLine(_indent + i, Color.CornflowerBlue);

        Console.WriteLine();
    }

    public static void Error(string text, bool delay = true)
    {
        WriteLn(text, LogType.Error);

        if (delay)
            Thread.Sleep(1500);
    }

    public static void Error(Exception ex) =>
        Error(
            Program.IsDebug() ? ex.ToString() : ex.Message
            );

    public static void Info(string text) =>
        WriteLn(text, LogType.Info);

    public static bool InputBool(string text)
    {
        while (true)
        {
            Write(text + "? ", LogType.Input);

            Formatter[] formatter = {
                new("Y", Color.Lime),
                new("N", Color.Red),
            };

            Console.WriteLineFormatted(
                "{0}/{1}",
                Color.Gray,
                formatter
                );

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Y:
                    return true;

                case ConsoleKey.N:
                    return false;

                default:
                    Error("Failed to parse!");
                    continue;
            }
        }
    }

    public static int InputInt(
        string text,
        bool defOk = true
        )
    {
        int num;

        do
        {
            Input(text);

            bool res = int.TryParse(
                Console.ReadLine(),
                out num
                );

            if (res)
                break;

            Error("Failed to parse integer!");
        }
        while (defOk ? num != -1 : num >= 0);

        return num;
    }

    public static string? InputStr(string text)
    {
        Input(text);

        Console.ForegroundColor = Color.Yellow;

        string val =  Console.ReadLine();
        Console.ResetColor();

        return val;
    }

    public static void Input(string text) =>
        Write(text + ": ", LogType.Input);

    public static void Output(string text) =>
        WriteLn(text, LogType.Output);

    public static void Warn(string text, bool delay = true)
    {
        WriteLn(text, LogType.Warning);

        if (delay)
            Thread.Sleep(1500);
    }

    public static void WriteLn(string text, LogType type) =>
        Write(text + "\r\n", type);

    public static void Write(string text, LogType type)
    {
        lock (_writeLock)
            NonThreadSafeWrite(text, type);
    }

    private static void NonThreadSafeWrite(string text, LogType type)
    {
        Console.Write(
            _indent + DateTime.Now.ToString("G"),
            Color.LimeGreen
            );

        var typeColor = type switch
        {
            LogType.Error => Color.Red,
            LogType.Info => Color.Blue,
            LogType.Input => Color.Coral,
            LogType.Output => Color.Navy,
            LogType.Warning => Color.DarkOrange,
            _ => Color.Navy
        };

        Formatter[] formatter = {
            new(type.ToString().ToUpper(), typeColor)
        };

        Console.WriteFormatted(
            " [{0}] ",
            Color.Gray,
            formatter
            );

        Console.Write(text, Color.Silver);
    }

    private const string _indent = "  ";
    private static readonly object _writeLock = new();
}