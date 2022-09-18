# Docker 系列之 .Net Core 控制台和 Asp.net Core 服务生成镜像(DockerFile)

通过 Docker File 的方式来生成镜像，这个时候，镜像可以单独打包给对方，对方 Docker 加载了，就可以运行了。

通过对控制台应用以及 web 服务应用一起举例，会更容易参考。

## 新建 控制台应用和 web 服务

通过两种不同形态的程序来部署镜像

### 控制台应用

新建项目后，可以通过，项目右键，添加，docker 支持

![](https://tupian.wanmeisys.com/markdown/1663495627921-af9fd3d8-e468-48da-b143-807ff6639b28.png)
直接 Linux 即可
![](https://tupian.wanmeisys.com/markdown/1663495641452-f919206e-3ba6-4ae8-ac0f-24a03cae6cf2.png)
然后，查看就有一个 dockerfile 文件

![](https://tupian.wanmeisys.com/markdown/1663495688590-a6e78445-fb4a-49ca-b670-5992aa92a449.png)

我们，增加逻辑后
```csharp
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
```

DockerFile 内容

```csharp
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp1/ConsoleApp1.csproj", "ConsoleApp1/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "ConsoleApp1/ConsoleApp1.csproj"
COPY . .
WORKDIR "/src/ConsoleApp1"
RUN dotnet build "ConsoleApp1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV TZ=Asia/Shanghai
ENV config=empty
ENV psw=empty
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp1.dll"]
```
我在上面增加了三行代码，主要是解决时区和显示定义环境变量
```csharp
ENV TZ=Asia/Shanghai
ENV config=empty
ENV psw=empty
```
生成镜像的话，可以用 VS 自己的命令

![](https://tupian.wanmeisys.com/markdown/1663500688638-650df266-e494-4087-9feb-f0db93ed4ae7.png)

十分的方便，不用敲命令，如果你想敲，大概如下:

```csharp
docker build -f "G:\git\03 自己的项目\DockerFileDemo\ConsoleApp1\Dockerfile" --force-rm -t consoleapp1  "G:\git\03 自己的项目\DockerFileDemo"
```

因为在 windows 上，所以，此命令指定了具体的 dockerfile 地址和最后的上下文地址，其他的跟正常的命令一致。

运行 docker 镜像为容器服务
```csharp
docker run --name test -e psw=123 -e config=配置 -it -d consoleapp1
```
执行以上方法就可以把容器运行起来

![](https://tupian.wanmeisys.com/markdown/1663504256254-19c5dd03-9da9-4f2a-8e40-e8b51e3687dd.png)

可以明显看到内容是符合我们预期的，也获取到了环境变量的相关配置信息。

如果默认不给环境变量，会是我们自己定义的值

![](https://tupian.wanmeisys.com/markdown/1663504355750-e1cad4d4-b451-48c7-9208-61f3e0848289.png)

控制台方案，就以环境变量为案例。
## web api 服务

![](https://tupian.wanmeisys.com/markdown/1663495732689-5940a43e-a814-44fd-b927-a5b940892650.png)

添加后，默认就有 docker file 了，这docker file可以删除，然后，重新添加，如上

下面是我随便修改了一下逻辑
```csharp
[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        var info = ConfigManage.GetSetting("conn");
        return info;
    }
}
```
然后，右键生成镜像

执行docker 命令
```csharp
docker run --name test1  -p 900:80  -it -d webapplication1
```
然后访问 http://127.0.0.1:900/home 即可看到效果

![](https://tupian.wanmeisys.com/markdown/1663505404414-3e68a561-cbc3-4c38-9b46-67fd6bb6fbe8.png)

然后，执行以下命令
```csharp
docker run --name test1  -p 900:80 --mount type=bind,src=/d/dockerConfig/web/appsettings.json,dst=/app/appsettings.json -it -d webapplication1
```
![](https://tupian.wanmeisys.com/markdown/1663505915893-4b1f84be-44c0-46a3-b89d-24e8c1a599ab.png)
命令实现了对 以下路径 配置文件的挂载   mount 

![](https://tupian.wanmeisys.com/markdown/1663505954367-5ca1ed82-1fa7-4b69-9b39-dc382593b473.png)


# 总结
Visual Studio 真是好工具，我只能说太方便了，加上容器的功能，简直不要太到位。

不仅仅通过dockerfile创建了控制台的容器和web的容器，还解决了时区的问题，以及实现了服务器环境下的环境变量传参以及配置文件挂载传参。

# 阅

一键三连呦！，感谢大佬的支持，您的支持就是我的动力!

# 代码地址

https://github.com/kesshei/DockerFileDemo.git
https://gitee.com/kesshei/DockerFileDemo.git