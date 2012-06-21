﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.IO;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.Common
{
	/// <summary>
	/// Utility class for querying information about a disk.
	/// </summary>
	public class Diskspace
	{
        private static readonly string[] Suffix = new[] { SR.LabelBytes, SR.LabelKilobytes, SR.LabelMegabytes, SR.LabelGigabytes, SR.LabelTerabytes, SR.LabelPetabytes };

        private DriveInfo _driveInfo;

	    // TODO (CR Jun 2012): the fact that these values get cached is not totally clear, and has caused problems once already.
        // Need to rethink this a bit, and maybe build in a refresh time interval.

        //Mostly for unit testing.
        private bool? _isAvailable;
        private long? _totalSpace;
        private long? _freeSpace;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Diskspace(string name)
            :this (new DriveInfo(name))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Diskspace(DriveInfo driveInfo)
        {
            DriveInfo = driveInfo;
            LastRefresh = Platform.Time;
        }

        /// <summary>
        /// Constructor for unit testing.
        /// </summary>
        internal Diskspace()
        {
            _isAvailable = true;
            LastRefresh = Platform.Time;
        }

        public DateTime LastRefresh { get; private set; }

	    public DriveInfo DriveInfo
	    {
	        get { return _driveInfo; }
	        private set
	        {
	            _driveInfo = value;
                Refresh();
	        }
	    }

	    public void Refresh()
	    {
	        _isAvailable = null;
            _totalSpace = null;
            _freeSpace = null;
            LastRefresh = Platform.Time;
        }

        /// <summary>
		/// Formats the specified number of bytes into a human-readable value with units appropriate to the scale of the number.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string FormatBytes(double bytes)
        {
            return FormatBytes(bytes, "F1");
        }

        /// <summary>
        /// Formats the specified number of bytes into a human-readable value with units appropriate to the scale of the number.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="decimalFormatString"></param>
        /// <returns></returns>
        public static string FormatBytes(double bytes, string decimalFormatString)
    	{
			int powerOfKB = 0;
            var kilo = bytes / 1024.0;
            while (kilo >= 1.0 && powerOfKB < Suffix.Length-1)
            {
                bytes = kilo;
                ++powerOfKB;
                kilo = bytes / 1024.0;
            }
            
            string formatString = "{0:" + decimalFormatString + "} {1}";
			return String.Format(formatString, bytes, Suffix[powerOfKB]);
		}

	    public bool IsAvailable
	    {
            get { return _isAvailable.HasValue? _isAvailable.Value : _driveInfo.IsReady; }
	    }

        public long TotalSpace
        {
            get { return _totalSpace.HasValue ? _totalSpace.Value : (_totalSpace = _driveInfo.TotalSize).Value; }
            internal set { _totalSpace = value; }
        }

        public long UsedSpace
        {
            get { return TotalSpace - FreeSpace; }
        }

        public double UsedSpacePercent
        {
            get { return (double)UsedSpace / TotalSpace * 100; }
        }

        public long FreeSpace
        {
            get { return _freeSpace.HasValue ? _freeSpace.Value : (_freeSpace = _driveInfo.AvailableFreeSpace).Value; }
            internal set { _freeSpace = value; }
        }

        public double FreeSpacePercent
        {
            get
            {
                return (double)FreeSpace / TotalSpace * 100;
            }
        }
	}
}
