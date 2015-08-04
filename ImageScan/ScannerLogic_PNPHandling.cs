using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageScan
{
    partial class ScannerLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="ppnpData"></param>
        void OnPNPEvent(short eventType, ref string ppnpData)
        {
            switch (eventType)
            {
                case ScannerDefinitions.SCANNER_ATTACHED:
                    SendPNPInfo(true);
                    break;

                case ScannerDefinitions.SCANNER_DETTACHED:
                    SendPNPInfo(false);
                    break;
            }

            //InitializeScanner();
        }
    }
}
