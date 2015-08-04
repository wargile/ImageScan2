using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ImageScan
{
    /// <summary>
    /// Class handling logging information
    /// </summary>
    public class FileLogging
    {
        #region Private variables

        private bool doLogging = false;
        private bool doDeleteLogFiles = false;

        private string logFilePath = string.Empty;
        private string logFileExtension = string.Empty;
        private string logFileName = string.Empty;
        private string logFileFullName = string.Empty;
        private string logFileSearchPattern = string.Empty;
        private string logFileArchivePath = string.Empty;

        private int daysToKeepLogFiles = 0;
        private int daysToArchiveLogFiles = 0;

        #endregion

        #region Public Properties

        public bool DoLogging
        {
            get { return doLogging; }
            set { doLogging = value; }
        }

        public bool DoDeleteLogFiles
        {
            get { return doDeleteLogFiles; }
            set { doDeleteLogFiles = value; }
        }

        public string LogFilePath
        {
            get { return logFilePath; }

            set 
            { 
                logFilePath = value;
                CreateDirectory(logFilePath);
            }
        }

        public string LogFileExtension
        {
            get { return logFileExtension; }

            set 
            { 
                logFileExtension = value;
                logFileFullName = CreateLogFileName();
            }
        }

        public string LogFileName
        {
            get { return logFileName; }

            set 
            { 
                logFileName = value;
                logFileFullName = CreateLogFileName();
            }
        }

        public string LogFileArchivePath
        {
            get { return logFileArchivePath; }

            set 
            { 
                logFileArchivePath = value;
                CreateDirectory(logFileArchivePath);
            }
        }


        public string LogFileSearchPattern
        {
            get { return logFileSearchPattern; }
            set { logFileSearchPattern = value; }
        }

        public int DaysToKeepLogFiles
        {
            get { return daysToKeepLogFiles; }
            set { daysToKeepLogFiles = value; }
        }

        public int DaysToArchiveLogFiles
        {
            get { return daysToArchiveLogFiles; }
            set { daysToArchiveLogFiles = value; }
        }

        public string LogFileFullName
        {
            get { return logFileFullName; }
            set { logFileFullName = value; }
        }

        #endregion

        #region Public Enums

        /// <summary>
        /// 
        /// </summary>
        public enum MessageType
        {
            InfoMessage,
            ErrorMessage,
            ApplicationExceptionMessage,
            ExceptionMessage
        }


        #endregion

        #region Public Logging methods

        /// <summary>
        /// Logs start of operation
        /// </summary>
        /// <param name="applicationName"></param>
        public void LogStart(string applicationName)
        {
            if (doLogging)
            {
                string separator = new string('-', 50);
                string message = string.Format("{0}\r\n{1} {2}\r\n{3} starter logging.\r\n", separator, DateTime.Now.ToString(), MessageType.InfoMessage.ToString(), applicationName);
                WriteMessage(message);
            }
        }

        /// <summary>
        /// Logs end of operation
        /// </summary>
        /// <param name="applicationName"></param>
        public void LogEnd(string applicationName)
        {
            if (doLogging)
            {
                string message = string.Format("{0} {1}\r\n{2} avslutter logging...\r\n", DateTime.Now.ToString(), MessageType.InfoMessage.ToString(), applicationName);
                WriteMessage(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(string function, Exception exception)
        {
            if (doLogging)
            {
                string exceptionMessage = CreateExceptionString(exception);
                exceptionMessage = string.Format("Funksjon: {0}\r\n{1}", function, exceptionMessage);
                WriteMessage(exceptionMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void LogMessage(string message, MessageType type)
        {
            if (doLogging)
            {
                message = string.Format("{0} {1}\r\n{2}\r\n", DateTime.Now.ToString(), type.ToString(), message);
                WriteMessage(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whatToLog"></param>
        /// <param name="items"></param>
        /// <param name="type"></param>
        public void LogCollection(string whatToLog, List<string> items, MessageType type)
        {
            if (doLogging)
            {
                string message = string.Format("{0} {1}\r\n", DateTime.Now.ToString(), type.ToString());

                foreach (string item in items)
                {
                    message += string.Format("{0}: {1}\r\n", whatToLog, item);
                }
          
                WriteMessage(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whatToLog"></param>
        /// <param name="items"></param>
        /// <param name="type"></param>
        public void LogCollection(string whatToLog, List<FileInfo> items, MessageType type)
        {
            if (doLogging)
            {
                string message = string.Format("{0} {1}\r\n", DateTime.Now.ToString(), type.ToString());

                foreach (FileInfo item in items)
                {
                    message += string.Format("{0}: {1}\r\n", whatToLog, item.FullName);
                }

                WriteMessage(message);
            }
        }

        

        #endregion

        #region Write to File Metods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void WriteMessage(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            bool doLogFileMaintence = false;

            try
            {
                if (!File.Exists(logFileFullName))
                {
                    doLogFileMaintence = true;
                }

                WriteToStream(message, ref fileStream, ref streamWriter);

                if (doLogFileMaintence)
                {
                    ArchiveLogFiles();
                    DeleteLogFiles();
                }
            }             
            catch(IOException ioEx)
            {
                try
                {
                    System.Threading.Thread.Sleep(50);
                    WriteToStream(message, ref fileStream, ref streamWriter);
                }
                catch (Exception)
                {
                    DisposeStream(fileStream, streamWriter, ioEx);
                }
            }
            catch (Exception ex)
            {
                DisposeStream(fileStream, streamWriter, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="streamWriter"></param>
        /// <param name="ex"></param>
        private void DisposeStream(FileStream fileStream, StreamWriter streamWriter, Exception ex)
        {
            if (streamWriter != null)
            {
                streamWriter.Flush();
                streamWriter.Close();
            }

            if (fileStream != null)
            {
                fileStream.Close();
            }

            Utilities.WriteToEventlog(ex.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileStream"></param>
        /// <param name="streamWriter"></param>
        private void WriteToStream(string message, ref FileStream fileStream, ref StreamWriter streamWriter)
        {
            fileStream = new FileStream(logFileFullName, FileMode.Append);
            streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(message);

            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        #endregion

        #region Logfile Maintenance

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int ArchiveLogFiles()
        {
            int archivedFiles = 0;

            List<string> logFiles = FileIO.GetFilteredFilesInDirecory(LogFilePath, LogFileSearchPattern);

            foreach (string logFile in logFiles)
            {
                FileInfo logFileData = new FileInfo(logFile);
                DateTime created = logFileData.CreationTime;

                if (created.AddDays(daysToArchiveLogFiles) < DateTime.Now)
                {
                    FileIO.FileMove(logFile, LogFileArchivePath);
                    archivedFiles += 1;

                    string message = string.Format("Arkivert logfile: {0}\r\n", logFile);
                    WriteMessage(message);
                }
            }

            return archivedFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        private int DeleteLogFiles()
        {
            int deletedFiles = 0;

            if (doDeleteLogFiles)
            {
                List<string> logFiles = FileIO.GetFilteredFilesInDirecory(LogFileArchivePath, LogFileSearchPattern);

                foreach (string logFile in logFiles)
                {
                    FileInfo logFileData = new FileInfo(logFile);
                    DateTime created = logFileData.CreationTime;

                    if (created.AddDays(daysToKeepLogFiles) < DateTime.Now)
                    {
                        File.Delete(logFile);
                        deletedFiles += 1;

                        string message = string.Format("Slettet logfile: {0}\r\n", logFile);
                        WriteMessage(message);
                    }
                }
            }

            return deletedFiles;
        }

        #endregion

        #region Private Utilities

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string CreateLogFileName()
        {
            string returnValue = string.Format("{0}{1}_{2}.{3}",
                logFilePath,
                logFileName,
                DateTime.Now.ToString("yyyyMMdd"),
                logFileExtension);

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private string CreateExceptionString(Exception exception)
        {
            string exceptionType = exception.GetType().ToString();
            string innerExceptionMessage = "InnerException: Null\r\n";

            string exceptionMessage = string.Format("ExceptionType: {0}\r\nMessage: {1}\r\nStackTrace: {2}\r\n",
                exceptionType,
                exception.Message, 
                exception.StackTrace);

            if(exception.InnerException != null)
            {
                string innerExceptionType = exception.InnerException.GetType().ToString();

                innerExceptionMessage = string.Format("InnerExceptionType: {0}\r\nMessage: {1}\r\nStackTrace: {2}\r\n",
                    innerExceptionType,
                    exception.InnerException.Message,
                    exception.InnerException.StackTrace);
            }

            return string.Format("{0} {1}\r\n{2}\r\n{3}", 
                DateTime.Now.ToString(), 
                MessageType.ExceptionMessage.ToString(), 
                exceptionMessage, 
                innerExceptionMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}
