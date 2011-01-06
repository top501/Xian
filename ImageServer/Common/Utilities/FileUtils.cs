#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.IO;
using System.Threading;

namespace ClearCanvas.ImageServer.Common.Utilities
{
	/// <summary>
	/// File related utilities.
	/// </summary>
	public class FileUtils
	{
		private const int RETRY_MIN_DELAY = 100; // 100 ms
		private const long RETRY_MAX_DELAY = 10 * 1000; // 10 Seconds 
    
		/// <summary>
		/// Replacement for <see cref="File.Delete"/> that retries if the file is in use.
		/// </summary>
		/// <param name="path">The path to delete</param>
		static public void Delete(string path)
		{
			Delete(path, RETRY_MAX_DELAY, null, RETRY_MIN_DELAY);
		}

		/// <summary>
		/// Replacement for <see cref="File.Delete"/> that retries if the file is in use.
		/// </summary>
		/// <param name="path">The path to delete.</param>
		/// <param name="timeout">The timeout in milliseconds to attempt to retry to delete the file</param>
		/// <param name="stopSignal">An optional stopSignal to tell the delete operation to stop if retrying</param>
		/// <param name="retryMinDelay">The minimum number of milliseconds to delay when deleting.</param>
		static public void Delete(string path, long timeout, ManualResetEvent stopSignal, int retryMinDelay)
		{
            Exception lastException = null;
			long begin = Environment.TickCount;
			bool cancelled = false;

            while (!cancelled)
			{
				try
				{
                    if (!File.Exists(path))
                        return; // the file is gone.

					File.Delete(path);
					return;
				}
				catch (IOException e)
				{
					// other IO exceptions should be treated as retry
					lastException = e;
					Random rand = new Random();
					Thread.Sleep(rand.Next(retryMinDelay, 2*retryMinDelay));
				}

				if (timeout > 0 && Environment.TickCount - begin > timeout)
				{
					if (lastException != null)
						throw lastException;
					else
						throw new TimeoutException();
				}

				if (stopSignal != null)
				{
					cancelled = stopSignal.WaitOne(TimeSpan.FromMilliseconds(retryMinDelay), false);
				}
			}

            if (!cancelled)
                throw lastException;
		}


        /// <summary>
        /// Replacement for <see cref="File.Copy"/> that retries if the file is in use.
        /// </summary>
        /// <param name="source">The path to copy from.</param>
        /// <param name="destination">The path to copy to.</param>
        /// <param name="overwrite">Overwrite an existing destination file.</param>
        static public void Copy(string source, string destination, bool overwrite)
        {
            Copy(source, destination, overwrite, RETRY_MAX_DELAY, null, RETRY_MIN_DELAY);
        }

        /// <summary>
        /// Replacement for <see cref="File.Copy"/> that retries if the file is in use.
        /// </summary>
        /// <param name="source">The path to copy from.</param>
        /// <param name="destination">The path to copy to.</param>
        /// <param name="overwrite">Boolean value to indicate whether to overwrite the destination if it exists</param>
        /// <param name="timeout">The timeout in milliseconds to attempt to retry to delete the file</param>
        /// <param name="stopSignal">An optional stopSignal to tell the delete operation to stop if retrying</param>
        /// <param name="retryMinDelay">The minimum number of milliseconds to delay when deleting.</param>
        static public void Copy(string source, string destination, bool overwrite,
                long timeout, ManualResetEvent stopSignal, int retryMinDelay)
        {
            
            Exception lastException = null;
            long begin = Environment.TickCount;
            bool cancelled = false;

            while (!cancelled)
            {
                try
                {
                    // check if the file still exists every time in case it is moved/deleted 
                    // so that we are not stucked in the loop
                    if (!File.Exists(source))
                        throw new FileNotFoundException(String.Format("Source file {0} does not exist", source), source);
                    
                    File.Copy(source, destination, overwrite);
                    return;
                }
                catch (IOException e)
                {
                    // other IO exceptions should be treated as retry
                    lastException = e;
                    Random rand = new Random();
                    Thread.Sleep(rand.Next(retryMinDelay, 2 * retryMinDelay));
                }

                if (timeout > 0 && Environment.TickCount - begin > timeout)
                {
                    if (lastException != null)
                        throw lastException;
                    else
                        throw new TimeoutException();
                }

                if (stopSignal != null)
                {
                    cancelled = stopSignal.WaitOne(TimeSpan.FromMilliseconds(retryMinDelay), false);
                }
            }

            if (!cancelled)
                throw lastException;
        }

        /// <summary>
        /// Creates copy of the specified file and returns the path to the backup file.
        /// This method allows a file to be backed up more than once with different extensions.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The path to the backup file. Null if the file is not backed up (it doesn't exist).</returns>
        /// <param name="inSourceFolder">A boolean value indicating whether to save the backup in the same folder</param>
        /// <remarks>If the file is in use, retry will be attempted until it succeeds.</remarks>
        static public string Backup(string source, bool inSourceFolder)
        {
            FileInfo sourceInfo = new FileInfo(source);
            if (File.Exists(source))
            {
                int i = 0;
                bool filenameAbtained = false;
                while(true)
                {
                    string backup;

                    // check if the file still exists every time in case it is moved/deleted 
                    // so that we are not stucked in the loop
                    if (!File.Exists(source))
                        return null;

                    if (inSourceFolder)
                        backup = Path.Combine(sourceInfo.Directory.FullName, String.Format("{0}.bak({1})", sourceInfo.Name, i));
                    else 
                        backup =  (CommandProcessor.ExecutionContext.Current != null)
                                     ? Path.Combine(CommandProcessor.ExecutionContext.Current.BackupDirectory, String.Format("{0}.bak({1})", sourceInfo.Name, i))
                                     : Path.Combine(sourceInfo.Directory.FullName, String.Format("{0}.bak({1})", sourceInfo.Name, i));
                
                    try
                    {
                    	using (FileStream stream =
                    			FileStreamOpener.OpenForSoleUpdate(backup, FileMode.CreateNew, RETRY_MIN_DELAY))
                    	{
                    		stream.Close();
							filenameAbtained = true;
                    	}
                    }
                    catch(Exception)
                    {
						//NOTE: this method would cause an infinite loop if the filesystem cannot be written to.
                        
						// try another file name
                    }

                    if (filenameAbtained)
                    {
                        Copy(source, backup, true);
                        return backup;
                    }
                }
            }

            return null;
                
        }

        /// <summary>
        /// Creates copy of the specified file and returns the path to the backup file.
        /// This method allows a file to be backed up more than once with different extensions.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>The path to the backup file. Null if the file is not backed up.</returns>
        static public string Backup(string source)
        {
            return Backup(source, false);
        }
	}
}
