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
using ClearCanvas.Dicom.Iod.Sequences;
using ClearCanvas.ImageViewer.Graphics;
using ClearCanvas.ImageViewer.Mathematics;

namespace ClearCanvas.ImageViewer.PresentationStates.Dicom.GraphicAnnotationSerializers
{
	internal class EllipseGraphicAnnotationSerializer : GraphicAnnotationSerializer<IBoundableGraphic>
	{
		protected override void Serialize(IBoundableGraphic graphic, GraphicAnnotationSequenceItem serializationState)
		{
			if (!graphic.Visible)
				return; // if the graphic is not visible, don't serialize it!

			GraphicAnnotationSequenceItem.GraphicObjectSequenceItem annotationElement = new GraphicAnnotationSequenceItem.GraphicObjectSequenceItem();

			graphic.CoordinateSystem = CoordinateSystem.Source;
			try
			{
				annotationElement.GraphicAnnotationUnits = GraphicAnnotationSequenceItem.GraphicAnnotationUnits.Pixel;
				annotationElement.GraphicDimensions = 2;
				annotationElement.GraphicFilled = GraphicAnnotationSequenceItem.GraphicFilled.N;

				SizeF halfDims = new SizeF(graphic.Width/2, graphic.Height/2);
				if (FloatComparer.AreEqual(graphic.Width, graphic.Height)) // check if graphic is a circle
				{
					annotationElement.GraphicType = GraphicAnnotationSequenceItem.GraphicType.Circle;
					annotationElement.NumberOfGraphicPoints = 2;

					PointF[] list = new PointF[2];
					list[0] = graphic.TopLeft + halfDims; // centre of circle
					list[1] = graphic.TopLeft + new SizeF(0, halfDims.Height); // any point on the circle
					annotationElement.GraphicData = list;
				}
				else
				{
					annotationElement.GraphicType = GraphicAnnotationSequenceItem.GraphicType.Ellipse;
					annotationElement.NumberOfGraphicPoints = 4;

					int offset = graphic.Width < graphic.Height ? 2 : 0; // offset list by 2 if major axis is vertical
					PointF[] list = new PointF[4];
					list[(offset + 0)%4] = graphic.TopLeft + new SizeF(0, halfDims.Height); // left point of horizontal axis
					list[(offset + 1)%4] = graphic.TopLeft + new SizeF(graphic.Width, halfDims.Height); // right point of horizontal axis
					list[(offset + 2)%4] = graphic.TopLeft + new SizeF(halfDims.Width, 0); // top point of vertical axis
					list[(offset + 3)%4] = graphic.TopLeft + new SizeF(halfDims.Width, graphic.Height); // bottom point of vertical axis
					annotationElement.GraphicData = list;
				}
			}
			finally
			{
				graphic.ResetCoordinateSystem();
			}

			serializationState.AppendGraphicObjectSequence(annotationElement);
		}
	}
}