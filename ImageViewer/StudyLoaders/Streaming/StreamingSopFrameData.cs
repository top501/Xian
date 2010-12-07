#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.Iod.Modules;
using ClearCanvas.Dicom.ServiceModel.Streaming;
using ClearCanvas.ImageViewer.Common;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.ImageViewer.StudyManagement;
using System.Threading;

namespace ClearCanvas.ImageViewer.StudyLoaders.Streaming
{
	public class StreamingPerformanceInfo
	{
		internal StreamingPerformanceInfo(DateTime start, DateTime end, long totalBytesTransferred)
		{
			StartTime = start;
			EndTime = end;
			TotalBytesTransferred = totalBytesTransferred;
		}

		public readonly DateTime StartTime;
		public readonly DateTime EndTime;
		public readonly long TotalBytesTransferred;

		public TimeSpan ElapsedTime
		{
			get { return EndTime.Subtract(StartTime); }	
		}
	}

	public interface IStreamingSopFrameData : ISopFrameData
	{
		StreamingPerformanceInfo LastRetrievePerformanceInfo { get; } 
		bool PixelDataRetrieved { get; }
		void RetrievePixelData();
	}

	internal partial class StreamingSopDataSource
	{
		private class StreamingSopFrameData : StandardSopFrameData, IStreamingSopFrameData
		{
			private readonly FramePixelData _framePixelData;
			private readonly byte[][] _overlayData;

			public StreamingSopFrameData(int frameNumber, StreamingSopDataSource parent) 
				: base(frameNumber, parent, RegenerationCost.High)
			{
				_framePixelData = new FramePixelData(this.Parent, frameNumber);
				_overlayData = new byte[16][];
			}

			public new StreamingSopDataSource Parent
			{
				get { return (StreamingSopDataSource) base.Parent; }
			}

			public bool PixelDataRetrieved
			{
				get { return _framePixelData.AlreadyRetrieved; }
			}

			public StreamingPerformanceInfo LastRetrievePerformanceInfo
			{
				get { return _framePixelData.LastRetrievePerformanceInfo; }	
			}

			public void RetrievePixelData()
			{
				_framePixelData.Retrieve();
			}

			protected override byte[] CreateNormalizedPixelData()
			{
				byte[] pixelData = _framePixelData.GetUncompressedPixelData();

				string photometricInterpretationCode = this.Parent[DicomTags.PhotometricInterpretation].ToString();
				PhotometricInterpretation pi = PhotometricInterpretation.FromCodeString(photometricInterpretationCode);

				TransferSyntax ts = TransferSyntax.GetTransferSyntax(this.Parent.TransferSyntaxUid);
				if (pi.IsColor)
				{
					if (ts == TransferSyntax.Jpeg2000ImageCompression ||
					    ts == TransferSyntax.Jpeg2000ImageCompressionLosslessOnly ||
					    ts == TransferSyntax.JpegExtendedProcess24 ||
					    ts == TransferSyntax.JpegBaselineProcess1)
						pi = PhotometricInterpretation.Rgb;

					pixelData = ToArgb(this.Parent, pixelData, pi);
				}
				else
				{
					var overlayPlaneModuleIod = new OverlayPlaneModuleIod(Parent);
					foreach (var overlayPlane in overlayPlaneModuleIod)
					{
						if (IsOverlayEmbedded(overlayPlane) && _overlayData[overlayPlane.Index] == null)
						{
							// if the overlay is embedded in pixel data and we haven't cached it yet, extract it now before we normalize the frame pixel data
							var overlayData = OverlayData.UnpackFromPixelData(overlayPlane.OverlayBitPosition, Parent[DicomTags.BitsAllocated].GetInt32(0, 0), false, pixelData);
							_overlayData[overlayPlane.Index] = overlayData;
						}
						else if (!overlayPlane.HasOverlayData)
						{
							// if the overlay is not embedded and OverlayData appears to be missing, log a warning now
							Platform.Log(LogLevel.Warn, "The image {0} appears to be missing OverlayData for group 0x{1:X4}.", Parent.SopInstanceUid, overlayPlane.Group);
						}
					}

					NormalizeGrayscalePixels(this.Parent, pixelData);
				}

				return pixelData;
			}

