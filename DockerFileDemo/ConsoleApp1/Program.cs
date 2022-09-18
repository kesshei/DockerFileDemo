using Common;
using System.Globalization;

namespace ConsoleApp1
{
    internal class Program
    {
    /// <summary>
    /// 此服务，不能停
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        //获取环境变量
        var env = ConfigManage.GetSetting("config");
        var psw = ConfigManage.GetSetting("psw");
        Log.Info($"获取环境变量 config:{env} psw:{psw}");
        Task.Run(() =>
        {
            while (true)
            {
                Log.Info("自动执行");
                Thread.Sleep(5000);
            }
        });
        Console.ReadLine();
    }
    }
}