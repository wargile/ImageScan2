using System.Collections.Generic;

namespace ImageScan
{
    partial class ScannerLogic
    {
        #region FunctionsRelated to BARCODE

        #region OnBarcodeEvent

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="pscanData"></param>
        private void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            string barCode = string.Empty;
            string symbiologi = string.Empty;

            Dictionary<string, string> barCodeInfos = ViewBarcode.ShowBarcodeLabel(pscanData);

            foreach (KeyValuePair<string, string> barCodeInfo in barCodeInfos)
            {
                barCode = barCodeInfo.Key;
                symbiologi = barCodeInfo.Value;
            }

            SendBarCodeInfo(barCode, symbiologi);

            string message = string.Format("OnBarcodeEvent\r\nEventType: {0}\r\nScannnerData: {1}",
                eventType.ToString(), pscanData);

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        }

        #endregion

        #endregion
    }
}