			/// <summary>
			/// Called by <see cref="StandardSopFrameData.GetNormalizedOverlayData"/> to create a new byte buffer containing normalized 
			/// overlay pixel data for a particular overlay plane.
			/// </summary>
			/// <remarks>
			/// See <see cref="StandardSopFrameData.GetNormalizedOverlayData"/> for details on the expected format of the byte buffer.
			/// </remarks>
			/// <param name="overlayNumber">The 1-based overlay plane number.</param>
			/// <returns>A new byte buffer containing the normalized overlay pixel data.</returns>
			protected override byte[] CreateNormalizedOverlayData(int overlayNumber)
			{
				var overlayIndex = overlayNumber - 1;

				byte[] overlayData = null;

				// check whether or not the overlay plane exists before attempting to ascertain
				// whether or not the overlay is embedded in the pixel data
				var overlayPlaneModuleIod = new OverlayPlaneModuleIod(Parent);
				if (overlayPlaneModuleIod.HasOverlayPlane(overlayIndex))
				{
					if (_overlayData[overlayIndex] == null)
					{
						var overlayPlane = overlayPlaneModuleIod[overlayIndex];
						if (IsOverlayEmbedded(overlayPlane))
						{
							// if the overlay is embedded, trigger retrieval of pixel data which will populate the cache for us
							GetNormalizedPixelData();
						}
						else
						{
							// try to compute the offset in the OverlayData bit stream where we can find the overlay frame that applies to this image frame
							int overlayFrame;
							int bitOffset;
							if (overlayPlane.TryGetRelevantOverlayFrame(FrameNumber, Parent.NumberOfFrames, out overlayFrame) &&
							    overlayPlane.TryComputeOverlayDataBitOffset(overlayFrame, out bitOffset))
							{
								// offset found - unpack only that overlay frame
								var od = new OverlayData(bitOffset,
								                         overlayPlane.OverlayRows,
								                         overlayPlane.OverlayColumns,
								                         overlayPlane.IsBigEndianOW,
								                         overlayPlane.OverlayData);
								_overlayData[overlayIndex] = od.Unpack();
							}
							else
							{
								// no relevant overlay frame found - i.e. the overlay for this image frame is blank
								_overlayData[overlayIndex] = new byte[overlayPlane.GetOverlayFrameLength()];
							}
						}
					}

					overlayData = _overlayData[overlayIndex];
				}

				return overlayData;
			}

			protected override void OnUnloaded()
			{
				base.OnUnloaded();

				// dump pixel data retrieve results and stored overlays
				_framePixelData.Unload();
				_overlayData[0x0] = null;
				_overlayData[0x1] = null;
				_overlayData[0x2] = null;
				_overlayData[0x3] = null;
				_overlayData[0x4] = null;
				_overlayData[0x5] = null;
				_overlayData[0x6] = null;
				_overlayData[0x7] = null;
				_overlayData[0x8] = null;
				_overlayData[0x9] = null;
				_overlayData[0xA] = null;
				_overlayData[0xB] = null;
				_overlayData[0xC] = null;
				_overlayData[0xD] = null;
				_overlayData[0xE] = null;
				_overlayData[0xF] = null;
			}
		}

		private class FramePixelDataRetriever
		{
			public readonly string StudyInstanceUid;
			public readonly string SeriesInstanceUid;
			public readonly string SopInstanceUid;
			public readonly int FrameNumber;
			public readonly string TransferSyntaxUid;
			public readonly string AETitle;
			public readonly Uri BaseUrl;

			public FramePixelDataRetriever(FramePixelData source)
			{
				string host = source.Parent._host;
				string wadoPrefix = source.Parent._wadoUriPrefix;
				int wadoPort = source.Parent._wadoServicePort;

				try
				{
					BaseUrl = new Uri(String.Format(wadoPrefix, host, wadoPort));
				}
				catch (FormatException ex)
				{
					// this exception happens if the FormatWadoUriPrefix setting is invalid.
					throw new Exception(SR.MessageStreamingClientConfigurationException, ex);
				}

				AETitle = source.Parent._aeTitle;

				StudyInstanceUid = source.Parent.StudyInstanceUid;
				SeriesInstanceUid = source.Parent.SeriesInstanceUid;
				SopInstanceUid = source.Parent.SopInstanceUid;
				FrameNumber = source.FrameNumber;
				TransferSyntaxUid = source.Parent.TransferSyntaxUid;
			}

