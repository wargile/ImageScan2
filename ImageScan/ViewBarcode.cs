using System;
using System.Collections.Generic;
using System.Xml;

namespace ImageScan
{
    class ViewBarcode
    {
        #region GetSymbology

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        private static string GetSymbology(int Code)
        {
            switch (Code)
            {
                case ScannerDefinitions.ST_NOT_APP: return "NOT APPLICABLE";
                case ScannerDefinitions.ST_CODE_39: return "CODE 39";
                case ScannerDefinitions.ST_CODABAR: return "CODABAR";
                case ScannerDefinitions.ST_CODE_128: return "CODE 128";
                case ScannerDefinitions.ST_D2OF5: return "D 2 OF 5";
                case ScannerDefinitions.ST_IATA: return "IATA";
                case ScannerDefinitions.ST_I2OF5: return "I 2 OF 5";
                case ScannerDefinitions.ST_CODE93: return "CODE 93";
                case ScannerDefinitions.ST_UPCA: return "UPCA";
                case ScannerDefinitions.ST_UPCE0: return "UPCE 0";
                case ScannerDefinitions.ST_EAN8: return "EAN 8";
                case ScannerDefinitions.ST_EAN13: return "EAN 13";
                case ScannerDefinitions.ST_CODE11: return "CODE 11";
                case ScannerDefinitions.ST_CODE49: return "CODE 49";
                case ScannerDefinitions.ST_MSI: return "MSI";
                case ScannerDefinitions.ST_EAN128: return "EAN 128";
                case ScannerDefinitions.ST_UPCE1: return "UPCE 1";
                case ScannerDefinitions.ST_PDF417: return "PDF 417";
                case  ScannerDefinitions.ST_CODE16K: return "CODE 16K";
                case  ScannerDefinitions.ST_C39FULL: return "C39FULL";
                case  ScannerDefinitions.ST_UPCD: return "UPCD";
                case  ScannerDefinitions.ST_TRIOPTIC: return "TRIOPTIC";
                case  ScannerDefinitions.ST_BOOKLAND: return "BOOKLAND";
                case  ScannerDefinitions.ST_COUPON: return "COUPON";
                case  ScannerDefinitions.ST_NW7: return "NW7";
                case  ScannerDefinitions.ST_ISBT128: return "ISBT128";
                case  ScannerDefinitions.ST_MICRO_PDF: return "MICRO PDF";
                case  ScannerDefinitions.ST_DATAMATRIX: return "DATAMATRIX";
                case  ScannerDefinitions.ST_QR_CODE: return "QR CODE";
                case  ScannerDefinitions.ST_MICRO_PDF_CCA: return "MICRO PDF CCA";
                case  ScannerDefinitions.ST_POSTNET_US: return "POSTNET US";
                case  ScannerDefinitions.ST_PLANET_CODE: return "PLANET CODE";
                case  ScannerDefinitions.ST_CODE_32: return "CODE 32";
                case  ScannerDefinitions.ST_ISBT128_CON: return "ISBT 128 CON";
                case  ScannerDefinitions.ST_JAPAN_POSTAL: return "JAPAN POSTAL";
                case  ScannerDefinitions.ST_AUS_POSTAL: return "AUS POSTAL";
                case  ScannerDefinitions.ST_DUTCH_POSTAL: return "DUTCH POSTAL";
                case  ScannerDefinitions.ST_MAXICODE: return "MAXICODE";
                case  ScannerDefinitions.ST_CANADIN_POSTAL: return "CANADA POSTAL";
                case  ScannerDefinitions.ST_UK_POSTAL: return "UK POSTAL";
                case  ScannerDefinitions.ST_MACRO_PDF: return "MACRO PDF";
                case  ScannerDefinitions.ST_RSS14: return "RSS 14";
                case  ScannerDefinitions.ST_RSS_LIMITET: return "RSS LIMITET";
                case  ScannerDefinitions.ST_RSS_EXPANDED: return "RSS EXPANDED";
                case  ScannerDefinitions.ST_SCANLET: return "ST SCANLET";
                case  ScannerDefinitions.ST_UPCA_2: return "UPCA 2";
                case  ScannerDefinitions.ST_UPCE0_2: return "UPCE0 2";
                case  ScannerDefinitions.ST_EAN8_2: return "EAN8 2";
                case  ScannerDefinitions.ST_EAN13_2: return "EAN13 2";
                case  ScannerDefinitions.ST_UPCE1_2: return "UPCE1 2";
                case  ScannerDefinitions.ST_CCA_EAN128: return "CCA EAN 128";
                case  ScannerDefinitions.ST_CCA_EAN13: return "CCA EAN 13";
                case  ScannerDefinitions.ST_CCA_EAN8: return "CCA EAN 8";
                case  ScannerDefinitions.ST_CCA_RSS_EXPANDED: return "CCA RSS EXPANDED";
                case  ScannerDefinitions.ST_CCA_RSS_LIMITED: return "CCA RSS LIMITED";
                case  ScannerDefinitions.ST_CCA_RSS14: return "CCA RSS 14";
                case  ScannerDefinitions.ST_CCA_UPCA: return "CCA UPCA";
                case  ScannerDefinitions.ST_CCA_UPCE: return "CCA UPCE";
                case  ScannerDefinitions.ST_CCC_EAN128: return "CCC EAN 128";
                case  ScannerDefinitions.ST_TLC39: return "TLC39";
                case  ScannerDefinitions.ST_CCB_EAN128: return "CCB EAN 128";
                case  ScannerDefinitions.ST_CCB_EAN13: return "CCB EAN 13";
                case  ScannerDefinitions.ST_CCB_EAN8: return "CCB EAN 8";
                case  ScannerDefinitions.ST_CCB_RSS_EXPANDED: return "CCB RSS EXPANDED";
                case  ScannerDefinitions.ST_CCB_RSS_LIMITED: return "CCB RSS LIMITED";
                case  ScannerDefinitions.ST_CCB_RSS14: return "CCB RSS 14";
                case  ScannerDefinitions.ST_CCB_UPCA: return "CCB UPCA";
                case  ScannerDefinitions.ST_CCB_UPCE: return "CCB UPCE";
                case  ScannerDefinitions.ST_SIGNATURE_CAPTURE: return "SIGNATURE CAPTURE";
                case  ScannerDefinitions.ST_MATRIX2OF5: return "MATRIX 2 OF 5";
                case  ScannerDefinitions.ST_CHINESE2OF5: return "CHINESE 2 OF 5";
                case  ScannerDefinitions.ST_UPCA_5: return "UPCA 5";
                case  ScannerDefinitions.ST_UPCE0_5: return "UPCE0 5";
                case  ScannerDefinitions.ST_EAN8_5: return "EAN8 5";
                case  ScannerDefinitions.ST_EAN13_5: return "EAN13 5";
                case  ScannerDefinitions.ST_UPCE1_5: return "UPCE1 5";
                case  ScannerDefinitions.ST_MACRO_MICRO_PDF: return "MACRO MICRO PDF";
                case  ScannerDefinitions.ST_MICRO_QR_CODE: return "MICRO QR CODE";
                case  ScannerDefinitions.ST_AZTEC: return "AZTEC";
                default: return "";
            }
        }

        #endregion

        #region ShowBarcodeLabel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ShowBarcodeLabel(string strXml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(strXml);

            string barCodeString = "";
            string barcode = xdoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
            string symbology = xdoc.DocumentElement.GetElementsByTagName("datatype").Item(0).InnerText;

            string[] numbers = barcode.Split(' ');

            foreach (string number in numbers)
            {
                if (null == number || "" == number)
                    break;

                barCodeString += ((char)Convert.ToInt32(number, 16)).ToString();
            }

            string synbologi= GetSymbology((int)Convert.ToInt32(symbology));

            Dictionary<string, string> returnValues = new Dictionary<string, string>();
            returnValues.Add(barCodeString, synbologi);

            return returnValues;
        }

        #endregion
    }
}
