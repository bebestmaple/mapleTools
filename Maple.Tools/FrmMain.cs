using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Utils.SecurityEx;
using Utils.TaskEx;
using Utils.WinformEx;

namespace Maple.Tools
{
    public partial class FrmMain : Form
    {
        string savePath = "";
        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            

            // 图片批量下载同步进度
            BaseBehavior.TaskProgress.ProgressChanged += ImgBatchDownloadProgressEvent;
            btnImgBatchDownloadPause.Enabled = false;

            #region 对称加密 下拉框
            Dictionary<int, string> encryptMethodDic = new Dictionary<int, string>();
            encryptMethodDic.Add(1, "AES(128)");
            encryptMethodDic.Add(2, "AES(256)");
            encryptMethodDic.Add(3, "DES");
            encryptMethodDic.Add(4, "RC4");
            //encryptFormDic.Add(5, "Rabbit");
            encryptMethodDic.Add(6, "TripleDES");
            BindingSource bs = new BindingSource();
            bs.DataSource = encryptMethodDic;
            cmbSymmetricEncryptionEncryptMethod.DataSource = bs;
            cmbSymmetricEncryptionEncryptMethod.ValueMember = "Key";
            cmbSymmetricEncryptionEncryptMethod.DisplayMember = "Value"; 
            #endregion
        }

        #region 编码解码
        #region base64 编解码
        private void btn_Base64_Code_Click(object sender, EventArgs e)
        {
            var codeText = rtxt_Base64_Orginal.Text;
            if (string.IsNullOrWhiteSpace(codeText))
            {
                WinFormHelper.ShowWarning("请输入要编码的文本！");
                return;
            }
            try
            {
                rtxt_Base64_Handled.Text = Base64Helper.ToBase64String(codeText);
            }
            catch (Exception ex)
            {
                WinFormHelper.ShowError(ex);

            }
        }

        private void btn_Base64_Decode_Click(object sender, EventArgs e)
        {
            var codeText = rtxt_Base64_Orginal.Text;
            if (string.IsNullOrWhiteSpace(codeText))
            {
                WinFormHelper.ShowWarning("请输入要解码的文本！");
                return;
            }
            try
            {
                rtxt_Base64_Handled.Text = Base64Helper.UnBase64String(codeText);
            }
            catch (Exception ex)
            {
                WinFormHelper.ShowError(ex);
            }
        }

        private void btnBase64Clear_Click(object sender, EventArgs e)
        {
            rtxt_Base64_Orginal.Text = rtxt_Base64_Handled.Text = "";
        }
        #endregion

        #region base64 图片互转
        private void btnBase64Bmp_PickFile_Click(object sender, EventArgs e)
        {
            Base64BmpPicBmp();
        }

        private void btnBase64Bmp_ToBase64_Click(object sender, EventArgs e)
        {
            if (picBoxBase64Bmp.Image == null)
            {
                Base64BmpPicBmp();
            }
            try
            {
                rtxtBase64BmpStr.Text = "data:image/png;base64," + Base64Helper.ImgToBase64(picBoxBase64Bmp.Image);

            }
            catch (Exception ex)
            {
                WinFormHelper.ShowError(ex);
            }
        }

