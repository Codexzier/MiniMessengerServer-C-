using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniMessengerServer
{
    public static class SimpleLogging
    {
        public static void Log(string message) => SingeltonLogging.GetInstance().Log(message);

        private class SingeltonLogging
        {
            private static SingeltonLogging _instance;
            private readonly string _path;

            private SingeltonLogging(string path)
            {
                this._path = path;
            }

            internal static SingeltonLogging GetInstance()
            {
                if (_instance == null)
                {
                    _instance = new SingeltonLogging(Environment.CurrentDirectory);
                }

                return _instance;
            }

            internal void Log(string message)
            {
                using var writer = new StreamWriter($"{this._path}\\log_{DateTime.Now:yyyy_MM_dd}.txt", true);
                writer.Write($"{DateTime.Now:dd.MM.yyyy hh:mm} | {message}");
            }
        }
    }
}
