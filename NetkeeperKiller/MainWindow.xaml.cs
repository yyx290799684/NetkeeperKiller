using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetkeeperKiller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon notifyIcon;
        public MainWindow()
        {
            dokill();
        }

        private async void dokill()
        {
            int a = 0;
            while (true)
            {
                string baiduIP = GetIpByHostName("www.baidu.com");//得到百度的ip
                bool baidu = Ping(baiduIP);//Ping测试连通性
                try
                {
                    if (baidu)
                    {
                        var mprocess = Process.GetProcesses();
                        foreach (var item in mprocess)
                        {
                            if (item.ProcessName == "NetKeeper")
                            {
                                item.Kill();
                                this.notifyIcon = new NotifyIcon();
                                this.notifyIcon.BalloonTipText = "Netkeeper已终止\n程序自动关闭";
                                this.notifyIcon.Text = "NetkeeperKiller";
                                this.notifyIcon.Visible = true;
                                this.notifyIcon.Icon = new System.Drawing.Icon("NetkeeperKiller.ico");
                                this.notifyIcon.ShowBalloonTip(1000);
                                App.Current.Shutdown();
                            }
                        }
                        a++;
                        if (a > 1)
                        {
                            this.notifyIcon = new NotifyIcon();
                            this.notifyIcon.BalloonTipText = "系统已经联网\n程序自动关闭";
                            this.notifyIcon.Text = "NetkeeperKiller";
                            this.notifyIcon.Visible = true;
                            this.notifyIcon.Icon = new System.Drawing.Icon("NetkeeperKiller.ico");
                            this.notifyIcon.ShowBalloonTip(1000);
                            App.Current.Shutdown();
                        }
                    }
                    else
                        a = 0;
                }
                catch (Exception) { }
                await Task.Delay(10000);
            }
        }

        /// <summary>
        /// 根据主机名（域名）获得主机的IP地址
        /// </summary>
        /// <param name="hostName">主机名或域名</param>
        /// <example> GetIPByDomain("www.google.com");</example>
        /// <returns>主机的IP地址</returns>
        public string GetIpByHostName(string hostName)
        {
            try
            {
                hostName = hostName.Trim();
                if (hostName == string.Empty)
                    return string.Empty;
                try
                {
                    System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(hostName);
                    return host.AddressList.GetValue(0).ToString();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            catch (Exception) { return string.Empty; }
        }

        /// <summary>
        /// 是否能 Ping 通指定的主机
        /// </summary>
        /// <param name="ip">ip 地址或主机名或域名</param>
        /// <returns>true 通，false 不通</returns>
        public bool Ping(string ip)
        {
            try
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
                options.DontFragment = true;
                string data = "Test Data!";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000; // Timeout 时间，单位：毫秒
                System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
            }
        }
    }
