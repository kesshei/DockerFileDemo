using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 公共简单的日志方法
    /// </summary>
    public static class Log
    {
        static object LockObj = new object();
        public static void Info(string msg)
        {
            lock (LockObj)
            {
                var msginfo = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {msg}";
                File.AppendAllLines(Path.Combine(AppContext.BaseDirectory, "log.log"), new string[] { msg });
                Console.WriteLine(msginfo);
            }
        }
        public static void Error(Exception exception, string msg = "")
        {
            lock (LockObj)
            {
                var msginfo = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {exception.Message}";
                File.AppendAllLines(Path.Combine(AppContext.BaseDirectory, "log.log"), new string[] { msg });
                Console.WriteLine(msginfo);
            }
        }
    }
}