        private void btnBase64Bmp_ToBmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtxtBase64BmpStr.Text))
            {
                rtxtBase64BmpStr.Focus();
            }
            else
            {
                try
                {
                    picBoxBase64Bmp.Image.Dispose();
                    picBoxBase64Bmp.Image = null;
                    picBoxBase64Bmp.Image = Base64Helper.Base64ToImg(rtxtBase64BmpStr.Text.Replace("data:image/png;base64,", ""));
                }
                catch (Exception ex)
                {
                    WinFormHelper.ShowError(ex);
                }
            }
        }


        private void btnBase64BmpClear_Click(object sender, EventArgs e)
        {
            txtBase64Bmp_FilePath.Text = rtxtBase64BmpStr.Text = "";
            picBoxBase64Bmp.Image = null;
        }


        public void Base64BmpPicBmp()
        {
            //初始化一个OpenFileDialog类
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择文件的后缀名
                string extension = Path.GetExtension(fileDialog.FileName);
                //声明允许的后缀名
                string[] str = new string[] { ".gif", ".jpge", ".jpg", ".png", ".bmp" };
                if (!((IList)str).Contains(extension))
                {
                    MessageBox.Show("仅能选择gif,jpge,jpg,png,bmp格式的图片！");
                }
                else
                {
                    picBoxBase64Bmp.ImageLocation = fileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                    txtBase64Bmp_FilePath.Text = fileInfo.FullName;
                }
            }
        }
        #endregion 
        #endregion

        #region 加密解密
        #region Md5加密
        private void btnMd5Encrypt_Click(object sender, EventArgs e)
        {
            var str = rtxtMd5Txt.Text;
            if (string.IsNullOrWhiteSpace(str))
            {
                rtxtMd5Txt.Focus();
            }
            else
            {
                var _16Str = SecurityHelper.GetMd5_16(str);
                var _32Str = SecurityHelper.GetMd5_32(str);
                lbMd5Status.Text = "加密完成";
                txtMd5_16Lowcase.Text = _16Str.ToLower();
                txtMd5_16Uppercase.Text = _16Str;
                txtMd5_32Lowcase.Text = _32Str.ToLower();
                txtMd5_32Uppercase.Text = _32Str;
            }
        }

        private void btnMd5Clear_Click(object sender, EventArgs e)
        {
            rtxtMd5Txt.Text = txtMd5_16Lowcase.Text = txtMd5_16Uppercase.Text = txtMd5_32Lowcase.Text = txtMd5_32Uppercase.Text = lbMd5Status.Text = "";
        }
        #endregion

        #region 对称加解密
        private void btnSymmetricEncryption_Clear_Click(object sender, EventArgs e)
        {
            rtxtSymmetricEncryptionOrginal.Text = rtxtSymmetricEncryptionHandled.Text = txtSymmetricEncryptionKey.Text = "";
        }

        private void btnSymmetricEncryption_Encrypt_Click(object sender, EventArgs e)
        {
            var orginal = rtxtSymmetricEncryptionOrginal.Text;
            if (string.IsNullOrWhiteSpace(orginal))
            {
                rtxtSymmetricEncryptionOrginal.Focus();
                return;
            }
            var key = txtSymmetricEncryptionKey.Text;
            if (string.IsNullOrWhiteSpace(key))
            {
                txtSymmetricEncryptionKey.Focus();
                return;
            }
            try
            {
                switch (Convert.ToInt32(cmbSymmetricEncryptionEncryptMethod.SelectedValue))
                {
                    case 1: // AES128
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.AES_128.Encrypt(orginal, key);
                        break;
                    case 2: // AES256
                        if (key.Length != 32)
                        {
                            rtxtSymmetricEncryptionHandled.Text = "密钥必须为32位！";
                            break;
                        }
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.AES_256.Encrypt(orginal, key);
                        break;
                    case 3: // DES
                        if (key.Length != 8)
                        {
                            rtxtSymmetricEncryptionHandled.Text = "密钥必须为8位！";
                            break;
                        }
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.DES.Encrypt(orginal, key);
                        break;
                    case 4: // RC4
                        using (SecurityHelper.RC4Crypt crypter = new SecurityHelper.RC4Crypt(key))
                        {
                            rtxtSymmetricEncryptionHandled.Text = crypter.Encrypt(orginal);
                        }
                        break;
                    case 5: // Rabbit
                        break;
                    case 6: // TripleDES
                        if (key.Length != 24)
                        {
                            rtxtSymmetricEncryptionHandled.Text = "密钥必须为24位！";
                            break;
                        }
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.TripleDES.Encrypt(orginal, key);
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                WinFormHelper.ShowError(ex);
            }
        }

        private void btnSymmetricEncryption_Dencrypt_Click(object sender, EventArgs e)
        {
            var orginal = rtxtSymmetricEncryptionOrginal.Text;
            if (string.IsNullOrWhiteSpace(orginal))
            {
                rtxtSymmetricEncryptionOrginal.Focus();
                return;
            }
            var key = txtSymmetricEncryptionKey.Text;
            if (string.IsNullOrWhiteSpace(key))
            {
                txtSymmetricEncryptionKey.Focus();
                return;
            }
            try
            {
                switch (Convert.ToInt32(cmbSymmetricEncryptionEncryptMethod.SelectedValue))
                {
                    case 1: // AES128
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.AES_128.Decrypt(orginal, key);
                        break;
                    case 2: // AES256
                        if (key.Length != 32)
                        {
                            rtxtSymmetricEncryptionHandled.Text = "密钥必须为32位！";
                            break;
                        }
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.AES_256.Decrypt(orginal, key);
                        break;
                    case 3: // DES
                        if (key.Length != 8)
                        {
                            rtxtSymmetricEncryptionHandled.Text = "密钥必须为8位！";
                            break;
                        }
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.DES.Decrypt(orginal, key);
                        break;
                    case 4: // RC4
                        using (SecurityHelper.RC4Crypt crypter = new SecurityHelper.RC4Crypt(key))
                        {
                            rtxtSymmetricEncryptionHandled.Text = crypter.Decrypt(orginal);
                        }
                        break;
                    case 5: // Rabbit
                        break;
                    case 6: // TripleDES
                        rtxtSymmetricEncryptionHandled.Text = SecurityHelper.TripleDES.Decrypt(orginal, key);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                WinFormHelper.ShowError(ex);
            }
        }

        #endregion 

        #region Sha加密
        private void btnSha1Encrypt_Click(object sender, EventArgs e)
        {
            if (CheckShaEncryptSource())
            {
              rtxtShaEncryptTarget.Text = SecurityHelper.SHA1_Encrypt(rtxtShaEncryptSource.Text);
            
            }
        }

        private void btnSha256Encrypt_Click(object sender, EventArgs e)
        {
            if (CheckShaEncryptSource())
            {
                rtxtShaEncryptTarget.Text = SecurityHelper.SHA256_Encrypt(rtxtShaEncryptSource.Text);

            }
        }

        private void btnSha384Encrypt_Click(object sender, EventArgs e)
        {
            if (CheckShaEncryptSource())
            {
                rtxtShaEncryptTarget.Text = SecurityHelper.SHA384_Encrypt(rtxtShaEncryptSource.Text);

            }
        }

        private void btnSha512Encrypt_Click(object sender, EventArgs e)
        {
            if (CheckShaEncryptSource())
            {
                rtxtShaEncryptTarget.Text = SecurityHelper.SHA512_Encrypt(rtxtShaEncryptSource.Text);

            }
        }

        private void btnShaEncryptClear_Click(object sender, EventArgs e)
        {
            rtxtShaEncryptTarget.Text = rtxtShaEncryptSource.Text = string.Empty;
        }

        private bool CheckShaEncryptSource()
        {
            if (string.IsNullOrEmpty(rtxtShaEncryptSource.Text))
            {
                rtxtShaEncryptSource.Focus();
                return false;
            }
            return true;
        }
        #endregion
        #endregion

        #region 字符串相关
        #region 批量替换
        private void btnBatchReplaceClear_Click(object sender, EventArgs e)
        {
            rtxtBatchReplaceStr.Text = txtBatchReplaceNewStr.Text = txtBatchReplaceOldStr.Text = "";
        }

        private void btnBatchReplace_Click(object sender, EventArgs e)
        {
            var oldStr = txtBatchReplaceOldStr.Text;
            var orginalTxt = rtxtBatchReplaceStr.Text;

            if (string.IsNullOrWhiteSpace(orginalTxt))
            {
                rtxtBatchReplaceStr.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(oldStr))
            {
                txtBatchReplaceOldStr.Focus();
                return;
            }

            rtxtBatchReplaceStr.Text = orginalTxt.Replace(oldStr, txtBatchReplaceNewStr.Text);
        }
        #endregion 

        #region 截取
        private void txtJieQuStartIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtJieQueEndIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtJieQueEndIndex_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnJieQu_Click(object sender, EventArgs e)
        {
            var str = rtxtJieQuStr.Text;
            if (string.IsNullOrWhiteSpace(str))
            {
                rtxtJieQuStr.Focus();
            }
            else
            {
                var startIndex = string.IsNullOrWhiteSpace(txtJieQuStartIndex.Text) ? 1 : Convert.ToInt32(txtJieQuStartIndex.Text);

                if (ckbJieQuNoSpace.Checked)
                {
                   str =  str.Replace(" ", "");
                }
                if (ckbJieQuNoNewLines.Checked)
                {
                   str =  str.Replace("\n", "").Replace("\r","").Replace("\r\n","");
                }

                if (string.IsNullOrWhiteSpace(txtJieQuLength.Text))
                {
                    rtxtJieQuStr.Text = str.Substring(startIndex-1);
                }
                else
                {
                    var length = Convert.ToInt32(txtJieQuLength.Text)>str.Length?str.Length:Convert.ToInt32(txtJieQuLength.Text);
                    rtxtJieQuStr.Text =str.Substring(startIndex-1,length);
                }
            }
        }
        #endregion

       

      
        #endregion

        #region 图片

        #region 图片批量下载

        private static Utils.ImageDownLoadEngine.ImageDownLoader imgDownloader;
        private static ConcurrentQueue<string> imgBatchDownloadQueue;
        private static Utils.ImageDownLoadEngine.FileAnalysisBehavior fileAnalysisiBehavior;
        private static Utils.ImageDownLoadEngine.ImageDownloadBehavior imgDownloadBehavior;

        private void btnImgBatchDownLoadAction_Click(object sender, EventArgs e)
        {
            SetSaveFolder();
            ImgBatchDownloadProgressEvent(null, "图片保存路径为：" + savePath);
            if (string.IsNullOrWhiteSpace(txtImgBathDownLoadFilePath.Text))
            {
                ImgBatchDownLoadPickFile();
            }

            if(imgBatchDownloadQueue == null)
            {
                imgBatchDownloadQueue = new ConcurrentQueue<string>();
            }
            imgBatchDownloadQueue.Clear();

            if (fileAnalysisiBehavior == null || (fileAnalysisiBehavior != null && fileAnalysisiBehavior.FilePath != savePath))
            {
                fileAnalysisiBehavior = new Utils.ImageDownLoadEngine.FileAnalysisBehavior(txtImgBathDownLoadFilePath.Text, imgBatchDownloadQueue);
            }
            if (imgDownloadBehavior == null)
            {
                imgDownloadBehavior = new Utils.ImageDownLoadEngine.ImageDownloadBehavior(Convert.ToInt32(txtImgBatchDownloadThreadCount.Text), imgBatchDownloadQueue, savePath);
            
            }
            if (imgDownloader == null)
            {
                imgDownloader = new Utils.ImageDownLoadEngine.ImageDownLoader(imgBatchDownloadQueue, fileAnalysisiBehavior, imgDownloadBehavior);
            }

            imgDownloader.Action();

            btnImgBatchDownLoadAction.Enabled = false;

            btnImgBatchDownloadPause.Enabled = true;
        }

        private void txtImgBatchDownloadThreadCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnImgBatchDownloadPause_Click(object sender, EventArgs e)
        {

            imgDownloader.End();

            btnImgBatchDownLoadAction.Enabled = true;

            btnImgBatchDownloadPause.Enabled = false;
            ImgBatchDownloadProgressEvent(null, "--------------------------------------[任务停止]--------------------------------------");
        }

        private void btnImgBatchDownLoadPickFile_Click(object sender, EventArgs e)
        {
            ImgBatchDownLoadPickFile();
        }
        public void ImgBatchDownLoadPickFile()
        {
            //初始化一个OpenFileDialog类
            OpenFileDialog fileDialog = new OpenFileDialog();
            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择文件的后缀名
                string extension = Path.GetExtension(fileDialog.FileName);
                //声明允许的后缀名
                string[] str = new string[] { ".txt" };
                if (!((IList)str).Contains(extension))
                {
                    MessageBox.Show("仅能选择txt格式的文件！");
                }
                else
                {
                    picBoxBase64Bmp.ImageLocation = fileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                    txtImgBathDownLoadFilePath.Text = fileInfo.FullName;
                    ImgBatchDownloadProgressEvent(null, "已选择文件：" + fileInfo.FullName);
                }
            }
        }


        private static object ImgBatchDownLoadLockObj = new object();
      
        /// <summary>
        /// 进度通知
        /// </summary>
        /// <param name="text">文本</param>
        private void ImgBatchDownloadProgressEvent(object obj, string text)
        {
            lock (ImgBatchDownLoadLockObj)
            {
                rtxtImgBatchDownloadStatus.AppendText(text + "\r\n");
            }
        }
       
        #endregion

        #endregion

       

      private void SetSaveFolder()
      {
          fbdSavePath.ShowDialog();
          string path = fbdSavePath.SelectedPath;
           if (!string.IsNullOrEmpty(path))
            {
                savePath = fbdSavePath.SelectedPath + "\\";
            }
            else
            {
               savePath = Application.StartupPath + savePath;
            }
      
      }

      private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
      {
          if (imgDownloader != null)
          {
              imgDownloader.End();
          }
      }
    }
}
