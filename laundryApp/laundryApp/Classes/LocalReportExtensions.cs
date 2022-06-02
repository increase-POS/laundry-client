using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Reporting.WinForms;

namespace Microsoft.Reporting.WinForms
{
    public static class LocalReportExtensions
    {

        private static int m_currentPageIndex;
        private static IList<Stream> m_streams;
        private static PageSettings m_pageSettings;

        public static Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        public static void ExportbyPrinterName(string printerName, LocalReport report, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                PrintbyPrinterName(printerName);
            }
        }

        public static void ExportbyPrinterNameAndCopy(string printerName, LocalReport report, short copy, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                PrintbyPrinterNameAndCopy(printerName, copy);
            }
        }
        public static void Export(LocalReport report, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print();
            }
        }
        public static void ExportToPDF(LocalReport report, String FullPath)
        {/*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
          CultureInfo.InvariantCulture,
          "<DeviceInfo>" +
              "<OutputFormat>PDF</OutputFormat>" +

   /*
   "<PageWidth>{5}</PageWidth>" +
   "<PageHeight>{4}</PageHeight>" +

   "<MarginTop>{0}</MarginTop>" +
   "<MarginLeft>{1}</MarginLeft>" +
   "<MarginRight>{2}</MarginRight>" +
   "<MarginBottom>{3}</MarginBottom>" +

*/
   "<EmbedFonts>None</EmbedFonts>" +
          "</DeviceInfo>");
            /*
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            */
            //  byte[] renderedBytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            byte[] Bytes = report.Render(format: "PDF", deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try { 
            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                try
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                    stream.Close();

                }
                catch
                {

                }
                finally
                {
                    stream.Close();
                }
            }
            }
            catch { }

        }
        public static void ExportToWORD(LocalReport report, String FullPath)
        {

            string deviceInfo = "<DeviceInfo>" +
                    "  <OutputFormat>WORD</OutputFormat>" +

                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";


            //   byte[] renderedBytes = report.Render("WORD", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // byte[] Bytes = report.Render("Excel", deviceInfo);
            byte[] Bytes = report.Render(format: "WORD", deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);

            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }

        }

        public static void ExportToExcel(LocalReport report, String FullPath)
        {

            string deviceInfo = "<DeviceInfo>" +
                    "  <OutputFormat>Excel</OutputFormat>" +

                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";


            byte[] Bytes = report.Render("Excel", deviceInfo);
            // byte[] Bytes = report.Render("Excel", "", out contentType, out encoding, out extension, out streamIds, out warnings);

            //  byte[] Bytes = report.Render(format: "EXCEL", deviceInfo: deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try { 
            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                try
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                    stream.Close();

                }
                catch
                {

                }
                finally
                {
                    stream.Close();
                }

            }
        }
            catch { }
        }
        // Handler for PrintPageEvents
        public static void PrintPage(object sender, PrintPageEventArgs e)
        {
            Stream pageToPrint = m_streams[m_currentPageIndex];
            pageToPrint.Position = 0;

            // Load each page into a Metafile to draw it.
            using (Metafile pageMetaFile = new Metafile(pageToPrint))
            {
                Rectangle adjustedRect = new Rectangle(
                        e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                        e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                        e.PageBounds.Width,
                        e.PageBounds.Height);

                // Draw a white background for the report
                e.Graphics.FillRectangle(Brushes.White, adjustedRect);

                // Draw the report content
                e.Graphics.DrawImage(pageMetaFile, adjustedRect);

                // Prepare for next page.  Make sure we haven't hit the end.
                m_currentPageIndex++;
                e.HasMorePages = m_currentPageIndex < m_streams.Count;
            }
        }

        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }
            }
        }
        public static void PrintbyPrinterName(string printerName)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName == "" || printerName is null)
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }


                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }
            }
        }


        public static void PrintbyPrinterNameAndCopy(string printerName, short copy)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.Copies = copy;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName is null || printerName == "")
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }
            }
        }

        public static void PrintToPrinter(this LocalReport report)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            Export(report);
        }

        public static void PrintToPrinterbyName(this LocalReport report, string printerName)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterName(printerName, report);
        }
        public static void PrintToPrinterbyNameAndCopy(this LocalReport report, string printerName, short copy)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterNameAndCopy(printerName, report, copy);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        private static string ToInches(int hundrethsOfInch)
        {
            double inches = hundrethsOfInch / 100.0;
            return inches.ToString(CultureInfo.InvariantCulture) + "in";
        }

        // custom Print
        public static void customPrintToPrinterwh(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            customExportbyPrinterwh(printerName, report, copy, width, height);
        }
        public static void customPrintToPrinter(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            customExportbyPrinter(printerName, report, copy, width, height);
        }

        public static void customExportbyPrinter(string printerName, LocalReport report, short copy, int width, int height, bool print = true)
        {

            PaperSize paperSize = m_pageSettings.PaperSize;

            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +

                  "<PageWidth>{5}</PageWidth>" +
                  "<PageHeight>{4}</PageHeight>" +

                  "<MarginTop>{0}</MarginTop>" +
                  "<MarginLeft>{1}</MarginLeft>" +
                  "<MarginRight>{2}</MarginRight>" +
                  "<MarginBottom>{3}</MarginBottom>" +

                  "</DeviceInfo>"
                  ,
                  ToInches(margins.Top),
                  ToInches(margins.Left),
                  ToInches(margins.Right),
                  ToInches(margins.Bottom),
                  ToInches(height),
                  ToInches(width + 1)
                  );
            /*
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
             * */
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                customPrintbyPrinter(printerName, copy, width, height);
            }
        }

        public static void customExportbyPrinterwh(string printerName, LocalReport report, short copy, int width, int height, bool print = true)
        {

            PaperSize paperSize = m_pageSettings.PaperSize;

            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +

                  "<PageWidth>{5}</PageWidth>" +
                  "<PageHeight>{4}</PageHeight>" +

                  "<MarginTop>{0}</MarginTop>" +
                  "<MarginLeft>{1}</MarginLeft>" +
                  "<MarginRight>{2}</MarginRight>" +
                  "<MarginBottom>{3}</MarginBottom>" +

                  "</DeviceInfo>"
                  ,
                  ToInches(margins.Top),
                  ToInches(margins.Left),
                  ToInches(margins.Right),
                  ToInches(margins.Bottom),
                  ToInches(height),
                  ToInches(width)
                  );
            /*
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
             * */
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                customPrintbyPrinter(printerName, copy, width, height);
            }
        }
        public static void customPrintbyPrinter(string printerName, short copy, int width, int height)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            PaperSize oldpapersize = printDoc.DefaultPageSettings.PaperSize;
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", width, height);

            printDoc.PrinterSettings.Copies = copy;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName is null || printerName == "")
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }

            }
            printDoc.DefaultPageSettings.PaperSize = oldpapersize;
        }

        public static void customExportToPDF(LocalReport report, String FullPath, int width, int height)
        {/*
           /*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +
                      "<PageWidth>{1}</PageWidth>" +
                      "<PageHeight>{0}</PageHeight>" +
                       "<EmbedFonts>None</EmbedFonts>" +
                  "</DeviceInfo>",

                  ToInches(height + 100),
                  ToInches(width));
            byte[] Bytes = report.Render(format: "PDF", deviceInfo: deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try { 
            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                try
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                    stream.Close();

                }
                catch
                {

                }
                finally
                {
                    stream.Close();
                }
            }

        }
            catch { }

        }

   
        public static void customExportToPDFwh(LocalReport report, String FullPath, int width, int height)
        {/*
           /*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +
                      "<PageWidth>{1}</PageWidth>" +
                      "<PageHeight>{0}</PageHeight>" +
                       "<EmbedFonts>None</EmbedFonts>" +
                  "</DeviceInfo>",

                  ToInches(height),
                  ToInches(width));
       //   System.Drawing.Image img2 = System.Drawing.Image.FromFile(@"D:\mailtemp\images\image-122.png");
         //   img2.PhysicalDimension.Width; 
       

         byte[] Bytes = report.Render(format: "PDF", deviceInfo: deviceInfo);
           //byte[] Bytes = ImageToByteArray(img2);

       
           // BinaryFormatter bf = new BinaryFormatter();
        
           
          //  System.IO.File.WriteAllBytes(FullPath, Bytes);

            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }
                }

            }
            catch { }


        }


        public static void PrintToPrinterDirect(this LocalReport report, string printerName, short copy)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterNameAndCopy(printerName, report, copy);
        }

        public static void customPrintToPrinterDirect(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            customExportbyPrinter(printerName, report, copy, width, height);
        }

    }

    public static class LocalReportExtensionsPrint
    {

        private static int m_currentPageIndex;
        private static IList<Stream> m_streams;
        private static PageSettings m_pageSettings;

        public static Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        public static void ExportbyPrinterName(string printerName, LocalReport report, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                PrintbyPrinterName(printerName);
            }
        }

        public static void ExportbyPrinterNameAndCopy(string printerName, LocalReport report, short copy, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                PrintbyPrinterNameAndCopy(printerName, copy);
            }
        }
        public static void Export(LocalReport report, bool print = true)
        {
            PaperSize paperSize = m_pageSettings.PaperSize;
            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                CultureInfo.InvariantCulture,
                "<DeviceInfo>" +
                    "<OutputFormat>EMF</OutputFormat>" +
                /*
                "<PageWidth>{5}</PageWidth>" +
                "<PageHeight>{4}</PageHeight>" +

                "<MarginTop>{0}</MarginTop>" +
                "<MarginLeft>{1}</MarginLeft>" +
                "<MarginRight>{2}</MarginRight>" +
                "<MarginBottom>{3}</MarginBottom>" +
*/
                "</DeviceInfo>"
                /*,
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
                */);

            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print();
            }
        }
        public static void ExportToPDF(LocalReport report, String FullPath)
        {/*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
          CultureInfo.InvariantCulture,
          "<DeviceInfo>" +
              "<OutputFormat>PDF</OutputFormat>" +

   /*
   "<PageWidth>{5}</PageWidth>" +
   "<PageHeight>{4}</PageHeight>" +

   "<MarginTop>{0}</MarginTop>" +
   "<MarginLeft>{1}</MarginLeft>" +
   "<MarginRight>{2}</MarginRight>" +
   "<MarginBottom>{3}</MarginBottom>" +

*/
   "<EmbedFonts>None</EmbedFonts>" +
          "</DeviceInfo>");
            /*
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            */
            //  byte[] renderedBytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            byte[] Bytes = report.Render(format: "PDF", deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            catch { }

        }
        public static void ExportToWORD(LocalReport report, String FullPath)
        {

            string deviceInfo = "<DeviceInfo>" +
                    "  <OutputFormat>WORD</OutputFormat>" +

                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";


            //   byte[] renderedBytes = report.Render("WORD", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // byte[] Bytes = report.Render("Excel", deviceInfo);
            byte[] Bytes = report.Render(format: "WORD", deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);

            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }

        }

        public static void ExportToExcel(LocalReport report, String FullPath)
        {

            string deviceInfo = "<DeviceInfo>" +
                    "  <OutputFormat>Excel</OutputFormat>" +

                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";


            byte[] Bytes = report.Render("Excel", deviceInfo);
            // byte[] Bytes = report.Render("Excel", "", out contentType, out encoding, out extension, out streamIds, out warnings);

            //  byte[] Bytes = report.Render(format: "EXCEL", deviceInfo: deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }

                }
            }
            catch { }
        }
        // Handler for PrintPageEvents
        public static  void PrintPage(object sender, PrintPageEventArgs e)
        {
            Stream pageToPrint = m_streams[m_currentPageIndex];
            pageToPrint.Position = 0;

            // Load each page into a Metafile to draw it.
            using (Metafile pageMetaFile = new Metafile(pageToPrint))
            {
                Rectangle adjustedRect = new Rectangle(
                        e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                        e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                        e.PageBounds.Width,
                        e.PageBounds.Height);

                // Draw a white background for the report
                e.Graphics.FillRectangle(Brushes.White, adjustedRect);

                // Draw the report content
                e.Graphics.DrawImage(pageMetaFile, adjustedRect);

                // Prepare for next page.  Make sure we haven't hit the end.
                m_currentPageIndex++;
                e.HasMorePages = m_currentPageIndex < m_streams.Count;
            }
        }

        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {

                printDoc.PrintPage += new PrintPageEventHandler( PrintPage);
                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }
            }
        }
        public static void PrintbyPrinterName(string printerName)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName == "" || printerName is null)
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }


                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }
            }
        }


        public static void PrintbyPrinterNameAndCopy(string printerName, short copy)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.Copies = copy;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName is null || printerName == "")
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try {
                    printDoc.Print();
                } catch
                {

                }
               
            }
        }

        public static void PrintToPrinter(this LocalReport report)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            Export(report);
        }

        public static void PrintToPrinterbyName(this LocalReport report, string printerName)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterName(printerName, report);
        }
        public static void PrintToPrinterbyNameAndCopy(this LocalReport report, string printerName, short copy)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterNameAndCopy(printerName, report, copy);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        private static string ToInches(int hundrethsOfInch)
        {
            double inches = hundrethsOfInch / 100.0;
            return inches.ToString(CultureInfo.InvariantCulture) + "in";
        }

        // custom Print
        public static void customPrintToPrinterwh(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            customExportbyPrinterwh(printerName, report, copy, width, height);
        }
        public static void customPrintToPrinter(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

         customExportbyPrinter(printerName, report, copy, width, height);
        }

        public static void customExportbyPrinter(string printerName, LocalReport report, short copy, int width, int height, bool print = true)
        {

            PaperSize paperSize = m_pageSettings.PaperSize;

            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +

                  "<PageWidth>{5}</PageWidth>" +
                  "<PageHeight>{4}</PageHeight>" +

                  "<MarginTop>{0}</MarginTop>" +
                  "<MarginLeft>{1}</MarginLeft>" +
                  "<MarginRight>{2}</MarginRight>" +
                  "<MarginBottom>{3}</MarginBottom>" +

                  "</DeviceInfo>"
                  ,
                  ToInches(margins.Top),
                  ToInches(margins.Left),
                  ToInches(margins.Right),
                  ToInches(margins.Bottom),
                  ToInches(height),
                  ToInches(width + 1)
                  );
            /*
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
             * */
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                customPrintbyPrinter(printerName, copy, width, height);
            }
        }

        public static void customExportbyPrinterwh(string printerName, LocalReport report, short copy, int width, int height, bool print = true)
        {

            PaperSize paperSize = m_pageSettings.PaperSize;

            Margins margins = m_pageSettings.Margins;

            // The device info string defines the page range to print as well as the size of the page.
            // A start and end page of 0 means generate all pages.
            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +

                  "<PageWidth>{5}</PageWidth>" +
                  "<PageHeight>{4}</PageHeight>" +

                  "<MarginTop>{0}</MarginTop>" +
                  "<MarginLeft>{1}</MarginLeft>" +
                  "<MarginRight>{2}</MarginRight>" +
                  "<MarginBottom>{3}</MarginBottom>" +

                  "</DeviceInfo>"
                  ,
                  ToInches(margins.Top),
                  ToInches(margins.Left),
                  ToInches(margins.Right),
                  ToInches(margins.Bottom),
                  ToInches(height),
                  ToInches(width)
                  );
            /*
                ToInches(margins.Top),
                ToInches(margins.Left),
                ToInches(margins.Right),
                ToInches(margins.Bottom),
                ToInches(paperSize.Height),
                ToInches(paperSize.Width)
             * */
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                customPrintbyPrinter(printerName, copy, width, height);
            }
        }
        public static void customPrintbyPrinter(string printerName, short copy, int width, int height)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            PaperSize oldpapersize = printDoc.DefaultPageSettings.PaperSize;
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", width, height);

            printDoc.PrinterSettings.Copies = copy;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                if (printerName is null || printerName == "")
                {

                }
                else
                {
                    printDoc.PrinterSettings.PrinterName = printerName;
                }

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

                m_currentPageIndex = 0;
                try
                {
                    printDoc.Print();
                }
                catch
                {

                }

            }
            printDoc.DefaultPageSettings.PaperSize = oldpapersize;
        }

        public static void customExportToPDF(LocalReport report, String FullPath, int width, int height)
        {/*
           /*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +
                      "<PageWidth>{1}</PageWidth>" +
                      "<PageHeight>{0}</PageHeight>" +
                       "<EmbedFonts>None</EmbedFonts>" +
                  "</DeviceInfo>",

                  ToInches(height + 100),
                  ToInches(width));
            byte[] Bytes = report.Render(format: "PDF", deviceInfo: deviceInfo);
            // File.SetAttributes(savePath, FileAttributes.Normal);
            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }
                }

            }
            catch { }

        }


        public static void customExportToPDFwh(LocalReport report, String FullPath, int width, int height)
        {/*
           /*
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            var bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string fullpath = Path.Combine(DirPath, Filename);
            using (FileStream stream = new FileStream(fullpath.ToString(), FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }
           
            */


            string deviceInfo = string.Format(
                  CultureInfo.InvariantCulture,
                  "<DeviceInfo>" +
                      "<OutputFormat>EMF</OutputFormat>" +
                      "<PageWidth>{1}</PageWidth>" +
                      "<PageHeight>{0}</PageHeight>" +
                       "<EmbedFonts>None</EmbedFonts>" +
                  "</DeviceInfo>",

                  ToInches(height),
                  ToInches(width));
            //   System.Drawing.Image img2 = System.Drawing.Image.FromFile(@"D:\mailtemp\images\image-122.png");
            //   img2.PhysicalDimension.Width; 


            byte[] Bytes = report.Render(format: "PDF", deviceInfo: deviceInfo);
            //byte[] Bytes = ImageToByteArray(img2);


            // BinaryFormatter bf = new BinaryFormatter();


            //  System.IO.File.WriteAllBytes(FullPath, Bytes);

            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }
                }

            }
            catch { }


        }


        public static void PrintToPrinterDirect(this LocalReport report, string printerName, short copy)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            ExportbyPrinterNameAndCopy(printerName, report, copy);
        }

        public static void customPrintToPrinterDirect(this LocalReport report, string printerName, short copy, int width, int height)
        {
            m_pageSettings = new PageSettings();
            ReportPageSettings reportPageSettings = report.GetDefaultPageSettings();

            m_pageSettings.PaperSize = reportPageSettings.PaperSize;
            m_pageSettings.Margins = reportPageSettings.Margins;

            customExportbyPrinter(printerName, report, copy, width, height);
        }

    }
}