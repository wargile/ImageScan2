
namespace ImageScan
{
    partial class ScannerLogic
    {
        #region ExecCmd

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="inXml"></param>
        /// <param name="outXml"></param>
        /// <param name="status"></param>
        private void ExecCmd(int opCode, ref string inXml, out string outXml, out int status)
        {
            outXml = string.Empty;
            status = ScannerDefinitions.STATUS_FALSE;

            if (_IsScannerOpen)
            {
                _Scanner.ExecCommand(opCode, ref inXml, out outXml, out status);
            }
        }

        #endregion

        #region IsScannersConnected

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsScannersConnected()
        {
            return (_IsScannerOpen && (0 < _TotalNumberOfScanners));
        }

        #endregion

        #region GetScannerIDXml

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetScannerIDXml()
        {
            string strInXml = string.Empty;

            strInXml = string.Format("<inArgs><scannerID>{0}</scannerID></inArgs>", GetSelectedScannerID());
            return strInXml;
        }

        #endregion

        #region GetSelectedScannerID

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetSelectedScannerID()
        {
            string strSelScnID = "1";
            return strSelScnID;
        }

        #endregion

        #region GetOnlyScannerIDXml

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetOnlyScannerIDXml()
        {
            string strInXml = string.Empty;
            strInXml = string.Format("<scannerID>{0}</scannerID>", GetSelectedScannerID());

            return strInXml;
        }

        #endregion
    }
}
