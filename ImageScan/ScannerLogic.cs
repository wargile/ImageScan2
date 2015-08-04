using System;
using System.Configuration;
using System.Drawing;
using CoreScanner;
using System.Xml;


namespace ImageScan
{
    public partial class ScannerLogic
    {
        #region Private Variables

        private CCoreScannerClass _Scanner;
        private Scanner[] m_arScanners;
        private bool[] m_arSelectedTypes;
        private bool _IsScannerOpen;//Is open success
        private short _NumberOfTypesOfScanners;
        private short[] _ScannerTypes;
        private int _TotalNumberOfScanners;
        private XmlReader xmlReader;
        private int[] m_nArTotalScannersInType; //total scanners in types of SCANNER_TYPES_SNAPI,SCANNER_TYPES_SSI,SCANNER_TYPES_IBMHID,SCANNER_TYPES_NIXMODB,SCANNER_TYPES_HIDKB
        private  ushort[] m_arParamsList; //Parameter information list 
        private ushort[] m_arViewFindParamsList; 
        private Image imgCapturedImage;

        public event BarCodeHandler SendBarCodeInfo;
        public delegate void BarCodeHandler(string barCode, string symbiologi);

        public event VideoHandler SendVideoInfo;
        public delegate void VideoHandler(Image image);

        public event ImageHandler SendImageInfo;
        public delegate void ImageHandler(Image image);

        public event ErrorHandler SendErrorInfo;
        public delegate void ErrorHandler(string errorMessage);

        public event PNPHandler SendPNPInfo;
        public delegate void PNPHandler(bool isDeviceAttached);

        FileLogging logFile;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ScannerLogic()
        {
            InitializeLogFile();
            InitializeScanner();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeScanner()
        {
            _IsScannerOpen = false;
            _TotalNumberOfScanners = 0;
            m_nArTotalScannersInType = new int[5];
            InitScannersCount();

            m_arScanners = new Scanner[ScannerDefinitions.MAX_NUM_DEVICES];

            for (int i = 0; i < ScannerDefinitions.MAX_NUM_DEVICES; i++)
            {
                Scanner scanr = new Scanner();
                m_arScanners.SetValue(scanr, i);
            }

            m_arParamsList = new ushort[ScannerDefinitions.MAX_PARAM_LEN];
            m_arViewFindParamsList = new ushort[ScannerDefinitions.MAX_PARAM_LEN];
            xmlReader = new XmlReader();

            _NumberOfTypesOfScanners = 0;
            _ScannerTypes = new short[ScannerDefinitions.TOTAL_SCANNER_TYPES];
            m_arSelectedTypes = new bool[ScannerDefinitions.TOTAL_SCANNER_TYPES];

            _Scanner = new CoreScanner.CCoreScannerClass();

            /* Event registration for COM Service */
            _Scanner.BarcodeEvent += new CoreScanner._ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);
            _Scanner.ImageEvent += new CoreScanner._ICoreScannerEvents_ImageEventEventHandler(OnImageEvent);
            _Scanner.VideoEvent += new CoreScanner._ICoreScannerEvents_VideoEventEventHandler(OnVideoEvent);
            _Scanner.PNPEvent += new CoreScanner._ICoreScannerEvents_PNPEventEventHandler(OnPNPEvent);
            _Scanner.ScannerNotificationEvent += new _ICoreScannerEvents_ScannerNotificationEventEventHandler(OnScannerNotification);

            Keyboard_Emulator_Get_Config();
            Keyboard_Emulator_Set_Locale();

            //m_pCoreScanner.PNPEvent += new _ICoreScannerEvents_PNPEventEventHandler(OnPNPEvent);
            //m_pCoreScanner.ScanRMDEvent += new CoreScanner._ICoreScannerEvents_ScanRMDEventEventHandler(OnScanRMDEvent);
            //m_pCoreScanner.CommandResponseEvent += new CoreScanner._ICoreScannerEvents_CommandResponseEventEventHandler(OnCommandResponseEvent);
            //m_pCoreScanner.IOEvent += new CoreScanner._ICoreScannerEvents_IOEventEventHandler(OnIOEvent);
        }

        #endregion

        #region Public Methods

        #region Open

        /// <summary>
        /// 
        /// </summary>
        public void Open()
        {
            try
            {
                FilterScannerList();
                MakeConnectCtrl();
                RegisterForEvents();
                ShowScanners();
            }
            catch (Exception ex)
            {
                logFile.LogException("ScannerLogic.Open", ex);
                SendErrorInfo(ex.Message);
            }
        }

