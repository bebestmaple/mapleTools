using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.TaskEx;

namespace Utils
{
    public class ImgCutter : BaseBehavior
    {

        #region 属性
        /// <summary>
        /// 进度通知事件
        /// </summary>
        public  Progress<int> CutProgress = new Progress<int>();

        /// <summary>
        /// 线程管理器
        /// </summary>
        private TaskSupervisor TaskSupervisor { get; set; }

        /// <summary>
        /// 裁剪队列
        /// </summary>
        private ConcurrentQueue<CutterSetting> CuttingSettingQueue = new ConcurrentQueue<CutterSetting>();

        /// <summary>
        /// 图片处理总数
        /// </summary>
        private int TotalCount { get; set; }

        /// <summary>
        /// 最大线程数
        /// </summary>
        private int TaskMaxNum { get; set; }

        #endregion

        public ImgCutter(ConcurrentBag<string> imgPathQueue, int width, int height, string rootPath, string savePath = "", bool isSetWatermark = false, WatermarkTypeEnum watermarkType = WatermarkTypeEnum.Text, WatermarkPositionEnum watermarkPosition = WatermarkPositionEnum.RightButtom, string watermarkTextOrImgPath = "", int taskMaxNum = 0)
        {
            imgPathQueue.AsParallel().ForAll(imgPath =>
            {
                CuttingSettingQueue.Enqueue(new CutterSetting
                {
                    CutHeight = height,
                    CutWidth = width,
                    ImgPath = imgPath,
                    RootPath = rootPath,
                    SavePath = savePath,
                    IsSetWatermark = isSetWatermark,
                    WatermarkPosition = watermarkPosition,
                    WatermarkType = watermarkType,
                    WatermarkTextOrPath = watermarkTextOrImgPath

                });

            });
            TotalCount = CuttingSettingQueue.Count;
            TaskMaxNum = taskMaxNum <= 0 ? Environment.ProcessorCount + 2 : taskMaxNum;
            TaskSupervisor = new TaskSupervisor(TaskMaxNum);
        }

        public override async Task Action()
        {
            CancelTokenSource = new CancellationTokenSource();

            Progress("[图片裁剪器]开始处理...");

            TaskSupervisor.Add(Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!CancelTokenSource.IsCancellationRequested)
                    {
                        TaskSupervisor.DisposeInvalidTask();
                        if (CuttingSettingQueue.Count < 1)
                        {
                            Task.Delay(1000);
                            continue;
                        }

                        CuttingSettingQueue.TryDequeue(out var waitProcessImg);

                        if (!CancelTokenSource.IsCancellationRequested)
                        {
                            if (!TaskSupervisor.Add(new Task(new Action<object>(x =>
                            {
                                if (!CancelTokenSource.IsCancellationRequested)
                                {
                                    var cutterSetting = x as CutterSetting;

                                    if (!CancelTokenSource.IsCancellationRequested)
                                    {
                                        Progress($"[图片裁剪器]正在裁剪图片：{cutterSetting.ImgPath}");

                                        var savePath = cutterSetting.SavePath;
                                        var imgExtensionName = Path.GetExtension(cutterSetting.ImgPath);
                                        if (string.IsNullOrEmpty(savePath))
                                        {
                                            savePath = cutterSetting.ImgPath;
                                        }
                                        if (cutterSetting.ImgPath != savePath)
                                        {
                                            savePath = cutterSetting.ImgPath.Replace(cutterSetting.RootPath, savePath);
                                        }


                                        try
                                        {
                                            #region 裁剪
                                            var cutImg = ImgHelper.CutImg(cutterSetting.ImgPath, cutterSetting.CutWidth, cutterSetting.CutHeight); 
                                            #endregion

                                            #region 加水印
                                            if (cutterSetting.IsSetWatermark)
                                            {
                                                switch (cutterSetting.WatermarkType)
                                                {
                                                    case WatermarkTypeEnum.Image:
                                                        try
                                                        {
                                                            var watermarkImg = Image.FromFile(cutterSetting.WatermarkTextOrPath);
                                                            if (watermarkImg != null)
                                                            {
                                                                cutImg = Image.FromStream(WaterMarkHelper.AddWatermark(cutImg, imgExtensionName, watermarkImg, cutterSetting.WatermarkPosition));
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Progress($"[图片裁剪器]发生异常：{ex}");
                                                        }

                                                        break;
                                                    case WatermarkTypeEnum.Text:
                                                        cutImg = Image.FromStream(WaterMarkHelper.AddWatermark(cutImg, imgExtensionName, cutterSetting.WatermarkTextOrPath, Color.Black, cutterSetting.WatermarkPosition));
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            } 
                                            #endregion

                                            if (cutImg != null)
                                            {
                                                switch (imgExtensionName)
                                                {
                                                    case ".jpg":
                                                        ImageCodecInfo ici = ImgHelper.GetEncoderInfo("image/jpeg");
                                                        if (ici != null)
                                                        {
                                                            EncoderParameters ep = new EncoderParameters(1);
                                                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                                                            cutImg.Save(savePath, ici, ep);
                                                            ep.Dispose();
                                                            ep = null;
                                                            ici = null;
                                                        }
                                                        else
                                                        {
                                                            cutImg.Save(savePath, ImageFormat.Jpeg);
                                                        }
                                                        break;
                                                    case ".bmp":
                                                        cutImg.Save(savePath, ImageFormat.Bmp);
                                                        break;
                                                    case ".gif":
                                                        cutImg.Save(savePath, ImageFormat.Gif);
                                                        break;
                                                    case ".png":
                                                        cutImg.Save(savePath, ImageFormat.Png);
                                                        break;
                                                    default:
                                                        cutImg.Save(savePath, ImageFormat.Jpeg);
                                                        break;
                                                }

                                                CutterProgress(TotalCount - CuttingSettingQueue.Count);
                                            }
                                            else
                                            {
                                                Progress($"[图片裁剪器][{cutterSetting.ImgPath}]裁剪时发生未知错误，未能实施裁剪");
                                                CuttingSettingQueue.Enqueue(cutterSetting);
                                            }

                                            cutImg.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            Progress($"[图片裁剪器]发生异常：{ex}");
                                        }
                                    }
                                }
                                else
                                {
                                    Thread.CurrentThread.Abort();
                                }


                            }), waitProcessImg, CancelTokenSource.Token)))
                            {
                                CuttingSettingQueue.Enqueue(waitProcessImg);
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
                    Progress($"[图片裁剪器]发生异常：{ex}");
                }


            }, CancelTokenSource.Token));
        }


        #region 结束
        public void End()
        {
            //// 通知关闭
            CancelTokenSource.Cancel();
            TaskSupervisor.DisposeAllTask();

        }
        #endregion


        #region 进度同步
        /// <summary>
        /// 进度同步
        /// </summary>
        /// <param name="message">同步消息</param>
        protected void CutterProgress(int state)
        {
            if (TaskProgress != null)
            {
                IProgress<int> progress = CutProgress;
                progress.Report(state);
            }
        } 
        #endregion


        #region 裁剪设置
        public class CutterSetting
        {
            public string ImgPath { get; set; }

            public int CutWidth { get; set; }

            public int CutHeight { get; set; }

            public string SavePath { get; set; }

            public string RootPath { get; set; }

            public bool IsSetWatermark { get; set; }

            public WatermarkPositionEnum WatermarkPosition { get; set; }

            public WatermarkTypeEnum WatermarkType { get; set; }

            public string WatermarkTextOrPath { get; set; }
        } 
        #endregion
    }
}
