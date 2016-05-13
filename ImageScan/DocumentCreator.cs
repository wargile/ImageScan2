using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using iTextSharp.text;

namespace ImageScan
{
    // iTextSharp for PDF creation:
    // http://sourceforge.net/projects/itextsharp/
    // Good iTextSharp tutorial:
    // http://www.mikesdotnetting.com/Article/80/Create-PDFs-in-ASP.NET-getting-started-with-iTextSharp

    class DocumentCreator
    {
        #region Private Method Variables

        private string parcelNumber = string.Empty; // Scanned parcel number, set before creating/storing files
        private long sequenceNumber = int.MinValue;    // Get in constructor
        private System.Drawing.Image capturedImage = null;

        private string exportCatalog = string.Empty;
        private string temporaryImageStore = string.Empty;
        private string pdfDocumentName = string.Empty;
        // TBAK 2015-02-19: Added documentBaseName variable.
        // Stores the base name at first call to avoid timestamp second roll-over giving wrong basename on subsequent calls
        private string documentBaseName = string.Empty;
        private string infoFileName = string.Empty;

        private Document document;
        private float _PDF_Margin = 45f;
        private string _PDF_Format = "vertical";
        private string _Image_Format = "vertical";
        private bool _MaximizeImageSize = true;
        private float _ReduceImageSizeToPercent = 50f;

        FileLogging logFile;