			public RetrievePixelDataResult Retrieve()
			{
				Exception retrieveException;
				RetrievePixelDataResult result = TryClientRetrievePixelData(out retrieveException);

				if (result != null)
					return result;

				// if no result was returned, then the throw an exception with an appropriate, user-friendly message
				throw TranslateStreamingException(retrieveException);
			}

			private RetrievePixelDataResult TryClientRetrievePixelData(out Exception lastRetrieveException)
			{
				// retry parameters
				const int retryTimeout = 1500;
				int retryDelay = 50;
				int retryCounter = 0;
				
				StreamingClient client = new StreamingClient(this.BaseUrl);
				RetrievePixelDataResult result = null;
				lastRetrieveException = null;

				CodeClock timeoutClock = new CodeClock();
				timeoutClock.Start();

				while (true)
				{
					try
					{
						if (retryCounter > 0)
							Platform.Log(LogLevel.Info, "Retrying retrieve pixel data for Sop '{0}' (Attempt #{1})", this.SopInstanceUid, retryCounter);

						CodeClock statsClock = new CodeClock();
						statsClock.Start();

						result = client.RetrievePixelData(this.AETitle, this.StudyInstanceUid, this.SeriesInstanceUid, this.SopInstanceUid, this.FrameNumber - 1);

						statsClock.Stop();

						Platform.Log(LogLevel.Debug, "[Retrieve Info] Sop/Frame: {0}/{1}, Transfer Syntax: {2}, Bytes transferred: {3}, Elapsed (s): {4}, Retries: {5}",
						             this.SopInstanceUid, this.FrameNumber, this.TransferSyntaxUid,
						             result.MetaData.ContentLength, statsClock.Seconds, retryCounter);

						break;
					}
					catch (Exception ex)
					{
						lastRetrieveException = ex;

						timeoutClock.Stop();
						if (timeoutClock.Seconds*1000 >= retryTimeout)
						{
							// log an alert that we are aborting (exception trace at debug level only)
							int elapsed = (int)(1000*timeoutClock.Seconds);
							Platform.Log(LogLevel.Warn, "Failed to retrieve pixel data for Sop '{0}'; Aborting after {1} attempts in {2} ms", this.SopInstanceUid, retryCounter, elapsed);
							Platform.Log(LogLevel.Debug, ex, "[Retrieve Fail-Abort] Sop/Frame: {0}/{1}, Retry Attempts: {2}, Elapsed: {3} ms", this.SopInstanceUid, this.FrameNumber - 1, retryCounter, elapsed);
							break;
						}
						timeoutClock.Start();

						retryCounter++;

						// log the retry (exception trace at debug level only)
						Platform.Log(LogLevel.Warn, "Failed to retrieve pixel data for Sop '{0}'; Retrying in {1} ms", this.SopInstanceUid, retryDelay);
						Platform.Log(LogLevel.Debug, ex, "[Retrieve Fail-Retry] Sop/Frame: {0}/{1}, Retry in: {2} ms", this.SopInstanceUid, this.FrameNumber - 1, retryDelay);
						MemoryManager.Collect(retryDelay);
						retryDelay *= 2;
					}
				}

				return result;
			}
		}
		
		private class FramePixelData
		{
			private readonly object _syncLock = new object();
			private volatile bool _alreadyRetrieved;
			private RetrievePixelDataResult _retrieveResult;
			private volatile StreamingPerformanceInfo _lastRetrievePerformanceInfo;

			public readonly StreamingSopDataSource Parent;
			public readonly int FrameNumber;

			public FramePixelData(StreamingSopDataSource parent, int frameNumber)
			{
				Parent = parent;
				FrameNumber = frameNumber;
			}

