using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.NetEx;
using Utils.TaskEx;
using Utils.WinformEx;

namespace Maple.NetWorkTools
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;


            btnPortScanerStop.Enabled = false;
           
            /*设置超时时限最小为10s，最大为30s*/
            tbPortScanerTimeOut.Minimum = 10;
            tbPortScanerTimeOut.Maximum = 30;
            tbPortScanerTimeOut.Value = 10;
        }


        #region 端口扫描
        private ConcurrentBag<IpState> ipStateList = new ConcurrentBag<IpState>();
        private BindingList<string> ipStateBindingList = new BindingList<string>();
        private static PortScaner portScaner;
        long ipStart = 0, ipEnd = 0; int portStart = 0, portEnd = 0;
        #region 开始停止
        private void btnPortScanerStart_Click(object sender, EventArgs e)
        {
            ipStateList.Clear();
            ipStateBindingList.Clear();
            rtxtPortScanerStatus.Clear();
            lstbPortScanerStatus.DataSource = ipStateBindingList;
            ipStateBindingList.Add("IP地址              " + "    端口       " + "  端口状态 " + "             服务");
            btnPortScanerStop.Enabled = true;
            btnPortScanerStart.Enabled = false;



            if (cbPortScanerSingleIp.Checked)
            {
                txtPortScanerIpEnd.Text = txtPortScanerIpStart.Text;
            }
            if (cbPortScanerSinglePort.Checked)
            {
                txtPortScanerPortEnd.Text = txtPortScanerPortStart.Text;
            }

            if (IpHelper.IsIp(txtPortScanerIpEnd.Text) && IpHelper.IsIp(txtPortScanerIpStart.Text))//匹配正确IP
            {
                try
                {
                    if (IpHelper.IpToInt64(txtPortScanerIpStart.Text) <= IpHelper.IpToInt64(txtPortScanerIpEnd.Text))
                    {
                        if (string.IsNullOrEmpty(txtPortScanerPortStart.Text) || string.IsNullOrEmpty(txtPortScanerPortEnd.Text))
                        {
                            WinFormHelper.ShowError("请输入端口号！");
                        }
                        else
                        {
                            ipStart = IpHelper.IpToInt64(txtPortScanerIpStart.Text);
                            ipEnd = IpHelper.IpToInt64(txtPortScanerIpEnd.Text);
                            portStart = Int32.Parse(txtPortScanerPortStart.Text);
                            portEnd = Int32.Parse(txtPortScanerPortEnd.Text);

                            if (portEnd < portStart)
                            {
                                WinFormHelper.ShowError("请填写正确端口范围");
                                return;
                            }

                            var progress = ((ipEnd + 1 - ipStart) * (portEnd + 1 - portStart));
                            if (progress > int.MaxValue)
                            {
                                WinFormHelper.ShowError("Ip地址范围过大");
                                return;
                            }
                            pgbPortScanerProgress.Minimum = 0;
                            pgbPortScanerProgress.Maximum = (int)progress ;
                            pgbPortScanerProgress.Value = 0;

                            System.Diagnostics.Debug.WriteLine("[Form主线程]ThreadId:{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
                            portScaner = new PortScaner(txtPortScanerIpStart.Text, txtPortScanerIpEnd.Text, portStart, portEnd, tbPortScanerTimeOut.Value, Convert.ToInt32(txtPortScanerTaskNum.Text));
                            if (portScaner != null)
                            {
                                portScaner.TaskProgress.ProgressChanged += PortScanerStatusProgressEvent;
                                portScaner.ScanProgress.ProgressChanged += PortScanerIpListProgressEvent;

                                portScaner.Action();

                            }
                           
                        }
                    }
                    else
                    {
                        WinFormHelper.ShowError("请输入有效的IP地址范围");
                        return;
                    }
                }
                catch(Exception ex) 
                {
                
                }
            }
            else
            {
                WinFormHelper.ShowError("请输入有效的IP地址");
                return;
            }





        }

        private void btnPortScanerStop_Click(object sender, EventArgs e)
        {
            ipStateList.Clear();
            if (portScaner != null)
            {
                portScaner.End();
            }
            btnPortScanerStop.Enabled = false;
            btnPortScanerStart.Enabled = true;
        }
        #endregion

        #region IP地址
        private void cbPortScanerSingleIp_CheckedChanged(object sender, EventArgs e)
        {
            txtPortScanerIpEnd.Visible = lbPortScanerIp.Visible = !cbPortScanerSingleIp.Checked;
        }

        private void txtPortScanerIpStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtPortScanerIpEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void txtPortScanerIpStart_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPortScanerIpEnd_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region 端口地址
        private void cbPortScanerSinglePort_CheckedChanged(object sender, EventArgs e)
        {
            txtPortScanerPortEnd.Visible = lbPortScanerPort.Visible = !cbPortScanerSinglePort.Checked;
        }
        private void txtPortScanerPortStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void txtPortScanerPortEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void txtPortScanerPortStart_TextChanged(object sender, EventArgs e)
        {
            //if (txtPortScanerPortEnd.Text.Length > 0 && txtPortScanerPortStart.Text.Length > 0 && Convert.ToInt32(txtPortScanerPortEnd.Text) > Convert.ToInt32(txtPortScanerPortStart.Text))
            //{
            //    txtPortScanerPortStart.Text = txtPortScanerPortEnd.Text;
            //}
        }

        private void txtPortScanerPortEnd_TextChanged(object sender, EventArgs e)
        {
            //if (txtPortScanerPortEnd.Text.Length > 0 && txtPortScanerPortStart.Text.Length > 0 && Convert.ToInt32(txtPortScanerPortEnd.Text) > Convert.ToInt32(txtPortScanerPortStart.Text))
            //{
            //    txtPortScanerPortEnd.Text = txtPortScanerPortStart.Text;
            //}
        }
        #endregion

        private void cbShowEnableIp_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowEnableIp.Checked)
            {
                ipStateBindingList.Clear();
                ipStateBindingList.Add("IP地址              " + "    端口       " + "  端口状态 " + "             服务");
                ipStateBindingList = (BindingList<string>)(ipStateList.AsParallel().Where(x => x.IsConnected == true).OrderBy(x => x.IpAddress).ThenBy(x => x.Port).Select(x => string.Format("{0}                  {1}         {2}              {3}", x.IpAddress, x.Port, x.IsConnected ? "open" : "close", x.ServiceName)).AsEnumerable());
            }
            else
            {
                ipStateBindingList.Clear();
                ipStateBindingList.Add("IP地址              " + "    端口       " + "  端口状态 " + "             服务");
                ipStateBindingList = (BindingList<string>)(ipStateList.AsParallel().OrderBy(x => x.IpAddress).ThenBy(x => x.Port).Select(x => string.Format("{0}                  {1}         {2}              {3}", x.IpAddress, x.Port, x.IsConnected ? "open" : "close", x.ServiceName)).AsEnumerable());
            }
        }

        #region 超时时间
        private void tbPortScanerTimeOut_Scroll(object sender, EventArgs e)
        {
            txtPortScanerTimeOut.Text = tbPortScanerTimeOut.Value.ToString();
        }
        #endregion

        #region 线程数
        private void txtPortScanerTaskNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void txtPortScanerTaskNum_TextChanged(object sender, EventArgs e)
        {
            //if (txtPortScanerTaskNum.Text.Length > 0 && Convert.ToInt32(txtPortScanerTaskNum.Text) < 3)
            //{
            //    txtPortScanerTaskNum.Text = "3";
            //}
        }
        #endregion

        #region 状态反馈
        private static object statusObj = new object();
        /// <summary>
        /// 进度通知
        /// </summary>
        /// <param name="text">文本</param>
        private void PortScanerStatusProgressEvent(object obj, string text)
        {
            lock (statusObj)
            {
                rtxtPortScanerStatus.AppendText(text + "\r\n");
            }
        }

        private static object IpObj = new object();
        /// <summary>
        /// 进度通知
        /// </summary>
        /// <param name="text">文本</param>
        private void PortScanerIpListProgressEvent(object obj, IpState ipState)
        {
           
            lock (IpObj)
            {
                if (ipState.IsConnected)
                {
                    ipStateList.Add(ipState);

                    ipStateBindingList.Add(string.Format("{0}                  {1}         {2}              {3}", ipState.IpAddress, ipState.Port, ipState.IsConnected ? "open" : "close", ipState.ServiceName));
                }

                pgbPortScanerProgress.Value = (int)((IpHelper.IpToInt64(ipState.IpAddress) + 1 - ipStart) * (ipState.Port + 1 - portStart));
                if (pgbPortScanerProgress.Value == pgbPortScanerProgress.Maximum)
                {
                    btnPortScanerStop_Click(null, null);
                }
                
            }
        }
        #endregion 
        #endregion

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (portScaner != null)
            {
                portScaner.End();
            }
        }

       

       

       


    }
}