        #endregion

        #region Image_Abort

        /// <summary>
        /// 
        /// </summary>
        public void Image_Abort()
        {
            if (IsScannersConnected())
            {
                string inXml = GetScannerIDXml();
                int opCode = ScannerDefinitions.ABORT_IMAGE_XFER;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;
                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("ABORT_IMAGE_XFER\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                    opCode.ToString(), inXml, outXml, status.ToString());

                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region SetBarCodeMode

        /// <summary>
        /// 
        /// </summary>
        public void SetBarCodeMode()
        {
            try
            {
                if (IsScannersConnected())
                {
                    string inXml = GetScannerIDXml();

                    int opCode = ScannerDefinitions.DEVICE_CAPTURE_BARCODE;
                    string outXml = "";
                    int status = ScannerDefinitions.STATUS_FALSE;
                    ExecCmd(opCode, ref inXml, out outXml, out status);

                    string message = string.Format("SET_BARCODE_MODE\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                        opCode.ToString(), inXml, outXml, status.ToString());

                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
                }
            }
            catch (Exception ex)
            {
                logFile.LogException("ScannerLogic.ReadBarCode", ex);
                SendErrorInfo(ex.Message);
            }
        }

        #endregion

        #region SetImageMode

        /// <summary>
        /// 
        /// </summary>
        public void SetImageMode()
        {
            try
            {
                PerformOnJpg();

                if (IsScannersConnected())
                {
                    string inXml = GetScannerIDXml();

                    int opCode = ScannerDefinitions.DEVICE_CAPTURE_IMAGE;
                    string outXml = "";
                    int status = ScannerDefinitions.STATUS_FALSE;

                    ExecCmd(opCode, ref inXml, out outXml, out status);

                    string message = string.Format("SET_IMAGE_MODE\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                        opCode.ToString(), inXml, outXml, status.ToString());

                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);

                }
            }
            catch (Exception ex)
            {
                logFile.LogException("ScannerLogic.ReadImage", ex);
                SendErrorInfo(ex.Message);
            }
        }

        #endregion

        #region Pull_Trigger

        /// <summary>
        /// 
        /// </summary>
        public void Pull_Trigger()
        {
            if (IsScannersConnected())
            {
                string inXml = GetScannerIDXml();

                int opCode = ScannerDefinitions.DEVICE_PULL_TRIGGER;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;
                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("DEVICE_PULL_TRIGGER\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}", opCode.ToString(), inXml, outXml, status.ToString());
                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region Release_Trigger

        /// <summary>
        /// 
        /// </summary>
        public void Release_Trigger()
        {
            if (IsScannersConnected())
            {
                string inXml = GetScannerIDXml();

                int opCode = ScannerDefinitions.DEVICE_RELEASE_TRIGGER;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;
                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("DEVICE_RELEASE_TRIGGER\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}", opCode.ToString(), inXml, outXml, status.ToString());
                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region Reboot

        /// <summary>
        /// 
        /// </summary>
        public void Reboot()
        {
            if (IsScannersConnected())
            {
                string inXml = string.Format("<inArgs>{0}</inArgs>", GetOnlyScannerIDXml());
                int opCode = ScannerDefinitions.REBOOT_SCANNER;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;

                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("REBOOT_SCANNER\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}", opCode.ToString(), inXml, outXml, status.ToString());
                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region EnableVideoMode

        /// <summary>
        /// 
        /// </summary>
        public void EnableVideoMode()
        {
            if (IsScannersConnected())
            {
                string inXml = GetScannerIDXml();

                int opCode = ScannerDefinitions.DEVICE_CAPTURE_VIDEO;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;

                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("DEVICE_CAPTURE_VIDEO\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}", opCode.ToString(), inXml, outXml, status.ToString());
                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region EnableVideoMode

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableVideoMode"></param>
        public void EnableVideoMode(bool enableVideoMode)
        {
            PerformOnVideoViewFinder(enableVideoMode);
        }

        #endregion

        #region SetBeepLevel (not implemented)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beeperLevel"></param>
        //public void SetBeepLevel (ScannerDefinitions.BeeperLevel beeperLevel)
        //{
        //    try
        //    {
        //        if (IsScannersConnected())
        //        {
        //            string inXml = string.Format("<inArgs>{0}<cmdArgs><arg-int>{1}</arg-int></cmdArgs></inArgs>", GetOnlyScannerIDXml(), (int)beeperLevel);

        //            //string inXml = "<inArgs>" +
        //            //                    GetOnlyScannerIDXml() +
        //            //                    "<cmdArgs>" +
        //            //                    "<arg-int>" + comboBeep.SelectedIndex
        //            //                    + "</arg-int>" +
        //            //                    "</cmdArgs>" +
        //            //                    "</inArgs>";

        //            //int opCode = ScannerDefinitions.SET_ACTION;

        //            int opCode = ScannerDefinitions.DEVICE_BEEP_CONTROL;

        //            string outXml = "";
        //            int status = ScannerDefinitions.STATUS_FALSE;
        //            ExecCmd(opCode, ref inXml, out outXml, out status);

        //            string message = string.Format("SET_ACTION\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
        //                opCode.ToString(), inXml, outXml, status.ToString());

        //            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logFile.LogException("ScannerLogic.SetBeepLevel", ex);
        //        SendErrorInfo(ex.Message);
        //    }
        //}

        #endregion

        #region Close

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            if (_IsScannerOpen)
            {
                int appHandle = 0;
                int status = ScannerDefinitions.STATUS_FALSE;

                _Scanner.Close(appHandle, out status);

                if (ScannerDefinitions.STATUS_SUCCESS == status)
                {
                    _IsScannerOpen = false;

                    _TotalNumberOfScanners = 0;
                    InitScannersCount();
                }
            }
        }

        #endregion

        #endregion

        #region Private Functions

        #region Keyboard_Emulator_Set_Locale

        /// <summary>
        /// 
        /// </summary>
        private void Keyboard_Emulator_Set_Locale()
        {
            int opCode = ScannerDefinitions.KEYBOARD_EMULATOR_SET_LOCALE;
            int status = ScannerDefinitions.STATUS_FALSE;
            string outXml = "";

            string inXml = "<inArgs><cmdArgs><arg-int>0</arg-int></cmdArgs></inArgs>";

            _Scanner.ExecCommand(opCode, ref inXml, out outXml, out status);

            string message = string.Format("KEYBOARD_EMULATOR_SET_LOCALE\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                opCode.ToString(), inXml, outXml, status.ToString());

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        }

        #endregion

        #region Keyboard_Emulator_Get_Config

        /// <summary>
        /// 
        /// </summary>
        private void Keyboard_Emulator_Get_Config()
        {
            int opCode = ScannerDefinitions.KEYBOARD_EMULATOR_GET_CONFIG;
            string outXML = "";
            string inXML = "<inArgs></inArgs>";
            int status = ScannerDefinitions.STATUS_FALSE;

            _Scanner.ExecCommand(opCode, ref inXML, out outXML, out status);

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(outXML);

            string enable = xdoc.DocumentElement.GetElementsByTagName("KeyEnumState").Item(0).InnerText;
            string language = xdoc.DocumentElement.GetElementsByTagName("KeyEnumLocale").Item(0).InnerText;

            string message = string.Format("KEYBOARD_EMULATOR_GET_CONFIG\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}\r\nEnabled: {4}\r\nLanguage: {5}", 
                opCode.ToString(), inXML, outXML, status.ToString(),enable.ToString(), language.ToString());

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
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

            logFile.DoLogging = (bool)reader.GetValue("FileLogging.ScannerLogic.DoLogging", typeof(bool));
            logFile.DoDeleteLogFiles = (bool)reader.GetValue("FileLogging.ScannerLogic.DoDeleteLogFiles", typeof(bool));

            logFile.LogFilePath = (string)reader.GetValue("FileLogging.ScannerLogic.LogFilePath", typeof(string));
            logFile.LogFileName = (string)reader.GetValue("FileLogging.ScannerLogic.LogFileName", typeof(string));
            logFile.LogFileExtension = (string)reader.GetValue("FileLogging.ScannerLogic.LogFileExtension", typeof(string));

            logFile.LogFileArchivePath = (string)reader.GetValue("FileLogging.ScannerLogic.LogFileArchivePath", typeof(string));
            logFile.LogFileSearchPattern = (string)reader.GetValue("FileLogging.ScannerLogic.SearchPattern", typeof(string));

            logFile.DaysToKeepLogFiles = (int)reader.GetValue("FileLogging.ScannerLogic.DaysToKeepLogFiles", typeof(int));
            logFile.DaysToArchiveLogFiles = (int)reader.GetValue("FileLogging.ScannerLogic.DaysToArchiveLogFiles", typeof(int));

            logFile.LogStart("ImageScan");
        }

        #endregion

        #endregion
    }
}
