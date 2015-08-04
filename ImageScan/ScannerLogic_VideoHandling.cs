using System;
using System.Drawing;
using System.IO;

namespace ImageScan
{
    partial class ScannerLogic
    {
        #region PerformOnVideoViewFinder

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableVideoMode"></param>
        private void PerformOnVideoViewFinder(bool enableVideoMode)
        {
            if (!IsScannersConnected())
            {
                return;
            }

            m_arViewFindParamsList[0] = ScannerDefinitions.VIDEOVIEWFINDER_PARAMNUM;

            if (enableVideoMode)
            {
                m_arViewFindParamsList[1] = ScannerDefinitions.VIDEOVIEWFINDER_ON;
            }
            else
            {
                m_arViewFindParamsList[1] = ScannerDefinitions.VIDEOVIEWFINDER_OFF;
            }

            string inXml = string.Format("<inArgs>{0}<cmdArgs><arg-xml><attrib_list><attribute><id>{1}</id><datatype>B</datatype><value>{2}</value></attribute></attrib_list></arg-xml></cmdArgs></inArgs>",
                GetOnlyScannerIDXml(), m_arViewFindParamsList[0].ToString(), m_arViewFindParamsList[1].ToString());

            int opCode = ScannerDefinitions.DEVICE_SET_PARAMETERS;
            string outXml = "";
            int status = ScannerDefinitions.STATUS_FALSE;

            ExecCmd(opCode, ref inXml, out outXml, out status);

            string message = string.Format("SET_PARAMETERS(PerformOnVideoViewFinder)\r\nOpCode: {0}\r\nInXML: {1}\r\nOutXML: {2}\r\nStatus: {3}",
                opCode.ToString(), inXml, outXml, status.ToString());

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        }

        #endregion

        #region OnVideoEvent

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="size"></param>
        /// <param name="sfvideoData"></param>
        /// <param name="pScannerData"></param>
        private void OnVideoEvent(short eventType, int size, ref object sfvideoData, ref string pScannerData)
        {
            Array arr = (Array)sfvideoData;
            long len = arr.LongLength;
            byte[] byImage = new byte[size];
            arr.CopyTo(byImage, 0);

            MemoryStream ms = new MemoryStream();
            ms.Write(byImage, 0, byImage.Length);
            Image img = Image.FromStream(ms);

            SendVideoInfo(img);

            string message = string.Format("OnVideoEvent\r\nEventType: {0}\r\nSize: {1}\r\nScannnerData: {2}",
                eventType.ToString(), size.ToString(), pScannerData);

            logFile.LogMessage(message, FileLogging.MessageType.InfoMessage);
        }

        #endregion
    }
}
