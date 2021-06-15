using System;
using System.IO;
using System.IO.Pipes;

namespace SodaMod.IO
{
    public static class Logger
    {
        private static NamedPipeClientStream pipe;

        private static StreamWriter log;

        public static void Connect()
        {
            try
            {
                pipe = new NamedPipeClientStream(".", "sodamod-pipe", PipeDirection.InOut);
                log = new StreamWriter(pipe);
                pipe.Connect();
            }
            catch (InvalidOperationException)
            {
                // TODO
            }
        }

        public static void Close()
        {
            try
            {
                log?.Close();
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public static void WriteLine(string value, bool flush = true)
        {
            try
            {
                log.WriteLine(value);
                if (flush)
                {
                    log.Flush();
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
