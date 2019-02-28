using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Utils.TaskEx;

namespace Utils.NetEx
{
    public class PortScaner : BaseBehavior
    {

        static Regex reg = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

        /// <summary>
        /// 起始IP地址
        /// </summary>
        private string IpStart { get; set; }

        /// <summary>
        /// 结束IP地址
        /// </summary>
        private string IpEnd { get; set; }

        /// <summary>
        /// 起始端口
        /// </summary>
        private int PortStart { get; set; }

        /// <summary>
        /// 结束端口
        /// </summary>
        private int PortEnd { get; set; }

        /// <summary>
        /// 最大线程数
        /// </summary>
        private int TaskMaxNum { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        private int TimeOut { get; set; }

        private TaskSupervisor TaskSupervisor { get; set; }

        private ConcurrentQueue<string> ReachableIps = new ConcurrentQueue<string>();


        #region Init
        public PortScaner(string ipStart = "127.0.0.1", string ipEnd = "127.0.0.1", int portStart = 80, int portEnd = 80, int timeOut = 30, int taskMaxNum = 10)
        {
            Init(ipStart, ipEnd, portStart, portEnd, timeOut, taskMaxNum);
        }

        public PortScaner(string ipStart, string ipEnd, int portStart, int timeOut = 30, int taskMaxNum = 10)
        {
            Init(ipStart, ipEnd, portStart, portStart, timeOut, taskMaxNum);
        }

        public PortScaner(string ipStart, int portStart, int portEnd, int timeOut = 30, int taskMaxNum = 10)
        {
            Init(ipStart, ipStart, portStart, portEnd, timeOut, taskMaxNum);
        }

        public PortScaner(string ipStart, int portStart, int timeOut = 30, int taskMaxNum = 10)
        {
            Init(ipStart, ipStart, portStart, portStart, timeOut, taskMaxNum);
        }

        private void Init(string ipStart = "127.0.0.1", string ipEnd = "127.0.0.1", int portStart = 80, int portEnd = 80, int timeOut = 30, int taskMaxNum = 10)
        {
            if (!reg.IsMatch(ipStart) || !reg.IsMatch(ipEnd))//匹配正确IP
            {
                throw new ArgumentException("Ip地址错误");
            }
            if (IpHelper.IpToInt64(ipStart) > IpHelper.IpToInt64(ipEnd))
            {
                throw new ArgumentException("Ip地址范围错误");
            }

            if (portStart <= 0 || portEnd <= 0 || portEnd < portStart)
            {
                throw new ArgumentException("端口范围不正确");
            }

            this.IpStart = ipStart;
            this.IpEnd = ipEnd;
            this.PortStart = portStart;
            this.PortEnd = portEnd;
            this.TimeOut = timeOut <= 0 ? 10 : timeOut;
            this.TaskMaxNum = taskMaxNum <= 0 ? 10 : taskMaxNum;

            TaskSupervisor = new TaskEx.TaskSupervisor(this.TaskMaxNum);

        }
        #endregion

        public override async Task Action()
        {
            CancelTokenSource = new CancellationTokenSource();
          
            Progress("[端口扫描器]开始扫描...");

            #region Ping 线程
            TaskSupervisor.Add(new Task(() =>
            {
                var intIpStart = IpHelper.IpToInt64(IpStart);
                var intIpEnd = IpHelper.IpToInt64(IpEnd);
                using (Ping ping = new Ping())
                {
                    for (var ip = intIpStart; ip <= intIpEnd; ip++)
                    {
                        if (!CancelTokenSource.IsCancellationRequested)
                        {
                            if (ReachableIps.Count <= 500)
                            {
                                var ipAddress = IpHelper.Int64ToIp(ip);
                                var reply = ping.Send(ipAddress, this.TimeOut);

                                if (reply.Status == IPStatus.Success)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    ReachableIps.Enqueue(ipAddress);

                                    sb.Append("[端口扫描器]正在Ping[" + ipAddress + "]  耗时" + reply.RoundtripTime + "毫秒");
                                    try
                                    {
                                        var host = Dns.GetHostEntry(ipAddress);
                                        sb.AppendLine("  主机名为 " + host.HostName);
                                    }
                                    catch
                                    { }

                                    Progress(sb.ToString());
                                }
                                else
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("[端口扫描器]主机[" + ipAddress + "]不可达");
                                    Progress(sb.ToString());
                                }
                            }
                            else
                            {
                                ip--;
                            }
                        }
                        else
                        {
                            Thread.CurrentThread.Abort();
                        }
                    }
                }

            }, CancelTokenSource.Token));
            #endregion

