using System;
using System.Drawing;
using System.IO;

namespace ImageScan
{
    partial class ScannerLogic
    {
        #region PerformOnJpg

        /// <summary>
        /// 
        /// </summary>
        private void PerformOnJpg()
        {
            if (!IsScannersConnected())
            {
                return;
            }
            string inXml = string.Format("<inArgs>{0}<cmdArgs><arg-xml><attrib_list><attribute><id>{1}</id><datatype>B</datatype><value>{2}</value></attribute></attrib_list></arg-xml></cmdArgs></inArgs>",
                GetOnlyScannerIDXml(), ScannerDefinitions.IMAGE_FILETYPE_PARAMNUM, ScannerDefinitions.JPEG_FILE_SELECTION);

            int opCode = ScannerDefinitions.DEVICE_SET_PARAMETERS;
            string outXml = "";
            int status = ScannerDefinitions.STATUS_FALSE;
            ExecCmd(opCode, ref inXml, out outXml, out status);

            string message = string.Format("SET_PARAMETERS\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                opCode.ToString(), inXml, outXml, status.ToString());

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        }

        #endregion
         
        #region OnImageEvent

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="size"></param>
        /// <param name="imageFormat"></param>
        /// <param name="sfimageData"></param>
        /// <param name="pScannerData"></param>
        private void OnImageEvent(short eventType, int size, short imageFormat, ref object sfimageData, ref string pScannerData)
        {
            if (ScannerDefinitions.IMAGE_COMPLETE == eventType)
            {
                Array arr = (Array)sfimageData;
                long len = arr.LongLength;
                byte[] byImage = new byte[len];
                arr.CopyTo(byImage, 0);

                MemoryStream ms = new MemoryStream();
                ms.Write(byImage, 0, byImage.Length);

                Image img = Image.FromStream(ms);
                imgCapturedImage = img;
                SendImageInfo(imgCapturedImage);

                string message = string.Format("OnImageEvent\r\nEventType: {0}\r\nSize: {1}\r\nImageFormat: {2}\r\nScannnerData: {3}",
                    eventType.ToString(), size.ToString(), imageFormat.ToString(), pScannerData);

                logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
            }
        }

        #endregion

    }
}
