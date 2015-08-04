using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ImageScan
{
    public class FileIO
    {
        #region Private Variables

        private static string applicationIniFile = string.Empty;
        private static int _MinimumSequenceNumber = 501;
        private static int _MaximumSequenceNumber = 999;


        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        static FileIO()
        {
            AppSettingsReader reader = new AppSettingsReader();
            applicationIniFile = (string)reader.GetValue("ApplicationIniFile", typeof(string));

            _MinimumSequenceNumber = (int)reader.GetValue("MinimumSequenceNumber", typeof(int));
            _MaximumSequenceNumber = (int)reader.GetValue("MaximumSequenceNumber", typeof(int));

            if (!File.Exists(applicationIniFile))
            {
                ValidateIniFile();
            }
        }

        #endregion

        #region GetFilesInDirecory

        /// <summary>
        /// Return all files in directory as generic list of strings filtered by filtervalue
        /// </summary>
        /// <param name="sDir"></param>
        /// <param name="files"></param>
        public static List<string> GetFilteredFilesInDirecory(string directory, string filter)
        {
            List<string> files = new List<string>();

            string[] filesInArray = Directory.GetFiles(directory, filter);

            foreach (string file in filesInArray)
            {
                files.Add(file);
            }

            return files;
        }

        #endregion

        #region CreateDirectory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion

        #region FileMove

        /// <summary>
        /// 
        /// </summary>
        /// <param name="importFile"></param>
        /// <param name="archivePath"></param>
        public static string FileMove(string importFile, string archivePath)
        {
            FileInfo file = null;
            string newFile = string.Empty;
            string returnValue = string.Empty;

            try
            {
                FileIO.CreateDirectory(archivePath);

                file = new FileInfo(importFile);
                newFile = string.Format("{0}{1}", archivePath, file.Name);

                File.Move(importFile, newFile);
            }
            catch (Exception)
            {
                try
                {
                    returnValue = TryToMoveWithGeneratedName(archivePath, importFile, file.Name, file.Extension);
                }
                catch (Exception)
                {
                    returnValue = string.Format("Duplikat ved flytting av fil. Klarer ikke flytte med generert navn: {0}", newFile);
                }
            }

            return returnValue;
        }

        #endregion

        #region TryToMoveWithGeneratedName

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="importfFile"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private static string TryToMoveWithGeneratedName(string archivePath, string importfFile, string fileName, string fileExtension)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string randomFileName = Path.GetRandomFileName();
            randomFileName = Path.GetFileNameWithoutExtension(randomFileName);

            string newFileName = string.Format("{0}{1}.{2}{3}", archivePath, fileName, randomFileName, fileExtension);
            File.Move(importfFile, newFileName);

            string returnValue = string.Format("Duplikat ved flytting av fil. Fil: '{0}' endret navn til '{1}'", importfFile, newFileName);

            return returnValue;
        }

        #endregion

        #region GetSeqNumber

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long GetSeqNumber()
        {
            if (!File.Exists(applicationIniFile))
            {
                string message = string.Format("Finner ikke ImageScans Ini-fil: {0}", applicationIniFile);
                throw new ApplicationException(message);
            }

            long seqNr = long.MinValue;

            using (TextReader tr = new StreamReader(applicationIniFile))
            {
                seqNr = ExtractSeqNr(tr.ReadLine());
            }

            long newSeqNr = seqNr + 1;

            if (newSeqNr > _MaximumSequenceNumber)
            {
                newSeqNr = _MinimumSequenceNumber;
            }

            using (StreamWriter sw = new StreamWriter(applicationIniFile))
            {
                string output = string.Format("SeqNr={0}", newSeqNr);
                sw.Write(output);
            }

            return seqNr;
        }

        #endregion

        #region ExtractSeqNr

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqNrText"></param>
        /// <returns></returns>
        private static long ExtractSeqNr(string seqNrText)
        {
            long seqNr = long.MinValue;
            int startPos = seqNrText.IndexOf("=");
            int endPos = seqNrText.Length;

            if (startPos == -1)
            {
                string message = string.Format("Finner ikke løpenummer i Ini-fil: {0}", seqNrText);
                throw new ApplicationException(message);
            }

            startPos += 1;
            seqNrText = seqNrText.Substring(startPos, endPos - startPos);

            if (!long.TryParse(seqNrText, out seqNr))
            {
                string message = string.Format("Klarer ikke konvertere løpenummer fra Ini-fil: {0}", seqNrText);
                throw new ApplicationException(message);
            }

            if (seqNr == long.MinValue)
            {
                string message = string.Format("Klarer ikke trekke ut løpenummer fra Ini-fil: {0}", seqNrText);
                throw new ApplicationException(message);
            }

            return seqNr;
        }

        #endregion

        #region ValidateIniFile

        /// <summary>
        /// 
        /// </summary>
        private static void ValidateIniFile()
        {
            if (!File.Exists(applicationIniFile))
            {
                using (StreamWriter sw = new StreamWriter(applicationIniFile))
                {
                    string output = string.Format("SeqNr={0}", _MinimumSequenceNumber);
                    sw.Write(output);
                }
            }
        }

        #endregion

        #region DeleteFiles (not in use)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="numberOfDaysToKeep"></param>
        /// <returns></returns>
        //public static List<string> DeleteFiles(string archivePath, string searchPattern, int numberOfDaysToKeep)
        //{
        //    List<string> deletedFiles = new List<string>();
        //    List<string> files = GetFilteredFilesInDirecory(archivePath, searchPattern);

        //    foreach (string file in files)
        //    {
        //        FileInfo importFileData = new FileInfo(file);
        //        DateTime created = importFileData.CreationTime;

        //        if (created.AddDays(numberOfDaysToKeep) < DateTime.Now)
        //        {
        //            File.Delete(file);
        //            deletedFiles.Add(file);
        //        }
        //    }

        //    return deletedFiles;
        //}

        #endregion

        #region GetFilesInDirecory (not in use)

        /// <summary>
        /// Return all files in directory as generic list of strings
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        //public static List<string> GetFilesInDirecory(string directory)
        //{
        //    List<string> files = new List<string>();

        //    string[] filesInArray = Directory.GetFiles(directory);

        //    foreach (string file in filesInArray)
        //    {
        //        files.Add(file);
        //    }

        //    return files;
        //}

        #endregion

        #region CountNumberOfLinesInImportFile (not in use)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static int CountNumberOfLinesInImportFile(string importFile)
        //{
        //    int counter = 0;
        //    string line;

        //    System.IO.StreamReader file = new System.IO.StreamReader(importFile);

        //    while ((line = file.ReadLine()) != null)
        //    {
        //        counter++;
        //    }

        //    file.Close();
        //    return counter;
        //}

        #endregion

    }
}