        private enum Direction
        {
            horizontal,
            vertical
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string ParcelNumber
        {
            get { return parcelNumber; }
            
            set 
            { 
                if (value == string.Empty)
                {
                    throw new ApplicationException("Pakkenummer kan ikke være tomt");
                }
                else
                {
                    parcelNumber = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Drawing.Image CapturedImage
        {
            get { return capturedImage; }
            set 
            { 
                if(value == null)
                {
                    throw new ApplicationException("Bilde kan ikke være tomt");
                }
                else
                {
                    capturedImage = value; 
                }
            }
        }

        #endregion

        #region Public Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelNumber"></param>
        /// <param name="image"></param>
        public DocumentCreator(string parcelNumber, System.Drawing.Image image)
        {
            try
            {
                ParcelNumber = parcelNumber;
                CapturedImage = image;
                GetConfigData();
                InitializeLogFile();

                string message = string.Format("DocumentCreator.Constructor. Parcelnumber: {0}", parcelNumber);
                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
            catch (Exception ex)
            {
                //logFile.WriteToEventLog(ex);
                logFile.LogMessage(ex.Message, FileLogging.MessageType.ExceptionMessage);
                throw;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Create PDF document with scanned image and accompanying index file
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateDocuments()
        {
            try
            {
                if (_PDF_Format == Direction.horizontal.ToString())
                {
                    document = new Document(new iTextSharp.text.Rectangle(842f, 595f), _PDF_Margin, _PDF_Margin, _PDF_Margin, _PDF_Margin);
                }
                else
                {
                    document = new Document(PageSize.A4, _PDF_Margin, _PDF_Margin, _PDF_Margin, _PDF_Margin);
                }

                if (_Image_Format == Direction.vertical.ToString())
                {
                    CapturedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }

                sequenceNumber = FileIO.GetSeqNumber();
                pdfDocumentName = GetPDFDocumentName();
                infoFileName = GetInfoFileName();

                string imagePathAndName = string.Format("{0}{1}.jpg", temporaryImageStore, Guid.NewGuid().ToString());
                CapturedImage.Save(imagePathAndName);

                string pdfDocumentPathAndName = string.Format("{0}{1}", exportCatalog, pdfDocumentName);

                using (var stream = new FileStream(pdfDocumentPathAndName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);
                    document.Open();

                    using (var imageStream = new FileStream(imagePathAndName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var image = iTextSharp.text.Image.GetInstance(imageStream);

                        if (_MaximizeImageSize)
                        {
                            float width = document.PageSize.Width - (_PDF_Margin + _PDF_Margin);
                            float pageHeight = document.PageSize.Height - (_PDF_Margin + _PDF_Margin);

                            image.ScaleToFit(width, pageHeight);
                        }
                        else
                        {
                            image.ScalePercent(_ReduceImageSizeToPercent);
                        }

                        document.Add(image);
                    }

                    document.Close();
                    document.Dispose();

                    File.Delete(imagePathAndName);
                }

                // Create .INF file
                string infoFilePathAndName = string.Format("{0}{1}", exportCatalog, infoFileName);

                using (StreamWriter outFile = new StreamWriter(infoFilePathAndName))
                {
                    outFile.Write(this.GetInfoFileContent());
                }

                string message = string.Format("DocumentCreator.CreateDocuments\r\nPDF-dokument: {0}\r\nBilde: {1}\r\nInfofil: {2}",
                    pdfDocumentPathAndName, imagePathAndName, infoFilePathAndName);

                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
            catch (Exception ex)
            {
                //logFile.WriteToEventLog(ex);
                logFile.LogMessage(ex.Message, FileLogging.MessageType.ExceptionMessage);
                throw;
            }
        }

        #endregion

        #region GetDocumentBaseName

        /// <summary>
        /// Return the base document name: pc-name + 10-digit leading zeroes index number
        /// </summary>
        /// <returns>The formatted base document name</returns>
        private string GetDocumentBaseName()
        {
            DateTime now = DateTime.Now;
            string prefix = string.Format("{0}{1}{2}{3}{4}{5}", now.ToString("yy"), now.ToString("MM"), now.ToString("dd"), now.ToString("hh"),now.ToString("mm"), now.ToString("ss") );

            string machineName = System.Environment.MachineName.PadRight(10, '_');

            return string.Format("{0}{1}{2}", prefix, machineName, sequenceNumber.ToString("D3"));
        }

        #endregion

        #region GetPDFDocumentName

        /// <summary>
        /// Return the name of the PDF document: pc-name + 10-digit leading zeroes index number + .pdf
        /// </summary>
        /// <returns>The formatted PDF document name</returns>
        private string GetPDFDocumentName()
        {
            // TBAK 2015-02-19: Added check on documentBaseName variable.
            if (this.documentBaseName == string.Empty)
                this.documentBaseName = GetDocumentBaseName();
            return this.documentBaseName + ".pdf";
        }

        #endregion

        #region GetExportCatalog

        /// <summary>
        /// Get the storage location for produced PDF and INF-files
        /// </summary>
        /// <returns>The ex</returns>
        private string GetExportCatalog()
        {
            if (!exportCatalog.Substring(exportCatalog.Length - 1, 1).Equals(@"\"))
            {
                exportCatalog = exportCatalog + @"\"; // Append backslash if not present
            }

            if (!Directory.Exists(exportCatalog))
            {
                Directory.CreateDirectory(exportCatalog);
            }

            return exportCatalog;
        }

        #endregion

        #region GetTemporaryImageCatalog

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetTemporaryImageCatalog()
        {
            if (!temporaryImageStore.Substring(temporaryImageStore.Length - 1, 1).Equals(@"\"))
            {
                temporaryImageStore = temporaryImageStore + @"\"; // Append backslash if not present
            }

            if (!Directory.Exists(temporaryImageStore))
            {
                Directory.CreateDirectory(temporaryImageStore);
            }

            return temporaryImageStore;
        }

        #endregion

        #region GetInfoFileName

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The name of the INF file</returns>
        private string GetInfoFileName()
        {
            // TBAK 2015-02-19: Added check on documentBaseName variable.
            if (this.documentBaseName == string.Empty)
                this.documentBaseName = GetDocumentBaseName();
            return this.documentBaseName + ".inf";
        }

        #endregion

        #region GetInfoFileContent

        /// <summary>
        /// Get the INF file content
        /// </summary>
        /// <returns>The INF file content</returns>
        private string GetInfoFileContent()
        {
            string content = string.Format("{0};{1}{2};{3}:{4}:{5};{6}-{7}-{8}",
                parcelNumber, exportCatalog, pdfDocumentName, 
                DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2"), DateTime.Now.Second.ToString("D2"),
                DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("D2"), DateTime.Now.Day.ToString("D2"));

            return content;
        }

        #endregion

        #region RotateImage

        /// <summary>
        /// http://www.dreamincode.net/code/snippet2640.htm
        /// http://www.switchonthecode.com/tutorials/csharp-tutorial-image-editing-rotate
        /// http://www.codeproject.com/Articles/4861/An-Image-Viewer-with-Lossless-Rotation-EXIF-and-Ot
        /// http://www.toniwestbrook.com/archives/60
        /// http://www.farooqazam.net/image-viewer-application-using-c-sharp/
        /// Method to rotate an image either clockwise or counter-clockwise. Used for PDF document creation.
        /// </summary>
        /// <param name="image">The image to be rotated</param>
        /// <param name="rotationAngle">The angle (in degrees).
        /// NOTE: Positive values will rotate clockwise, negative values will rotate counter-clockwise
        /// </param>
        /// <returns>The rotated image</returns>
        private System.Drawing.Image RotateImage(System.Drawing.Image image, float rotationAngle)
        {
            // Create an empty Bitmap image
            Bitmap bitmapImage = new Bitmap(image.Width, image.Height);

            // Turn the Bitmap into a Graphics object
            Graphics graphics = Graphics.FromImage(bitmapImage);

            // Set the rotation point to the center of our image
            graphics.TranslateTransform((float)bitmapImage.Width / 2, (float)bitmapImage.Height / 2);

            // Rotate the image
            graphics.RotateTransform(rotationAngle);

            graphics.TranslateTransform(-(float)bitmapImage.Width / 2, -(float)bitmapImage.Height / 2);

            // Set the InterpolationMode to HighQualityBicubic to ensure a high
            // quality image once it is transformed to the specified size
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new image onto the graphics object
            graphics.DrawImage(image, new Point(0, 0));

            // Dispose of the Graphics object
            graphics.Dispose();

            // Return the image
            return bitmapImage;
        }

        #endregion

        #region GetConfigData

        /// <summary>
        /// 
        /// </summary>
        private void GetConfigData()
        {
            AppSettingsReader appdata = new AppSettingsReader();
            temporaryImageStore = (string)appdata.GetValue("TemporaryImageStore", typeof(string));
            exportCatalog = (string)appdata.GetValue("ExportCatalog", typeof(string));

            temporaryImageStore = GetTemporaryImageCatalog();
            exportCatalog = GetExportCatalog();

            _PDF_Margin = (float)appdata.GetValue("PDF_Margin", typeof(float));

            _PDF_Format = (string)appdata.GetValue("PDF_Format", typeof(string));
            _PDF_Format = _PDF_Format.ToLower();

            _Image_Format = (string)appdata.GetValue("Image_Format", typeof(string));
            _Image_Format = _Image_Format.ToLower();

            _MaximizeImageSize = (bool)appdata.GetValue("MaximizeImageSize", typeof(bool));
            _ReduceImageSizeToPercent = (float)appdata.GetValue("ReduceImageSizeToPercent", typeof(float));
        }

        #endregion

        #region InitializeLogFile

        /// <summary>
        /// 
        /// </summary>
        private void InitializeLogFile()
        {
            AppSettingsReader reader = new AppSettingsReader();

            logFile = new FileLogging();

            logFile.DoLogging = (bool)reader.GetValue("FileLogging.DocumentCreator.DoLogging", typeof(bool));
            logFile.DoDeleteLogFiles = (bool)reader.GetValue("FileLogging.DocumentCreator.DoDeleteLogFiles", typeof(bool));

            logFile.LogFilePath = (string)reader.GetValue("FileLogging.DocumentCreator.LogFilePath", typeof(string));
            logFile.LogFileName = (string)reader.GetValue("FileLogging.DocumentCreator.LogFileName", typeof(string));
            logFile.LogFileExtension = (string)reader.GetValue("FileLogging.DocumentCreator.LogFileExtension", typeof(string));

            logFile.LogFileArchivePath = (string)reader.GetValue("FileLogging.DocumentCreator.LogFileArchivePath", typeof(string));
            logFile.LogFileSearchPattern = (string)reader.GetValue("FileLogging.DocumentCreator.SearchPattern", typeof(string));

            logFile.DaysToKeepLogFiles = (int)reader.GetValue("FileLogging.DocumentCreator.DaysToKeepLogFiles", typeof(int));
            logFile.DaysToArchiveLogFiles = (int)reader.GetValue("FileLogging.DocumentCreator.DaysToArchiveLogFiles", typeof(int));

            logFile.LogStart("ImageScan");
        }

        #endregion
    }
}
