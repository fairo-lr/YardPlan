using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using O2S.Components.PDFRender4NET;

public partial class testPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string t_date = String.Empty;
        if (this.Request.QueryString["date"] != null)
        {
            t_date = this.Request.QueryString["date"].ToString() + "";
        }else{
            t_date = DateTime.Now.ToString("yyyy-MM-dd");
        }
        //t_date = "2013-03-06";

        FontFactory.RegisterDirectory(@"C:\WINDOWS\Fonts\");
        Document t_doc = new Document(PageSize.A4,0,0,0,0);
        String t_path = Server.MapPath("pdfs");
        String t_imgPath = Server.MapPath("images");
        MemoryStream t_memory = new MemoryStream();
        float t_rate = 2f;
        float t_newrate = 0.508378988f;
        float t_incraRate = 16.5493333333333333f;
        float t_holds = 75;
        float t_dateRate = 2.5f;
        float t_halfWidth = 0f;
        bool  t_isMoved = false;
        DateTime t_Weekdate = Convert.ToDateTime(t_date);
        string t_week = t_Weekdate.DayOfWeek.ToString();
        #region --
        //PDF TO IMAGE
        /*
        Acrobat.CAcroPDDoc t_pdfDoc;
        Acrobat.CAcroPDPage t_pdfPage;
        Acrobat.CAcroRect t_pdfRect;
        Acrobat.CAcroPoint t_pdfPoint;
        t_pdfDoc = (Acrobat.CAcroPDDoc)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.PDDoc", "");
        int t_ret = t_pdfDoc.Open(@"C:\Documents and Settings\colin\桌面\berth.pdf");
        t_pdfPage = (Acrobat.CAcroPDPage)t_pdfDoc.AcquirePage(0);
        t_pdfPoint = (Acrobat.CAcroPoint)t_pdfPage.GetSize();
        t_pdfRect = (Acrobat.CAcroRect)Microsoft.VisualBasic.Interaction.CreateObject("AcroExch.Rect", "");
        t_pdfRect.Left = 0;
        t_pdfRect.right = t_pdfPoint.x;
        t_pdfRect.Top = 0;
        t_pdfRect.bottom = t_pdfPoint.y;
        t_pdfPage.CopyToClipboard(t_pdfRect, 0, 0, 100);
        IDataObject clipboardData = Clipboard.GetDataObject();
        if (clipboardData.GetDataPresent(DataFormats.Bitmap))
        {
            Bitmap pdfbitmap = (Bitmap)clipboardData.GetData(DataFormats.Bitmap);
            int thumnailWidth = 45;
            int thumnailHieght = 59;
            String templateFile = @"C:\Documents and Settings\colin\桌面\berth.gif";
            Bitmap templateBitmap = new Bitmap(templateFile);
            System.Drawing.Image pdfImage = pdfbitmap.GetThumbnailImage(thumnailWidth, thumnailHieght, null, IntPtr.Zero);
            Bitmap thumbailBitmap = new Bitmap(thumnailWidth + 7, thumnailHieght + 7,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            templateBitmap.MakeTransparent();

            using(Graphics thumbnailGraphics = Graphics.FromImage(templateBitmap))
            {
                // Draw rendered pdf image to new blank bitmap
                thumbnailGraphics.DrawImage(pdfImage, 2, 2, thumnailWidth, thumnailHieght);

                // Draw template outline over the bitmap (pdf with show through the transparent area)
                thumbnailGraphics.DrawImage(templateBitmap, 0, 0);

                // Save as .png file
                thumbailBitmap.Save(@"C:\Documents and Settings\colin\桌面\berth", System.Drawing.Imaging.ImageFormat.Png);
            }
            t_pdfDoc.Close();
            Marshal.ReleaseComObject(t_pdfPage);
            Marshal.ReleaseComObject(t_pdfRect);
            Marshal.ReleaseComObject(t_pdfDoc);
        }
        */
        //--------------------------
        #endregion

        #region 绘制船舶
        try
        {
        BaseFont bf0 = BaseFont.CreateFont("C:/WINDOWS/Fonts/SIMhei.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        iTextSharp.text.Font t_font = new iTextSharp.text.Font(bf0);
        //获取实例
        PdfWriter t_writer = PdfWriter.GetInstance(t_doc, new FileStream(t_path + "/berth.pdf", FileMode.Create));
        //PdfWriter t_writer = PdfWriter.GetInstance(t_doc, t_memory);
        t_doc.Open();
        PdfContentByte t_pcb = t_writer.DirectContent; 
        
        //添加一个段落
        //获取一张图
        iTextSharp.text.Image t_berth = iTextSharp.text.Image.GetInstance(t_imgPath + "/2013berth.png");
        t_berth.ScalePercent(38, 38);
        t_berth.RotationDegrees = -90f;
        t_doc.Add(t_berth);

        iTextSharp.text.Image t_dateImage = iTextSharp.text.Image.GetInstance(t_imgPath + "/" + t_week + ".png");
        t_dateImage.ScalePercent(38, 60);
        t_dateImage.RotationDegrees = -90f;
        t_dateImage.SetAbsolutePosition(120f, 820f);
        t_doc.Add(t_dateImage);


        List<VslModel> t_lstVM = vesselDB.GetLstVsl(t_date);
        List<iTextSharp.text.Rectangle> t_lstRect = new List<iTextSharp.text.Rectangle>();
		
        foreach (VslModel t_vm in t_lstVM)
        {
            if (t_vm.IsPresent== "Y")
            {

                //判断颜色
                if (t_vm.Intrade == "Y" || t_vm.IsFinish.ToUpper() == "Y")
                {
                    t_pcb.SetRGBColorFill(0x99, 0xcc, 0xff);
                }
                else
                {
                    t_pcb.SetRGBColorFill(0xff, 0x99, 0xcc);
                }
                float t_stpstX = 0f;
                float t_stpstY = 0f;
                if (t_vm.BreDire == "L")
                {
                    t_stpstX = 135f + float.Parse(t_vm.DayOrder) * (t_dateRate + 1.68f / (200 - float.Parse(t_vm.DayOrder)));
                    t_stpstY = float.Parse(t_vm.EdPost) * (t_newrate + (t_holds - float.Parse(t_vm.EdPost) / t_incraRate) / 5000) + 10;
                    t_halfWidth = float.Parse(t_vm.Vbreadth) * t_newrate / 2;

                    iTextSharp.text.Rectangle t_rec = new iTextSharp.text.Rectangle(0, 0);
                    t_rec.Left = t_stpstX - t_halfWidth;
                    t_rec.Bottom = t_stpstY - float.Parse(t_vm.Vlength) * t_newrate;
                    t_rec.Right = t_halfWidth * 2;
                    t_rec.Top = float.Parse(t_vm.Vlength) * t_newrate;
                    while (IsIntersecting(t_rec, t_lstRect))
                    {
                        t_stpstX = t_stpstX + 3f;
                        t_rec.Left = t_stpstX - t_halfWidth;
                        t_isMoved = true;
                    }
                    t_lstRect.Add(t_rec);

                    t_pcb.MoveTo(t_stpstX, t_stpstY);
                    t_pcb.LineTo(t_stpstX - t_halfWidth, t_stpstY - 10f);
                    t_pcb.LineTo(t_stpstX - t_halfWidth, t_stpstY - float.Parse(t_vm.Vlength) * t_newrate);
                    t_pcb.LineTo(t_stpstX + t_halfWidth, t_stpstY - float.Parse(t_vm.Vlength) * t_newrate);
                    t_pcb.LineTo(t_stpstX + t_halfWidth, t_stpstY - 10f);
                    t_pcb.Fill();
                    t_pcb.SaveState();
                    t_pcb.SetFontAndSize(bf0, 5);
                    t_pcb.SetColorFill(BaseColor.BLACK);
                    t_pcb.BeginText();
                    if (float.Parse(t_vm.Vlength) > 180)
                    {
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.VCHname + " " + t_vm.Voyage + "  " + t_vm.Vlength + "M / " + t_vm.Bkstpost + "M", t_stpstX + t_halfWidth - 4, t_stpstY - 10f, -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.SlnName + "  LP:" + t_vm.PrePort + "  NP:" + t_vm.NextPort + "  ETB:" + t_vm.BreTm + "  ETD:" + t_vm.DptTm, t_stpstX + t_halfWidth - 10, t_stpstY - 10f, -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "▎" + t_vm.EdPost + "M", t_stpstX + t_halfWidth - 16, t_stpstY - 10f, -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.StPost + "M ▎", t_stpstX + t_halfWidth - 16, t_stpstY - float.Parse(t_vm.Vlength) * t_newrate + 16, -90f);
                        if (t_vm.Bkstpost != "0" && t_vm.EdPost != null && t_vm.Bkstpost != null)
                        {
                            t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, (float.Parse(t_vm.EdPost) - float.Parse(t_vm.Bkstpost)) + "M ▎", t_stpstX + t_halfWidth - 16, t_stpstY - float.Parse(t_vm.Bkstpost) * t_newrate + 15, -90f);
                        }
                    }
                    else
                    {
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.VCHname + " " + t_vm.Voyage, t_stpstX - 2, t_stpstY - 10f, -90f);

                    }
                    t_pcb.EndText();
                    t_pcb.RestoreState();
                    t_isMoved = false;
                }
                else
                {
                    t_stpstX = 135f + float.Parse(t_vm.DayOrder) * (t_dateRate + 1.68f / (200 - float.Parse(t_vm.DayOrder)));
                    t_stpstY = float.Parse(t_vm.StPost) * (t_newrate + (t_holds - float.Parse(t_vm.StPost) / t_incraRate) / 5000) + 10;
                    t_halfWidth = float.Parse(t_vm.Vbreadth) * t_newrate / 2;

                    iTextSharp.text.Rectangle t_rec = new iTextSharp.text.Rectangle(0, 0);
                    t_rec.Left = t_stpstX - t_halfWidth;
                    t_rec.Bottom = t_stpstY;
                    t_rec.Right = t_halfWidth * 2;
                    t_rec.Top = float.Parse(t_vm.Vlength) * t_newrate;
                    while (IsIntersecting(t_rec, t_lstRect))
                    {
                        t_stpstX = t_stpstX + 3f;
                        t_rec.Left = t_stpstX - t_halfWidth;
                        t_isMoved = true;
                    }
                    t_lstRect.Add(t_rec);

                    t_pcb.MoveTo(t_stpstX, t_stpstY);
                    t_pcb.LineTo(t_stpstX - t_halfWidth, t_stpstY + 10f);
                    t_pcb.LineTo(t_stpstX - t_halfWidth, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate);
                    t_pcb.LineTo(t_stpstX + t_halfWidth, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate);
                    t_pcb.LineTo(t_stpstX + t_halfWidth, t_stpstY + 10f);
                    t_pcb.Fill();
                    t_pcb.SaveState();
                    t_pcb.SetFontAndSize(bf0, 5);
                    t_pcb.SetColorFill(BaseColor.BLACK);
                    t_pcb.BeginText();
                    if (float.Parse(t_vm.Vlength) > 180)
                    {
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.VCHname + " " + t_vm.Voyage + "  " + t_vm.Vlength + "M / " + t_vm.Bkstpost + "M", t_stpstX + t_halfWidth - 4, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate, -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.SlnName + "  LP:" + t_vm.PrePort + "  NP:" + t_vm.NextPort + "  ETB:"+t_vm.BreTm+"  ETD:"+t_vm.DptTm, t_stpstX + t_halfWidth - 10, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate, -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "▎" + t_vm.EdPost + "M", t_stpstX + t_halfWidth - 16, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate , -90f);
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.StPost + "M ▎", t_stpstX + t_halfWidth - 16, t_stpstY + 20, -90f);

                        if (t_vm.Bkstpost != "0" && t_vm.Bkstpost != null && t_vm.StPost != null)
                        {
                            t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "▎" + (float.Parse(t_vm.Bkstpost) + float.Parse(t_vm.StPost)) + "M", t_stpstX + t_halfWidth - 16, t_stpstY + float.Parse(t_vm.Bkstpost) * t_newrate + 2, -90f);
                        }
                    }
                    else
                    {
                        t_pcb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, t_vm.VCHname + " " + t_vm.Voyage, t_stpstX - 2, t_stpstY + float.Parse(t_vm.Vlength) * t_newrate, -90f);

                    }
                    t_pcb.EndText();
                    t_pcb.RestoreState();
                    t_isMoved = false;
                }
            }
        }
		
        //测试基准
        //t_pcb.MoveTo(0, 10);
        //t_pcb.LineTo(600, 10);
        //t_pcb.Stroke();
        
        t_doc.Close();

        //this.Response.Clear();
        //this.Response.AddHeader("Content-Disposition", "attachment;filename=berth.pdf");
        //this.Response.ContentType = "application/octet-stream";
        //this.Response.OutputStream.Write(t_memory.GetBuffer(), 0, t_memory.GetBuffer().Length);
        //this.Response.OutputStream.Flush();
        //this.Response.OutputStream.Close();
        //this.Response.Flush();
        //this.Response.End();
         RotateImg(@"YardPlan\images\A1.Jpeg", 90);
		 ConvertPDF2Image(t_path + @"\berth.pdf", t_imgPath + @"\", "A", 1, 5, ImageFormat.Jpeg, Definition.Six);
        #endregion
		}
        catch (System.IO.IOException ex)
        {
			//PDF另一个程序正在使用异常直接忽略
        }
		catch (System.OutOfMemoryException ex)
        {
			//内存不足的异常直接忽略 
        }
        // ConvertPDF2Image(t_path + @"\berth.pdf", t_imgPath + @"\", "A", 1, 5, ImageFormat.Jpeg, Definition.Nine);
        //PdfToImages(t_path + @"\berth.pdf", t_imgPath + @"\");
        
        Random rand = new Random();
        this.imgBerath.ImageUrl = @"images\c1.Jpg?rand=" + rand.Next(1, 100).ToString();
        
        //PdfToImages(t_path + @"\berth.pdf", t_imgPath + @"\");

    }
    public void RotateImg(string p_img, int p_angle)
    {
        string t_path = HttpRuntime.AppDomainAppPath.ToString();
        System.Drawing.Image t_img = Bitmap.FromFile(t_path +  p_img);
        p_angle = p_angle % 360;
        //弧度转换
        double radian = p_angle * Math.PI / 180.0;
        double cos = Math.Cos(radian);
        double sin = Math.Sin(radian);

        //原图的宽和高
        int w = (int)(t_img.Width);
        int h = (int)(t_img.Height);
        int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
        int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));


        //目标位图
		//Response.Write(W.ToString()+"--"+H.ToString());
		//Response.End();
        Bitmap dsImage = new Bitmap(W, H);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //计算偏移量
        Point Offset = new Point((W - w) / 2, (H - h) / 2);
        //构造图像显示区域：让图像的中心与窗口的中心点一致
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Offset.X, Offset.Y, w, h);
        Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        g.TranslateTransform(center.X, center.Y);
        g.RotateTransform(360 - p_angle);
        //恢复图像在水平和垂直方向的平移
        g.TranslateTransform(-center.X, -center.Y);
        g.DrawImage(t_img, rect);
        //重至绘图的所有变换
        g.ResetTransform();
        g.Save();
        g.Dispose();
        //保存旋转后的图片
        t_img.Dispose();
        dsImage.Save(t_path + @"YardPlan\images\c1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
		dsImage.Dispose();
        this.imgBerath.Width = W;
        this.imgBerath.Height = H;
    }
    private bool IsIntersecting(iTextSharp.text.Rectangle p_rec, List<iTextSharp.text.Rectangle> p_lstRec)
    {
        float t_centerX = p_rec.Left + p_rec.Right / 2;
        float t_centerY = p_rec.Bottom + p_rec.Top / 2;

        foreach (iTextSharp.text.Rectangle t_rec in p_lstRec)
        {
            float t_iterX = t_rec.Left + t_rec.Right / 2;
            float t_iterY = t_rec.Bottom + t_rec.Top / 2;

            if (Math.Abs(t_centerX - t_iterX) <= p_rec.Right / 2 + t_rec.Right / 2 &&
                Math.Abs(t_centerY - t_iterY) < p_rec.Top / 2 + t_rec.Top / 2)
            {
                return true;
            }
        }
        return false;
    }

    public enum Definition
    {
        One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
    }

    /// <summary>
    /// 将PDF文档转换为图片的方法
    /// </summary>
    /// <param name="pdfInputPath">PDF文件路径</param>
    /// <param name="imageOutputPath">图片输出路径</param>
    /// <param name="imageName">生成图片的名字</param>
    /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
    /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
    /// <param name="imageFormat">设置所需图片格式</param>
    /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
    public static void ConvertPDF2Image(string pdfInputPath, string imageOutputPath,
        string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
    {
        PDFFile pdfFile = PDFFile.Open(pdfInputPath);

        if (!Directory.Exists(imageOutputPath))
        {
            Directory.CreateDirectory(imageOutputPath);
        }

        // validate pageNum
        if (startPageNum <= 0)
        {
            startPageNum = 1;
        }

        if (endPageNum > pdfFile.PageCount)
        {
            endPageNum = pdfFile.PageCount;
        }

        if (startPageNum > endPageNum)
        {
            int tempPageNum = startPageNum;
            startPageNum = endPageNum;
            endPageNum = startPageNum;
        }

        // start to convert each page
		try
		{
			for (int i = startPageNum; i <= endPageNum; i++)
			{
				Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);
				pageImage.Save(imageOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
				pageImage.Dispose();
			}
		}
        catch (System.Exception ex)
        {
				
        }
        pdfFile.Dispose();
    }
    /// <summary>
    /// 将PDF文档转换为图片的方法
    /// </summary>
    /// <param name="pdfInputPath">PDF文件路径</param>
    /// <param name="imageOutputPath">图片输出路径</param>
    /// <param name="imageName">生成图片的名字</param>
    /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
    /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
    /// <param name="imageFormat">设置所需图片格式</param>
    /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
    /*
    public static void ConvertPDF2Image(string pdfInputPath, string imageOutputPath,
        string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
    {
        PDFWrapper pdfWrapper = new PDFWrapper();

        pdfWrapper.LoadPDF(pdfInputPath);

        if (!System.IO.Directory.Exists(imageOutputPath))
        {
            Directory.CreateDirectory(imageOutputPath);
        }

        // validate pageNum
        if (startPageNum <= 0)
        {
            startPageNum = 1;
        }

        if (endPageNum > pdfWrapper.PageCount)
        {
            endPageNum = pdfWrapper.PageCount;
        }

        if (startPageNum > endPageNum)
        {
            int tempPageNum = startPageNum;
            startPageNum = endPageNum;
            endPageNum = startPageNum;
        }

        // start to convert each page
        for (int i = startPageNum; i <= endPageNum; i++)
        {
            pdfWrapper.ExportJpg(imageOutputPath + imageName + i.ToString() + ".jpg", i, i, 150, 90);//这里可以设置输出图片的页数、大小和图片质量
            if (pdfWrapper.IsJpgBusy) { System.Threading.Thread.Sleep(50); }
        }
        ;
    }
    */
    
}