            #region 端口扫描
            TaskSupervisor.Add(Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!CancelTokenSource.IsCancellationRequested)
                    {
                        TaskSupervisor.DisposeInvalidTask();
                        if (ReachableIps.Count < 1)
                        {
                            Task.Delay(1000);
                            continue;
                        }

                        string waitIpAddress = "";
                        ReachableIps.TryDequeue(out waitIpAddress);

                        if (!CancelTokenSource.IsCancellationRequested)
                        {
                            if (!TaskSupervisor.Add(new Task(new Action<object>(x =>
                            {
                                for (int port = this.PortStart; port <= this.PortEnd; port++)
                                {
                                    if (!CancelTokenSource.IsCancellationRequested)
                                    {
                                        if (!TaskSupervisor.Add(new Task(new Action<object>(t =>
                                        {
                                            var endPoint = (IPEndPoint)t;

                                            if (!CancelTokenSource.IsCancellationRequested)
                                            {
                                                Progress("[端口扫描器]正在扫描主机[{0}:{1}]...", endPoint.Address.ToString(), endPoint.Port);
                                                bool isContented = false;
                                                try
                                                {
                                                    using (TcpClient tcp = new TcpClient())
                                                    {
                                                        tcp.Connect(endPoint);
                                                        isContented = tcp.Connected;
                                                    }
                                                }
                                                catch { }
                                                finally
                                                {
                                                    if (isContented)
                                                    {
                                                        Progress("[端口扫描器]主机[{0}:{1}]可以连通", endPoint.Address.ToString(), endPoint.Port);
                                                    }
                                                    else
                                                    {
                                                        Progress("[端口扫描器]主机[{0}:{1}]不可连通", endPoint.Address.ToString(), endPoint.Port);
                                                    }
                                                    ScanerProgress(new IpState
                                                    {
                                                        IpAddress = IPAddress.Parse(waitIpAddress),
                                                        Port = endPoint.Port,
                                                        IsConnected = isContented,
                                                        ServiceName = GetServiceName(endPoint.Port)
                                                    });
                                                }
                                            }

                                        }), new IPEndPoint(IPAddress.Parse(x.ToString()), port), CancelTokenSource.Token)))
                                        {
                                            --port;
                                        }
                                    }
                                    else
                                    {
                                        Thread.CurrentThread.Abort();
                                    }
                                }

                            }), waitIpAddress, CancelTokenSource.Token)))
                            {
                                ReachableIps.Enqueue(waitIpAddress);
                            }
                        }
                        else
                        {
                            Thread.CurrentThread.Abort();
                        }

                        TaskSupervisor.DisposeInvalidTask();
                    }


                }
                catch (Exception ex)
                {
                    Progress("[端口扫描器]发生异常：" + ex.ToString());
                }


            }, CancelTokenSource.Token)); 
            #endregion
        }


        public void End()
        {
            //// 通知关闭
            CancelTokenSource.Cancel();
            TaskSupervisor.DisposeAllTask();
        
        }


        /// <summary>
        /// 进度同步
        /// </summary>
        /// <param name="message">同步消息</param>
        protected void ScanerProgress(IpState ipState)
        {
            if (TaskProgress != null)
            {
                IProgress<IpState> progress = ScanProgress;
                progress.Report(ipState);
            }
        }


        /// <summary>
        /// 进度通知
        /// </summary>
        public  Progress<IpState> ScanProgress = new Progress<IpState>();


        private string GetServiceName(int port)
        {
            switch (port)
            {

                case 80:
                    return "HTTP协议代理服务";

                case 135:
                    return "DCE endpoint resolutionnetbios-ns";

                case 445:
                    return "安全服务";

                case 1025:
                    return "NetSpy.698(YAI)";


                case 8080:
                    return "HTTP协议代理服务";

                case 8081:
                    return "HTTP协议代理服务";

                case 3128:
                    return "HTTP协议代理服务";

                case 9080:
                    return "HTTP协议代理服务";

                case 1080:
                    return "SOCKS代理协议服务";

                case 21:
                    return "FTP(文件传输)协议代理服务";

                case 23:
                    return "Telnet(远程登录)协议代理服务";

                case 443:
                    return "HTTPS协议代理服务";

                case 69:
                    return "TFTP协议代理服务";

                case 22:
                    return "SSH、SCP、端口重定向协议代理服务";

                case 25:
                    return "SMTP协议代理服务";

                case 110:
                    return "POP3协议代理服务";
                default:
                    return "Unknow Servies";



            }
        }

    }


    public class IpState
    {

        public IPAddress IpAddress { get; set; }

        public int Port { get; set; }

        public bool IsConnected { get; set; }

        public string ServiceName { get; set; }

    }
}
