﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System.Drawing;
using System.Drawing.Imaging;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Graphics;
using NUnit.Framework;

#if	UNIT_TESTS

#pragma warning disable 1591,0419,1574,1587


namespace ClearCanvas.ImageViewer.Rendering.Tests
{
	static class ImageRendererTestUtilities
	{
		public static Bitmap RenderLayer(ImageGraphic layer, int dstWidth, int dstHeight)
		{
			Bitmap bitmap = new Bitmap(dstWidth, dstHeight);
			Rectangle clientArea = new Rectangle(0, 0, dstWidth, dstHeight);

			BitmapData bitmapData = LockBitmap(bitmap);
			int bytesPerPixel = 4;
			ImageRenderer.Render(layer, bitmapData.Scan0, bitmapData.Width, bytesPerPixel, clientArea);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		public static void VerifyMonochromePixelValue16(int x, int y, int expectedPixelValue16, Bitmap bitmap)
		{
			int expectedPixelValue8 = expectedPixelValue16 / 256;

			VerifyMonochromePixelValue8(x, y, expectedPixelValue8, bitmap);
		}

		public static void VerifyMonochromePixelValue8(int x, int y, int expectedPixelValue8, Bitmap bitmap)
		{
			Color expectedPixelColor = Color.FromArgb(expectedPixelValue8, expectedPixelValue8, expectedPixelValue8);

			VerifyRGBPixelValue(x, y, expectedPixelColor, bitmap);
		}

		public static void VerifyRGBPixelValue(int x, int y, Color expectedPixelColor, Bitmap bitmap)
		{
			Color actualPixelColor = bitmap.GetPixel(x, y);
			Assert.AreEqual(expectedPixelColor, actualPixelColor);
		}

		private static BitmapData LockBitmap(Bitmap bitmap)
		{
			BitmapData bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadWrite,
				bitmap.PixelFormat);

			return bitmapData;
		}
	}
}

#endif