			public bool AlreadyRetrieved
			{
				get { return _alreadyRetrieved; }
			}

			public StreamingPerformanceInfo LastRetrievePerformanceInfo
			{
				get { return _lastRetrievePerformanceInfo; }
			}

			public void Retrieve()
			{
				if (!_alreadyRetrieved)
				{
					//construct this object before the lock so there's no chance of deadlocking
					//with the parent data source (because we are accessing it's tags at the 
					//same time as it's trying to get the pixel data).
					FramePixelDataRetriever retriever = new FramePixelDataRetriever(this);

					lock (_syncLock)
					{
						if (!_alreadyRetrieved)
						{
							//TODO (Time Review): use Environment.TickCount
							DateTime start = DateTime.Now;
							_retrieveResult = retriever.Retrieve();
							DateTime end = DateTime.Now;

							_lastRetrievePerformanceInfo = 
								new StreamingPerformanceInfo(start, end, _retrieveResult.MetaData.ContentLength);
							
							_alreadyRetrieved = true;
						}
					}
				}
			}

			public byte[] GetUncompressedPixelData()
			{
				//construct this object before the lock so there's no chance of deadlocking
				//with the parent data source (because we are accessing it's tags at the 
				//same time as it's trying to get the pixel data).
				FramePixelDataRetriever retriever = new FramePixelDataRetriever(this);

				lock (_syncLock)
				{
					RetrievePixelDataResult result;
					if (_retrieveResult == null)
						result = retriever.Retrieve();
					else
						result = _retrieveResult;

					//free this memory up in case it's holding a compressed buffer.
					_retrieveResult = null;

					CodeClock clock = new CodeClock();
					clock.Start();

					//synchronize the call to decompress; it's really already synchronized by
					//the parent b/c it's only called from CreateFrameNormalizedPixelData, but it doesn't hurt.
					byte[] pixelData = result.GetPixelData();

					clock.Stop();

					Platform.Log(LogLevel.Debug, "[Decompress Info] Sop/Frame: {0}/{1}, Transfer Syntax: {2}, Uncompressed bytes: {3}, Elapsed (s): {4}",
												   retriever.SopInstanceUid, FrameNumber, retriever.TransferSyntaxUid,
												   pixelData.Length, clock.Seconds);

					return pixelData;
				}
			}

			public void Unload()
			{
				lock (_syncLock)
				{
					_alreadyRetrieved = false;
					_retrieveResult = null;
				}
			}
		}

		/// <summary>
		/// Determines if the overlay data for this plane is embedded in the pixel data.
		/// </summary>
		/// <remarks>
		/// We cannot use <see cref="OverlayPlane.IsEmbedded"/> because the PixelData attribute is not in the dataset since we stream it separately.
		/// </remarks>
		internal static bool IsOverlayEmbedded(OverlayPlane overlayPlane)
		{
			IDicomAttributeProvider provider = overlayPlane.DicomAttributeProvider;

			// OverlayData exists => not embedded
			DicomAttribute overlayData = provider[overlayPlane.TagOffset + DicomTags.OverlayData];
			if (overlayData.IsEmpty || overlayData.IsNull)
			{
				// embedded => BitsAllocated={8|16}, OverlayBitPosition in [0, BitsAllocated)
				int overlayBitPosition = provider[overlayPlane.TagOffset + DicomTags.OverlayBitPosition].GetInt32(0, -1);
				int bitsAllocated = provider[DicomTags.BitsAllocated].GetInt32(0, 0);
				if (overlayBitPosition >= 0 && overlayBitPosition < bitsAllocated && (bitsAllocated == 8 || bitsAllocated == 16))
				{
					// embedded => OverlayBitPosition in (HighBit, BitsAllocated) or [0, HighBit - BitsStored + 1)
					int highBit = provider[DicomTags.HighBit].GetInt32(0, 0);
					int bitsStored = provider[DicomTags.BitsStored].GetInt32(0, 0);
					return (overlayBitPosition > highBit || overlayBitPosition < highBit - bitsStored + 1);
				}
			}
			return false;
		}
	}
}