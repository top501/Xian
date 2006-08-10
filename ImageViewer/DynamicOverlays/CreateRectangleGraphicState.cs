using System;
using System.Diagnostics;
using System.Drawing;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.DynamicOverlays
{
	public class CreateRectangleGraphicState : GraphicState
	{
		private int _ControlPointIndex;
		private int _NumberOfPointsAnchored = 1;

		// Create a graphic object
		public CreateRectangleGraphicState(InteractiveRectangleGraphic interactiveGraphic) 
			: base(interactiveGraphic)
		{
			// This control point index corresponds to the bottom right control point
			_ControlPointIndex = 3;
		}

		private InteractiveGraphic InteractiveGraphic
		{
			get { return base.StatefulGraphic as InteractiveGraphic; }
		}

		public override bool OnMouseDown(XMouseEventArgs e)
		{
			Platform.CheckForNullReference(e, "e");

			// We just started creating
			if (_NumberOfPointsAnchored == 1)
			{
				PointF mousePoint = this.InteractiveGraphic.SpatialTransform.ConvertToSource(new PointF(e.X, e.Y));
				this.InteractiveGraphic.CoordinateSystem = CoordinateSystem.Source;
				this.InteractiveGraphic.ControlPoints[0] = mousePoint;
				this.InteractiveGraphic.ControlPoints[3] = mousePoint;
				this.InteractiveGraphic.ResetCoordinateSystem();

				_NumberOfPointsAnchored++;
			}
			// We're done creating
			else
			{
				if (this.SupportUndo)
				{
					base.Command = new PositionGraphicCommand(this.InteractiveGraphic, true);
					base.Command.Name = SR.CommandCreateRectangleGraphic;
					this.InteractiveGraphic.ParentWorkspace.CommandHistory.AddCommand(base.Command);
				}

				base.StatefulGraphic.State = base.StatefulGraphic.CreateSelectedState();
			}

			return true;
		}

		public override bool OnMouseMove(XMouseEventArgs e)
		{
			Platform.CheckForNullReference(e, "e");

			PointF mousePoint = this.InteractiveGraphic.SpatialTransform.ConvertToSource(new PointF(e.X, e.Y));
			this.InteractiveGraphic.CoordinateSystem = CoordinateSystem.Source;
			this.InteractiveGraphic.ControlPoints[_ControlPointIndex] = mousePoint;
			this.InteractiveGraphic.ResetCoordinateSystem();
			this.InteractiveGraphic.Draw();

			return true;
		}

		public override string ToString()
		{
			return "CreateRectangleGraphicState\n";
		}
	}
}
