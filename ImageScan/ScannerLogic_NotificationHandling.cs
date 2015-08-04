
namespace ImageScan
{
    partial class ScannerLogic
    {
        #region OnScannerNotification

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="pScannerData"></param>
        private void OnScannerNotification(short notificationType, ref string pScannerData)
        {
            string message = string.Empty;

            switch (notificationType)
            {
                case ScannerDefinitions.IMAGE_MODE:
                    message = "Scanner Notification : Image Mode";
                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
                    break;

                case ScannerDefinitions.VIDEO_MODE:
                    message = "Scanner Notification : Video Mode";
                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
                    break;

                case ScannerDefinitions.BARCODE_MODE:
                    message = "Scanner Notification : BarCode Mode";
                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
                    break;

                default:
                    message = string.Format("Scanner Notification : Unknown type: {0}", notificationType);
                    logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
                    break;
            }
        }

        #endregion
    }
}
