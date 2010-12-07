﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

#if	UNIT_TESTS
#pragma warning disable 1591,0419,1574,1587

using System.Drawing;
using ClearCanvas.Dicom.Tests;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Iod.Modules.Tests
{
	[TestFixture]
	public class OverlayPlaneTests
	{
		#region Basic Tests

		[Test]
		public void TestOverlayPlaneModuleIod_SingleFrame()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int imageRows = 97;
			const int imageColumns = 101;
			const int overlay1Rows = 103;
			const int overlay1Columns = 89;
			const int overlay2Rows = 67;
			const int overlay2Columns = 71;

			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[imageRows*imageColumns*2], imageRows, imageColumns, 1, 16, 13, 12, false);
			SetOverlay(dataset, 0, new bool[overlay1Rows*overlay1Columns], OverlayType.G, new Point(1, 1), overlay1Rows, overlay1Columns, bigEndian, false);
			SetOverlay(dataset, 1, new bool[imageRows*imageColumns], OverlayType.G, new Point(2, 3), 15, bigEndian);
			SetOverlay(dataset, 2, new bool[overlay2Rows*overlay2Columns], OverlayType.R, new Point(4, 5), overlay2Rows, overlay2Columns, bigEndian, true);
			SetOverlay(dataset, 13, new bool[imageRows*imageColumns], OverlayType.R, new Point(6, 7), 14, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);

			// test overlay group 6000
			{
				const int overlayIndex = 0;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6000, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x000000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(true, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(103*89, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(103, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(89, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.G, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(1, 1), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(null, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 6002
			{
				const int overlayIndex = 1;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6002, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x020000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(false, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(97*101, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(97, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(101, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(15, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(16, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.G, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(2, 3), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(null, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 6004
			{
				const int overlayIndex = 2;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6004, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(2, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x040000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(true, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(67*71, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(67, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(71, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.R, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(4, 5), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(null, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 601A
			{
				const int overlayIndex = 13;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x601A, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(13, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x1A0000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(false, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(97*101, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(97, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(101, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(14, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(16, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.R, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(6, 7), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(null, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}
		}

		[Test]
		public void TestOverlayPlaneModuleIod_Multiframe()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int imageRows = 97;
			const int imageColumns = 101;
			const int imageFrames = 11;
			const int overlay1Rows = 103;
			const int overlay1Columns = 89;
			const int overlay1Frames = 1;
			const int overlay2Rows = 67;
			const int overlay2Columns = 71;
			const int overlay2Frames = 7;

			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[imageRows*imageColumns*imageFrames*2], imageRows, imageColumns, imageFrames, 16, 13, 12, false);
			SetOverlay(dataset, 0, new bool[overlay1Rows*overlay1Columns*overlay1Frames], OverlayType.G, new Point(1, 1), overlay1Rows, overlay1Columns, overlay1Frames, 3, bigEndian, false);
			SetOverlay(dataset, 1, new bool[imageRows*imageColumns*imageFrames], OverlayType.G, new Point(2, 3), 15, bigEndian);
			SetOverlay(dataset, 2, new bool[overlay2Rows*overlay2Columns*overlay2Frames], OverlayType.R, new Point(4, 5), overlay2Rows, overlay2Columns, overlay2Frames, bigEndian, true);
			SetOverlay(dataset, 13, new bool[imageRows*imageColumns*imageFrames], OverlayType.R, new Point(6, 7), 14, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);

			// test overlay group 6000
			{
				const int overlayIndex = 0;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6000, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x000000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(true, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(103*89, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(103, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(89, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.G, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(1, 1), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(3, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 6002
			{
				const int overlayIndex = 1;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6002, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x020000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(false, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(97*101, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(97, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(101, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(15, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(16, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.G, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(2, 3), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(1, overlayPlane.ImageFrameOrigin.GetValueOrDefault(1), "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(11, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 6004
			{
				const int overlayIndex = 2;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x6004, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(2, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x040000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(true, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(67*71, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(67, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(71, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(0, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(1, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.R, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(4, 5), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(7, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}

			// test overlay group 601A
			{
				const int overlayIndex = 13;
				var overlayPlane = module[overlayIndex];

				// assert overlay plane identification
				Assert.AreEqual(0x601A, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
				Assert.AreEqual(13, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
				Assert.AreEqual(0x1A0000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

				// assert overlay plane encoding detection
				Assert.AreEqual(false, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
				Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
				Assert.AreEqual(true, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
				Assert.AreEqual(97*101, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

				// assert overlay plane basic attribute values
				Assert.AreEqual(97, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
				Assert.AreEqual(101, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
				Assert.AreEqual(14, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
				Assert.AreEqual(16, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
				Assert.AreEqual(OverlayType.R, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
				Assert.AreEqual(new Point(6, 7), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

				// assert overlay plane multiframe attribute values
				Assert.AreEqual(1, overlayPlane.ImageFrameOrigin.GetValueOrDefault(1), "Wrong image frame origin for plane #{0}", overlayIndex);
				Assert.AreEqual(11, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
			}
		}

		[Test]
		public void TestOverlayPlaneModuleIod_MultiframeImageSingleFrameOverlay()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int imageRows = 97;
			const int imageColumns = 101;
			const int imageFrames = 11;
			const int overlay2Rows = 67;
			const int overlay2Columns = 71;

			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[imageRows*imageColumns*imageFrames*2], imageRows, imageColumns, imageFrames, 16, 13, 12, false);
			SetOverlay(dataset, 2, new bool[overlay2Rows*overlay2Columns], OverlayType.R, new Point(4, 5), overlay2Rows, overlay2Columns, bigEndian, true);

			var module = new OverlayPlaneModuleIod(dataset);

			// test overlay group 6004

			const int overlayIndex = 2;
			var overlayPlane = module[overlayIndex];

			// assert overlay plane identification
			Assert.AreEqual(0x6004, overlayPlane.Group, "Wrong overlay group for plane #{0}", overlayIndex);
			Assert.AreEqual(2, overlayPlane.Index, "Wrong overlay index for plane #{0}", overlayIndex);
			Assert.AreEqual(0x040000, overlayPlane.TagOffset, "Wrong tag offset for plane #{0}", overlayIndex);

			// assert overlay plane encoding detection
			Assert.AreEqual(true, overlayPlane.HasOverlayData, "Incorrect OverlayData detection for plane #{0}", overlayIndex);
			Assert.AreEqual(false, overlayPlane.IsBigEndianOW, "Incorrect OW/OB detection for plane #{0}", overlayIndex);
			Assert.AreEqual(false, overlayPlane.IsEmbedded, "Incorrect embedded overlay detection for plane #{0}", overlayIndex);
			Assert.AreEqual(false, overlayPlane.IsMultiFrame, "Incorrect multiframe detection for plane #{0}", overlayIndex);
			Assert.AreEqual(67*71, overlayPlane.GetOverlayFrameLength(), "Incorrect frame size computation for plane #{0}", overlayIndex);

			// assert overlay plane basic attribute values
			Assert.AreEqual(67, overlayPlane.OverlayRows, "Wrong overlay rows for plane #{0}", overlayIndex);
			Assert.AreEqual(71, overlayPlane.OverlayColumns, "Wrong overlay columns for plane #{0}", overlayIndex);
			Assert.AreEqual(0, overlayPlane.OverlayBitPosition, "Wrong overlay bit position for plane #{0}", overlayIndex);
			Assert.AreEqual(1, overlayPlane.OverlayBitsAllocated, "Wrong overlay bits allocated for plane #{0}", overlayIndex);
			Assert.AreEqual(OverlayType.R, overlayPlane.OverlayType, "Wrong overlay type for plane #{0}", overlayIndex);
			Assert.AreEqual(new Point(4, 5), overlayPlane.OverlayOrigin, "Wrong overlay origin for plane #{0}", overlayIndex);

			// assert overlay plane multiframe attribute values
			Assert.AreEqual(null, overlayPlane.ImageFrameOrigin, "Wrong image frame origin for plane #{0}", overlayIndex);
			Assert.AreEqual(null, overlayPlane.NumberOfFramesInOverlay, "Wrong number of frames in overlay for plane #{0}", overlayIndex);
		}

		#endregion

		#region IsEmbedded Tests

		[Test]
		public void TestIsEmbedded()
		{
			// no values => not embedded
			AssertIsEmbedded(false, false, false, null, null, null, null);

			// normal embedded => embedded
			AssertIsEmbedded(true, false, true, 16, 15, 14, 15);
			AssertIsEmbedded(true, false, true, 16, 15, 15, 0);
			AssertIsEmbedded(true, false, true, 8, 6, 7, 0);
			AssertIsEmbedded(true, false, true, 8, 6, 5, 6);

			// has embedded attributes, but already has overlay data => not embedded
			AssertIsEmbedded(false, true, true, 16, 15, 14, 15);

			// missing pixel data => not embedded
			AssertIsEmbedded(false, false, false, 16, 15, 14, 15);

			// not 8 or 16 bits allocated => not embedded
			AssertIsEmbedded(false, false, true, null, 12, 11, 13);
			AssertIsEmbedded(false, false, true, 6, 12, 11, 13);

			// bit position overlaps pixel data as defined by bits stored and high bit => not embedded
			AssertIsEmbedded(false, false, true, 16, 12, 11, 11);
		}

		private static void AssertIsEmbedded(bool expectedResult,
		                                     bool hasOverlayData, bool hasPixelData, int? bitsAllocated, int? bitsStored, int? highBit, int? overlayBitPosition)
		{
			var dataset = new DicomAttributeCollection();

			if (hasOverlayData)
				dataset[DicomTags.OverlayData].Values = new byte[1];
			if (hasPixelData)
				dataset[DicomTags.PixelData].Values = new byte[1];
			if (bitsAllocated.HasValue)
				dataset[DicomTags.BitsAllocated].SetInt32(0, bitsAllocated.Value);
			if (bitsStored.HasValue)
				dataset[DicomTags.BitsStored].SetInt32(0, bitsStored.Value);
			if (highBit.HasValue)
				dataset[DicomTags.HighBit].SetInt32(0, highBit.Value);
			if (overlayBitPosition.HasValue)
				dataset[DicomTags.OverlayBitPosition].SetInt32(0, overlayBitPosition.Value);

			var plane = new OverlayPlane(0, dataset);
			Assert.AreEqual(expectedResult, plane.IsEmbedded,
			                "Embedded overlay status incorrectly detected (OD:{0},PD:{1},bA{2},bS{3},hb{4},bp{5})",
			                hasOverlayData, hasPixelData, bitsAllocated, bitsStored, highBit, overlayBitPosition);
		}

		#endregion

		#region GetRelevantOverlayFrame Tests

		[Test]
		public void TestGetRelevantOverlayFrame_MultiframeEmbedded()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[rows*columns*2*frames], rows, columns, frames, 16, 12, 11, false);
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), 12, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// all valid image frame inputs should map 1-to-1 with the same numbered overlay frame

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(1, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #1");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #1");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(2, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #2");
			Assert.AreEqual(2, actualOverlayFrame, "Wrong overlay frame matched to image frame #2");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(3, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #3");
			Assert.AreEqual(3, actualOverlayFrame, "Wrong overlay frame matched to image frame #3");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(4, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #4");
			Assert.AreEqual(4, actualOverlayFrame, "Wrong overlay frame matched to image frame #4");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(5, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #5");
			Assert.AreEqual(5, actualOverlayFrame, "Wrong overlay frame matched to image frame #5");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(6, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #6");
			Assert.AreEqual(6, actualOverlayFrame, "Wrong overlay frame matched to image frame #6");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(7, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #7");
			Assert.AreEqual(7, actualOverlayFrame, "Wrong overlay frame matched to image frame #7");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_SingleFrameEmbedded()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[rows*columns*2], rows, columns, 1, 16, 12, 11, false);
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), 12, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, 1, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, 1, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, 1, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// in the single frame case, there's only one pairing

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(1, 1, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #1");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #1");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_MultiframeOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), rows, columns, frames, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, frames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// all valid image frame inputs should map 1-to-1 with the same numbered overlay frame

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(1, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #1");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #1");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(2, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #2");
			Assert.AreEqual(2, actualOverlayFrame, "Wrong overlay frame matched to image frame #2");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(3, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #3");
			Assert.AreEqual(3, actualOverlayFrame, "Wrong overlay frame matched to image frame #3");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(4, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #4");
			Assert.AreEqual(4, actualOverlayFrame, "Wrong overlay frame matched to image frame #4");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(5, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #5");
			Assert.AreEqual(5, actualOverlayFrame, "Wrong overlay frame matched to image frame #5");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(6, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #6");
			Assert.AreEqual(6, actualOverlayFrame, "Wrong overlay frame matched to image frame #6");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(7, frames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #7");
			Assert.AreEqual(7, actualOverlayFrame, "Wrong overlay frame matched to image frame #7");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_MultiframeOverlayDataWithOrigin()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 5;

			const int imageFrames = 7;
			const int imageFrameOrigin = 2;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), rows, columns, frames, imageFrameOrigin, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(7, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #7");

			// all valid image frame inputs should map 1-to-1 with an overlay frame starting with image frame #2 and overlay frame #1

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(2, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #2");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #2");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(3, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #3");
			Assert.AreEqual(2, actualOverlayFrame, "Wrong overlay frame matched to image frame #3");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(4, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #4");
			Assert.AreEqual(3, actualOverlayFrame, "Wrong overlay frame matched to image frame #4");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(5, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #5");
			Assert.AreEqual(4, actualOverlayFrame, "Wrong overlay frame matched to image frame #5");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(6, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #6");
			Assert.AreEqual(5, actualOverlayFrame, "Wrong overlay frame matched to image frame #6");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_SingleMultiframeOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int imageFrames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), rows, columns, 1, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(2, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #2");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(5, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #5");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// the only valid mapping is for the image frame identified by the origin

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(1, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #1");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #1");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_SingleMultiframeOverlayDataWithOrigin()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int imageFrames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), rows, columns, 1, 5, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(2, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #2");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// the only valid mapping is for the image frame identified by the origin

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(5, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #5");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #5");
		}

		[Test]
		public void TestGetRelevantOverlayFrame_SingleFrameOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int imageFrames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), rows, columns, null, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualOverlayFrame;

			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(-1, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #-1");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(0, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #0");
			Assert.IsFalse(overlayPlane.TryGetRelevantOverlayFrame(8, imageFrames, out actualOverlayFrame), "Should not any matching overlay frame for image frame #8");

			// in the single frame case, all valid image frame inputs should map to overlay frame #1

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(1, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #1");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #1");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(2, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #2");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #2");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(3, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #3");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #3");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(4, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #4");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #4");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(5, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #5");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #5");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(6, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #6");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #6");

			Assert.IsTrue(overlayPlane.TryGetRelevantOverlayFrame(7, imageFrames, out actualOverlayFrame), "Should be able to match an overlay frame to image frame #7");
			Assert.AreEqual(1, actualOverlayFrame, "Wrong overlay frame matched to image frame #7");
		}

		#endregion

		#region ComputeOverlayDataBitOffset Tests

		[Test]
		public void TestComputeOverlayDataBitOffset_MultiframeOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), rows, columns, frames, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;

			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(8, out actualBitOffset), "Should not be able to compute bit offset for frame number 8");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should be able to compute bit offset for frame number 1");
			Assert.AreEqual(0*rows*columns, actualBitOffset, "Wrong offset computed for frame number 1");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should be able to compute bit offset for frame number 2");
			Assert.AreEqual(1*rows*columns, actualBitOffset, "Wrong offset computed for frame number 2");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(3, out actualBitOffset), "Should be able to compute bit offset for frame number 3");
			Assert.AreEqual(2*rows*columns, actualBitOffset, "Wrong offset computed for frame number 3");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(4, out actualBitOffset), "Should be able to compute bit offset for frame number 4");
			Assert.AreEqual(3*rows*columns, actualBitOffset, "Wrong offset computed for frame number 4");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(5, out actualBitOffset), "Should be able to compute bit offset for frame number 5");
			Assert.AreEqual(4*rows*columns, actualBitOffset, "Wrong offset computed for frame number 5");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(6, out actualBitOffset), "Should be able to compute bit offset for frame number 6");
			Assert.AreEqual(5*rows*columns, actualBitOffset, "Wrong offset computed for frame number 6");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(7, out actualBitOffset), "Should be able to compute bit offset for frame number 7");
			Assert.AreEqual(6*rows*columns, actualBitOffset, "Wrong offset computed for frame number 7");
		}

		[Test]
		public void TestComputeOverlayDataBitOffset_MultiframeOverlayDataWithOrigin()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 7;

			// just because you're getting overlay frame #2 for image frame #4 doesn't change the offset where overlay frame #2 can be found!!
			const int imageFrameOrigin = 3;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), rows, columns, frames, imageFrameOrigin, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;

			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(8, out actualBitOffset), "Should not be able to compute bit offset for frame number 8");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should be able to compute bit offset for frame number 1");
			Assert.AreEqual(0*rows*columns, actualBitOffset, "Wrong offset computed for frame number 1");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should be able to compute bit offset for frame number 2");
			Assert.AreEqual(1*rows*columns, actualBitOffset, "Wrong offset computed for frame number 2");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(3, out actualBitOffset), "Should be able to compute bit offset for frame number 3");
			Assert.AreEqual(2*rows*columns, actualBitOffset, "Wrong offset computed for frame number 3");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(4, out actualBitOffset), "Should be able to compute bit offset for frame number 4");
			Assert.AreEqual(3*rows*columns, actualBitOffset, "Wrong offset computed for frame number 4");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(5, out actualBitOffset), "Should be able to compute bit offset for frame number 5");
			Assert.AreEqual(4*rows*columns, actualBitOffset, "Wrong offset computed for frame number 5");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(6, out actualBitOffset), "Should be able to compute bit offset for frame number 6");
			Assert.AreEqual(5*rows*columns, actualBitOffset, "Wrong offset computed for frame number 6");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(7, out actualBitOffset), "Should be able to compute bit offset for frame number 7");
			Assert.AreEqual(6*rows*columns, actualBitOffset, "Wrong offset computed for frame number 7");
		}

		[Test]
		public void TestComputeOverlayDataBitOffset_SingleMultiframeOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), rows, columns, 1, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should not be able to compute bit offset for frame number 2");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should be able to compute bit offset for frame number 1");
			Assert.AreEqual(0, actualBitOffset, "Wrong offset computed for frame number 1");
		}

		[Test]
		public void TestComputeOverlayDataBitOffset_SingleFrameOverlayData()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), rows, columns, null, null, bigEndian, false);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should not be able to compute bit offset for frame number 2");

			Assert.IsTrue(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should be able to compute bit offset for frame number 1");
			Assert.AreEqual(0, actualBitOffset, "Wrong offset computed for frame number 1");
		}

		[Test]
		public void TestComputeOverlayDataBitOffset_MultiframeEmbedded()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;
			const int frames = 7;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[rows*columns*frames*2], rows, columns, frames, 16, 12, 14, false);
			SetOverlay(dataset, overlayIndex, new bool[rows*columns*frames], OverlayType.G, new Point(1, 1), 15, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;

			// bit offset doesn't make any sense for embedded data

			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should not be able to compute bit offset for frame number 1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should not be able to compute bit offset for frame number 2");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(8, out actualBitOffset), "Should not be able to compute bit offset for frame number 8");
		}

		[Test]
		public void TestComputeOverlayDataBitOffset_SingleFrameEmbedded()
		{
			const bool bigEndian = false;

			// these parameters should be kept prime numbers so that we can exercise the overlay handling for rows/frames that cross byte boundaries
			const int rows = 97;
			const int columns = 101;

			const int overlayIndex = 0;
			var dataset = new DicomAttributeCollection();
			SetImage(dataset, new byte[rows*columns*2], rows, columns, 1, 16, 12, 14, false);
			SetOverlay(dataset, overlayIndex, new bool[rows*columns], OverlayType.G, new Point(1, 1), 15, bigEndian);

			var module = new OverlayPlaneModuleIod(dataset);
			var overlayPlane = module[overlayIndex];

			int actualBitOffset;

			// bit offset doesn't make any sense for embedded data

			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(-1, out actualBitOffset), "Should not be able to compute bit offset for frame number -1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(0, out actualBitOffset), "Should not be able to compute bit offset for frame number 0");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(1, out actualBitOffset), "Should not be able to compute bit offset for frame number 1");
			Assert.IsFalse(overlayPlane.TryComputeOverlayDataBitOffset(2, out actualBitOffset), "Should not be able to compute bit offset for frame number 2");
		}

		#endregion

		#region Utility Helpers

		private static void SetImage(IDicomAttributeProvider dataset, byte[] pixelData, int rows, int columns, int frames, int bitsAllocated, int bitsStored, int highBit, bool isSigned)
		{
			DicomOverlayTestHelper.SetImagePixels(dataset, pixelData, rows, columns, frames, bitsAllocated, bitsStored, highBit, isSigned);
		}

		private static void SetOverlay(IDicomAttributeProvider dataset, int overlayIndex, bool[] overlayData, OverlayType type, Point origin, int rows, int columns, bool bigEndian, bool useOW)
		{
			DicomOverlayTestHelper.AddOverlayPlane(dataset, overlayIndex, overlayData, type, origin, rows, columns, bigEndian, useOW);
		}

		private static void SetOverlay(IDicomAttributeProvider dataset, int overlayIndex, bool[] overlayData, OverlayType type, Point origin, int rows, int columns, int? frames, bool bigEndian, bool useOW)
		{
			DicomOverlayTestHelper.AddOverlayPlane(dataset, overlayIndex, overlayData, type, origin, rows, columns, frames, bigEndian, useOW);
		}

		private static void SetOverlay(IDicomAttributeProvider dataset, int overlayIndex, bool[] overlayData, OverlayType type, Point origin, int rows, int columns, int? frames, int? frameOrigin, bool bigEndian, bool useOW)
		{
			DicomOverlayTestHelper.AddOverlayPlane(dataset, overlayIndex, overlayData, type, origin, rows, columns, frames, frameOrigin, bigEndian, useOW);
		}

		private static void SetOverlay(IDicomAttributeProvider dataset, int overlayIndex, bool[] overlayData, OverlayType type, Point origin, int bitPosition, bool bigEndian)
		{
			DicomOverlayTestHelper.AddOverlayPlane(dataset, overlayIndex, overlayData, type, origin, bitPosition, bigEndian);
		}

		#endregion
	}
}

#endif