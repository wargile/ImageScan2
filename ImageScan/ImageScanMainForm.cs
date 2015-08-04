using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ImageScan
{
    public partial class ImageScanMainForm : Form
    {
        #region Private Variables

        private ScannerLogic scannerLogic;

        private delegate void UpdateMainFormAfterBarCodeEventDelegate(string parcelNumber);
        private delegate void UpdateMainFormAfterVideoEventDelegate(Image image);
        private delegate void UpdateMainFormAfterImageEventDelegate(Image image);

        private const string cancelPicture = "Bildet er i orden";

        FileLogging logFile;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ImageScanMainForm()
        {
            InitializeComponent();
            InitializeWhenApplicationStart();
        }

        #endregion

        #region Local Eventhandling

        #region btnScanBarCode_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                scannerLogic.Pull_Trigger();
            }
            catch (Exception ex)
            {
                logFile.LogException("ImageScanMainForm.btnScan_Click", ex);
                MessageBox.Show(ex.Message, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region btnScanPicture_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScanPicture_Click(object sender, EventArgs e)
        {
            try
            {
                scannerLogic.Pull_Trigger();
                //scannerLogic.Release_Trigger();
            }
            catch (Exception ex)
            {
                logFile.LogException("ImageScanMainForm.btnScanLabel_Click", ex);
                MessageBox.Show(ex.Message, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region btnSavePicture_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePicture_Click(object sender, EventArgs e)
        {
            try
            {
                DocumentCreator documentCreator = new DocumentCreator(txtParcelNumber.Text, picImageVideo.Image);
                documentCreator.CreateDocuments();
            }
            catch (Exception ex)
            {
                logFile.LogException("ImageScanMainForm.btnImageIsOK_Click", ex);
                MessageBox.Show(ex.Message, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.InitializeAfterRejectedBarCode();
        }

        #endregion

        #region timerDateTime_Tick

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString();
        }

        #endregion

        #region chkVideoViewFinderEnable_CheckedChanged

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkVideoViewFinderEnable_CheckedChanged(object sender, EventArgs e)
        {
            scannerLogic.EnableVideoMode(chkVideoViewFinderEnable.Checked);
        }

        #endregion

        #region btnCancel_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeAfterUserCancelling();
            }
            catch (Exception ex)
            {
                logFile.LogException("ImageScanMainForm.btnCancel_Click", ex);
                MessageBox.Show(ex.Message, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ImageScanMainForm_FormClosing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageScanMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logFile.LogEnd("ImageScan");
        }

        #endregion

        #region btnExit_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            scannerLogic.Close();
            logFile.LogEnd("ImageScan");
            this.Close();
        }

        #endregion

        #region lnkInfo_LinkClicked

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SplashForm();
        }

        #endregion

        #endregion

        #region Functions to handle events from ScannerLogic

        #region ScannerLogic_ReceiveBarCodeInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parcelNumber"></param>
        /// <param name="symbiologi"></param>
        private void ScannerLogic_ReceiveBarCodeInfo(string parcelNumber, string symbiologi)
        {
            string message = string.Format("ImageScanMainForm.ScannerLogic_ReceiveBarCodeInfo\r\nParcelnumber: {0}. Symbiologi: {1}", parcelNumber, symbiologi);
            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);

            bool isValid = Utilities.IsParcelNumberPostStandard(parcelNumber) |
                Utilities.IsParcelNumberPostUPU(parcelNumber) | Utilities.IsParcelNumberGS1(parcelNumber) |
                Utilities.IsParcelNumberPostFinlandStandard(parcelNumber);

            if (!isValid)
                MessageBox.Show(parcelNumber + " er ikke et gyldig sendingsnummer.");
            else
            {
                UpdateMainFormWithBarCode(parcelNumber);
            }
        }

        #endregion

        #region ScannerLogic_ReceiveImageInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void ScannerLogic_ReceiveImageInfo(Image image)
        {
            string message = string.Format("ImageScanMainForm.ScannerLogic_ReceiveImageInfo\r\nBildehøyde: {0}. Bildebredde: {1}", image.Size.Height.ToString(), image.Size.Width.ToString());
            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);

            UpdateMainFormWithImage(image);
        }

        #endregion

        #region ScannerLogic_ReceiveVideoInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void ScannerLogic_ReceiveVideoInfo(Image image)
        {
            UpdateMainFormWithVideo(image);
        }

        #endregion

        #region ScannerLogic_ReceiveErrorInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        private void ScannerLogic_ReceiveErrorInfo(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region ScannerLogic_ReceivePNPInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDeviceAttached"></param>
        void ScannerLogic_ReceivePNPInfo(bool isDeviceAttached)
        {
            if (isDeviceAttached)
            {
                InitializeAfterDeattatchedDevice();
                MessageBox.Show("Scanner er nå tilkoblet igjen.", "Informasjon", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Scanner er blitt frakoblet.\r\nApplikasjonen kan ikke brukes før den blir tilkoblet igjen.", "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion

        #region Functions to handle events from DocumentCreator

        #region DocumentCreator_SendErrorInfo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        void DocumentCreator_SendErrorInfo(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #endregion

        #region Functions to update MainForm when received events from Scannerlogic

        #region UpdateMainFormWithBarCode

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pacelNumber"></param>
        private void UpdateMainFormWithBarCode(string pacelNumber)
        {
            if (txtParcelNumber.InvokeRequired) 
            {     
                // this is worker thread     
                UpdateMainFormAfterBarCodeEventDelegate updateMainFormAfterBarCodeEventDelegate = new UpdateMainFormAfterBarCodeEventDelegate(UpdateMainFormWithBarCode);
                txtParcelNumber.Invoke(updateMainFormAfterBarCodeEventDelegate, new object[] { pacelNumber }); 
            } 
            else 
            {
                // this is UI thread     
                InitializeAfterReceivedBarCode(pacelNumber);
            }
        }

        #endregion

        #region UpdateMainFormWithImage

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void UpdateMainFormWithImage(Image image)
        {
            if (picImageVideo.InvokeRequired)
            {
                // this is worker thread     
                UpdateMainFormAfterImageEventDelegate updateMainFormAfterImageEventDelegate = new UpdateMainFormAfterImageEventDelegate(UpdateMainFormWithImage);
                picImageVideo.Invoke(updateMainFormAfterImageEventDelegate, new object[] { image });
            }
            else
            {
                // this is UI thread     
                InitializeAfterReceivedImage(image);
            }
        }

        #endregion

        #region UpdateMainFormWithVideo

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void UpdateMainFormWithVideo(Image image)
        {
            if (picImageVideo.InvokeRequired)
            {
                // this is worker thread     
                UpdateMainFormAfterVideoEventDelegate updateMainFormAfterVideoEventDelegate = new UpdateMainFormAfterVideoEventDelegate(UpdateMainFormWithVideo);
                picImageVideo.Invoke(updateMainFormAfterVideoEventDelegate, new object[] { image });
            }
            else
            {
                // this is UI thread     
                picImageVideo.Image = image;
            }
        }

        #endregion

        #endregion

        #region Functions to Initialize and Reset

        #region InitializeWhenApplicationStart

        /// <summary>
        /// 
        /// </summary>
        private void InitializeWhenApplicationStart()
        {
            try
            {
                lblInfoTextHeading.Text = "Les av strekkode";
                lblInfoText1.Text = "1. Les av strekkoden med IS-skanneren\n    med én av følgende metoder:\n       a) Trykk på avtrekkeren\n       b) Trykk [Enter]\n       c) Trykk [Alt-S] eller [S]\n       d) Klikk på knappen under";
                lblInfoText2.Text = "2. Ta bilde med IS-skanneren med\n    én av følgende metoder:\n       a) Trykk på avtrekkeren\n       b) Trykk [Enter]\n       c) Trykk [Alt-B] eller [B]\n       d) Klikk på knappen under";
                lblInfoText3.Text = "3. Hvis bildet er i orden, trykk [Enter],\n    eller benytt knappen under";

                StripStatusMessage.Text = "Les av strekkode";

                txtParcelNumber.Enabled = false;
                btnScanPicture.Visible = false;
                lblInfoText2.Visible = false;
                lblInfoText3.Visible = false;
                btnSavePicture.Visible = false;

                panel1.BackColor = Color.FromArgb(210, 230, 230);
                toolStripContainer1.ContentPanel.BackColor = Color.FromArgb(230, 240, 250);
                lblDateTime.Text = DateTime.Now.ToString();
                btnCancel.Enabled = false;

                InitializeLogFile();

                this.Text = GetVersionNumber();

                scannerLogic = new ScannerLogic();
                scannerLogic.SendBarCodeInfo += new ScannerLogic.BarCodeHandler(ScannerLogic_ReceiveBarCodeInfo);
                scannerLogic.SendVideoInfo += new ScannerLogic.VideoHandler(ScannerLogic_ReceiveVideoInfo);
                scannerLogic.SendImageInfo += new ScannerLogic.ImageHandler(ScannerLogic_ReceiveImageInfo);
                scannerLogic.SendErrorInfo += new ScannerLogic.ErrorHandler(ScannerLogic_ReceiveErrorInfo);
                scannerLogic.SendPNPInfo += new ScannerLogic.PNPHandler(ScannerLogic_ReceivePNPInfo);

                scannerLogic.Open();
                scannerLogic.SetBarCodeMode();

                btnScanBarCode.Focus();
            }
            catch (COMException cex)
            {
                ExceptionHandler("ImageScanMainForm.InitializeWhenApplicationStart.COMException", cex);

                string cexMessage = "Det har oppstått en alvorlig feil.\r\nÅrsaken kan være at programvare for scanner (scannerdriver) ikke er installert.\r\nFeilen er logget til applikasjonslogg eller eventlogg.\r\n\r\nApplikasjonen lukkes nå automatisk";
                MessageBox.Show(cexMessage, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);

                logFile.LogEnd("ImageScan");
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler("ImageScanMainForm.InitializeWhenApplicationStart.Exception", ex);

                string message = "Det har oppstått en uventet feilmelding.\r\nFeilen er logget til applikasjonslogg eller eventlogg.\r\nTa kontakt med systemansvarlig dersom feil vedvarer.\r\n\r\nApplikasjonen lukkes nå automatisk";
                MessageBox.Show(message, "Feilmelding", MessageBoxButtons.OK, MessageBoxIcon.Error);

                logFile.LogEnd("ImageScan");
                this.Close();
            }
        }

        #endregion

        #region InitializeAfterReceivedBarCode

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pacelNumber"></param>
        private void InitializeAfterReceivedBarCode(string pacelNumber)
        {
            txtParcelNumber.Text = pacelNumber;

            lblInfoTextHeading.Text = "Ta bilde av tollinformasjon";
            StripStatusMessage.Text = "Ta bilde av tollinformasjon";

            lblInfoText1.Enabled = false;
            btnScanBarCode.Enabled = false;
            lblInfoText2.Visible = true;
            btnScanPicture.Visible = true;
            btnCancel.Enabled = true;
            this.AcceptButton = this.btnScanPicture;

            scannerLogic.SetImageMode();
            btnScanPicture.Focus();
        }

        #endregion

        #region InitializeAfterReceivedImage

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        private void InitializeAfterReceivedImage(Image image)
        {
            if (image == null)
            {
                picImageVideo.Image = null;
            }
            else
            {
                picImageVideo.Image = image;
            }

            btnScanPicture.Enabled = false;
            lblInfoText2.Enabled = false;
            lblInfoText3.Visible = true;
            btnSavePicture.Visible = true;
            lblInfoTextHeading.Text = "Bekreft bilde";
            StripStatusMessage.Text = "Bekreft bilde";

            this.AcceptButton = btnSavePicture;
            btnSavePicture.Focus();
        }


        #endregion

        #region InitializeAfterCancelledImage

        /// <summary>
        /// 
        /// </summary>
        private void InitializeAfterCancelledImage()
        {
            lblInfoText1.Enabled = true;
            lblInfoText1.Visible = true;

            lblInfoText2.Enabled = true;
            lblInfoText2.Visible = true;

            lblInfoText3.Enabled = false;
            lblInfoText3.Visible = false;

            btnScanBarCode.Enabled = false;
            btnScanBarCode.Visible = true;

            btnScanPicture.Enabled = true;
            btnScanPicture.Visible = true;

            btnSavePicture.Enabled = true;
            btnSavePicture.Visible = false;

            picImageVideo.Image = null;

            this.AcceptButton = btnScanPicture;

            lblInfoTextHeading.Text = "Les av tollinformasjon";
            StripStatusMessage.Text = "Les av tollinformasjon";

            scannerLogic.Image_Abort();
            scannerLogic.SetImageMode();
            btnScanPicture.Focus();
        }

        #endregion

        #region InitializeAfterRejectedBarCode

        /// <summary>
        /// 
        /// </summary>
        private void InitializeAfterRejectedBarCode()
        {
            lblInfoText1.Enabled = true;
            lblInfoText1.Visible = true;

            lblInfoText2.Enabled = true;
            lblInfoText2.Visible = false;

            lblInfoText3.Enabled = true;
            lblInfoText3.Visible = false;

            btnScanBarCode.Enabled = true;
            btnScanBarCode.Visible = true;

            btnSavePicture.Enabled = true;
            btnSavePicture.Visible = false;

            btnScanPicture.Enabled = true;
            btnScanPicture.Visible = false;

            btnCancel.Enabled = false;

            txtParcelNumber.Text = string.Empty;
            picImageVideo.Image = null;

            this.AcceptButton = btnScanBarCode;

            lblInfoTextHeading.Text = "Les av strekkode";
            StripStatusMessage.Text = "Les av strekkode";

            scannerLogic.SetBarCodeMode();
            btnScanBarCode.Focus();
        }

        #endregion

        #region InitializeAfterUserCancelling

        /// <summary>
        /// Reset scan process, start over
        /// </summary>
        private void InitializeAfterUserCancelling()
        {
            Button button = (Button)this.AcceptButton;

            if (button.Text == cancelPicture)
            {
                InitializeAfterCancelledImage();
            }
            else
            {
                if (btnCancel.Enabled)
                {
                    InitializeAfterRejectedBarCode();
                }
            }
        }


        #endregion

        #region InitializeAfterDeattatchedDevice

        /// <summary>
        /// 
        /// </summary>
        private void InitializeAfterDeattatchedDevice()
        {
            txtParcelNumber.Text = string.Empty;
            picImageVideo.Image = null;

            scannerLogic = new ScannerLogic();

            scannerLogic.SendBarCodeInfo += new ScannerLogic.BarCodeHandler(ScannerLogic_ReceiveBarCodeInfo);
            scannerLogic.SendVideoInfo += new ScannerLogic.VideoHandler(ScannerLogic_ReceiveVideoInfo);
            scannerLogic.SendImageInfo += new ScannerLogic.ImageHandler(ScannerLogic_ReceiveImageInfo);
            scannerLogic.SendErrorInfo += new ScannerLogic.ErrorHandler(ScannerLogic_ReceiveErrorInfo);
            scannerLogic.SendPNPInfo += new ScannerLogic.PNPHandler(ScannerLogic_ReceivePNPInfo);

            scannerLogic.Open();
            scannerLogic.SetBarCodeMode();
        }

        #endregion

        #endregion

        #region SplahsScreen Functions

        #region SplashForm

        /// <summary>
        /// 
        /// </summary>
        private void SplashForm()
        {
            ImageScanSplash newSplashForm = new ImageScanSplash();
            newSplashForm.ShowDialog();
            newSplashForm.Dispose();
        }

        #endregion

        #region ShowSplashScreen

        /// <summary>
        /// 
        /// </summary>
        private void ShowSplashScreen()
        {
            Thread splashThread = new Thread(new ThreadStart(SplashForm));
            splashThread.Start();
        }

        #endregion

        #endregion

        #region Private Functions

        #region ProcessCmdKey

        /// <summary>
        /// Override ProcessCmdKey to capture ESC key to reset scan process
        /// </summary>
        /// <param name="msg">The command message</param>
        /// <param name="keyData">The key data</param>
        /// <returns>True</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                InitializeAfterUserCancelling();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region GetVersionNumber

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetVersionNumber()
        {
            string versionNumber = string.Empty;
            versionNumber = string.Format("ImageScan - Imageskanning for Posten Norge. Versjon {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            return versionNumber;
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

            logFile.DoLogging = (bool)reader.GetValue("FileLogging.ImageScanMainForm.DoLogging", typeof(bool));
            logFile.DoDeleteLogFiles = (bool)reader.GetValue("FileLogging.ImageScanMainForm.DoDeleteLogFiles", typeof(bool));

            logFile.LogFilePath = (string)reader.GetValue("FileLogging.ImageScanMainForm.LogFilePath", typeof(string));
            logFile.LogFileName = (string)reader.GetValue("FileLogging.ImageScanMainForm.LogFileName", typeof(string));
            logFile.LogFileExtension = (string)reader.GetValue("FileLogging.ImageScanMainForm.LogFileExtension", typeof(string));

            logFile.LogFileArchivePath = (string)reader.GetValue("FileLogging.ImageScanMainForm.LogFileArchivePath", typeof(string));
            logFile.LogFileSearchPattern = (string)reader.GetValue("FileLogging.ImageScanMainForm.SearchPattern", typeof(string));

            logFile.DaysToKeepLogFiles = (int)reader.GetValue("FileLogging.ImageScanMainForm.DaysToKeepLogFiles", typeof(int));
            logFile.DaysToArchiveLogFiles = (int)reader.GetValue("FileLogging.ImageScanMainForm.DaysToArchiveLogFiles", typeof(int));

            logFile.LogStart("ImageScan");
        }

        #endregion

        #region ExceptionHandler

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="ex"></param>
        private void ExceptionHandler(string functionName, Exception ex)
        {
            if (logFile != null)
            {
                logFile.LogException(functionName, ex);
            }
            else
            {
                //DO nothing
                //Utilities.WriteToEventlog(ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
