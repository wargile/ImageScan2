
namespace ImageScan
{
    partial class ScannerLogic
    {
        #region FilterScannerList

        /// <summary>
        /// 
        /// </summary>
        private void FilterScannerList()
        {
            for (int i = 0; i < ScannerDefinitions.TOTAL_SCANNER_TYPES; i++)
            {
                m_arSelectedTypes[i] = false;
            }

            m_arSelectedTypes[ScannerDefinitions.SCANNER_TYPES_SNAPI - 1] = true;
        }

        #endregion

        #region MakeConnectCtrl

        /// <summary>
        /// 
        /// </summary>
        private void MakeConnectCtrl()
        {
            Connect();
        }

        #endregion

        #region RegisterForEvents

        /// <summary>
        /// 
        /// </summary>
        private void RegisterForEvents()
        {
            if (_IsScannerOpen)
            {
                int nEvents = 0;
                string strEvtIDs = GetRegUnregIDs(out nEvents);

                string inXml = string.Format("<inArgs><cmdArgs><arg-int>{0}</arg-int><arg-int>{1}</arg-int></cmdArgs></inArgs>", nEvents.ToString(), strEvtIDs);

                int opCode = ScannerDefinitions.REGISTER_FOR_EVENTS;
                string outXml = "";
                int status = ScannerDefinitions.STATUS_FALSE;

                ExecCmd(opCode, ref inXml, out outXml, out status);

                string message = string.Format("REGISTER_FOR_EVENTS\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                    opCode.ToString(), inXml, outXml, status.ToString());

                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

        #region ShowScanners

        /// <summary>
        /// 
        /// </summary>
        private void ShowScanners()
        {
            m_arScanners.Initialize();

            if (_IsScannerOpen)
            {
                _TotalNumberOfScanners = 0;
                short numOfScanners = 0;
                int nScannerCount = 0;
                string outXML = "";
                int status = ScannerDefinitions.STATUS_FALSE;
                int[] scannerIdList = new int[ScannerDefinitions.MAX_NUM_DEVICES];

                _Scanner.GetScanners(out numOfScanners, scannerIdList, out outXML, out status);

                if (ScannerDefinitions.STATUS_SUCCESS == status)
                {
                    _TotalNumberOfScanners = numOfScanners;
                    xmlReader.ReadXmlString_GetScanners(outXML, m_arScanners, numOfScanners, out nScannerCount);

                    FillScannerList();
                }
            }
        }

        #endregion

        #region Connect

        /// <summary>
        /// 
        /// </summary>
        private void Connect()
        {
            if (_IsScannerOpen)
            {
                return;
            }

            int appHandle = 0;
            GetSelectedScannerTypes();
            int status = ScannerDefinitions.STATUS_FALSE;

            _Scanner.Open(appHandle, _ScannerTypes, _NumberOfTypesOfScanners, out status);

            if (ScannerDefinitions.STATUS_SUCCESS == status)
            {
                _IsScannerOpen = true;
            }
        }

        #endregion

        #region GetSelectedScannerTypes

        /// <summary>
        /// 
        /// </summary>
        private void GetSelectedScannerTypes()
        {
            _NumberOfTypesOfScanners = 0;

            for (int i = 0, k = 0; i < ScannerDefinitions.TOTAL_SCANNER_TYPES; i++)
            {
                if (m_arSelectedTypes[i])
                {
                    _NumberOfTypesOfScanners++;

                    switch (i + 1)
                    {
                        case ScannerDefinitions.SCANNER_TYPES_SNAPI:
                            _ScannerTypes[k++] = ScannerDefinitions.SCANNER_TYPES_SNAPI;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region GetRegUnregIDs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nEvents"></param>
        /// <returns></returns>
        private string GetRegUnregIDs(out int nEvents)
        {
            string strIDs = string.Empty;

            nEvents = ScannerDefinitions.NUM_SCANNER_EVENTS;
            strIDs = ScannerDefinitions.SUBSCRIBE_BARCODE.ToString();
            strIDs += "," + ScannerDefinitions.SUBSCRIBE_IMAGE.ToString();
            strIDs += "," + ScannerDefinitions.SUBSCRIBE_VIDEO.ToString();
            strIDs += "," + ScannerDefinitions.SUBSCRIBE_RMD.ToString();
            strIDs += "," + ScannerDefinitions.SUBSCRIBE_PNP.ToString();
            strIDs += "," + ScannerDefinitions.SUBSCRIBE_OTHER.ToString();

            return strIDs;
        }

        #endregion

        #region FillScannerList

        /// <summary>
        /// 
        /// </summary>
        private void FillScannerList()
        {
            InitScannersCount();

            for (int i = 0; i < _TotalNumberOfScanners; i++)
            {
                Scanner scanr = (Scanner)m_arScanners.GetValue(i);

                string[] strItems = new string[] { "", "", "", "", "" };

                switch (scanr.SCANNERTYPE)
                {
                    case Scanner.SCANNER_SNAPI:
                        m_nArTotalScannersInType[0]++;
                        strItems = new string[] { scanr.SCANNERID, scanr.SCANNERTYPE, scanr.SERIALNO, scanr.MODELNO, scanr.GUID, scanr.SCANNERFIRMWARE, scanr.SCANNERMNFDATE };
                        break;

                    //case Scanner.SCANNER_SSI:
                    //    m_nArTotalScannersInType[1]++;
                    //    strItems = new string[] { scanr.SCANNERID, scanr.SCANNERTYPE, scanr.PORT, "", "", "", "" };
                    //    break;
                    //case Scanner.SCANNER_IBMHID:
                    //    m_nArTotalScannersInType[2]++;
                    //    strItems = new string[] { scanr.SCANNERID, "IBM HANDHELD", scanr.SERIALNO, scanr.MODELNO, scanr.GUID, scanr.SCANNERFIRMWARE, scanr.SCANNERMNFDATE };
                    //    break;
                    //case Scanner.SCANNER_NIXMODB:
                    //    m_nArTotalScannersInType[3]++;
                    //    strItems = new string[] { scanr.SCANNERID, scanr.SCANNERTYPE, scanr.PORT, "", "", "", "" };
                    //    break;
                    //case Scanner.SCANNER_HIDKB:
                    //    m_nArTotalScannersInType[4]++;
                    //    strItems = new string[] { scanr.SCANNERID, "HID KEYBOARD", scanr.SERIALNO, scanr.MODELNO, scanr.GUID, "", "" };
                    //    break;
                }
            }

            if (_TotalNumberOfScanners == 0)
            {
                SendErrorInfo("Ingen scanner er tilkoblet.\r\nApplikasjonen kan ikke brukes før en scanner tilkobles.");
            }
        }

        #endregion

        #region InitScannersCount

        /// <summary>
        /// 
        /// </summary>
        private void InitScannersCount()
        {
            for (int i = 0; i < 5; i++)
            {
                m_nArTotalScannersInType[i] = 0;
            }
        }

        #endregion

    }
}
