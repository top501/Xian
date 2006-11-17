using System;
using System.Drawing;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Layers;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.InputManagement;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
    [MenuAction("activate", "imageviewer-contextmenu/MenuToolsStandardPan", Flags = ClickActionFlags.CheckAction)]
    [MenuAction("activate", "global-menus/MenuTools/Standard/MenuToolsStandardPan", Flags = ClickActionFlags.CheckAction)]
    [ButtonAction("activate", "global-toolbars/ToolbarStandard/ToolbarToolsStandardPan", Flags = ClickActionFlags.CheckAction)]
	[KeyboardAction("activate", "imageviewer-keyboard/ToolsStandardPan/Activate", KeyStroke = XKeys.P)]
    [CheckedStateObserver("activate", "Active", "ActivationChanged")]
    [ClickHandler("activate", "Select")]
    [Tooltip("activate", "ToolbarToolsStandardPan")]
	[IconSet("activate", IconScheme.Colour, "", "Icons.PanMedium.png", "Icons.PanLarge.png")]

	[KeyboardAction("panleft", "imageviewer-keyboard/ToolsStandardPan/PanLeft", KeyStroke = XKeys.Control | XKeys.Left)]
	[ClickHandler("panleft", "PanLeft")]

	[KeyboardAction("panright", "imageviewer-keyboard/ToolsStandardPan/PanRight", KeyStroke = XKeys.Control | XKeys.Right)]
	[ClickHandler("panright", "PanRight")]

	[KeyboardAction("panup", "imageviewer-keyboard/ToolsStandardPan/PanUp", KeyStroke = XKeys.Control | XKeys.Up)]
	[ClickHandler("panup", "PanUp")]

	[KeyboardAction("pandown", "imageviewer-keyboard/ToolsStandardPan/PanDown", KeyStroke = XKeys.Control | XKeys.Down)]
	[ClickHandler("pandown", "PanDown")]

	[MouseButtonControl(XMouseButtons.Left, ModifierFlags.Control)]

	[CursorToken("Icons.PanMedium.png", typeof(PanTool))]	
	[MouseToolButton(XMouseButtons.Left, false)]
    
	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
	public class PanTool : MouseTool
	{
		private UndoableCommand _command;
		private SpatialTransformApplicator _applicator;

		public PanTool()
		{
		}

		private void CaptureBeginState(IPresentationImage image)
		{
			_applicator = new SpatialTransformApplicator(image);
			_command = new UndoableCommand(_applicator);
			_command.Name = SR.CommandPan;
			_command.BeginState = _applicator.CreateMemento();
		}

		private void CaptureEndState()
		{
			if (_command == null)
				return;

			_command.EndState = _applicator.CreateMemento();

			// If the state hasn't changed since MouseDown just return
			if (_command.EndState.Equals(_command.BeginState))
			{
				_command = null;
				return;
			}

			// Apply the final state to all linked images
			_applicator.SetMemento(_command.EndState);

			this.Context.Viewer.CommandHistory.AddCommand(_command);
		}

		private void PanLeft()
		{
			IncrementPan(-20, 0);
		}

		private void PanRight()
		{
			IncrementPan(20, 0);
		}

		private void PanUp()
		{
			IncrementPan(0, -20);
		}

		private void PanDown()
		{
			IncrementPan(0, 20);
		}

		private void IncrementPan(int xIncrement, int yIncrement)
		{
			IPresentationImage image = this.Context.Viewer.SelectedPresentationImage;

			if (image == null)
				return;

			if (image.LayerManager.SelectedImageLayer == null)
				return;

			this.CaptureBeginState(image);
			this.IncrementPan(image, xIncrement, yIncrement);
			this.CaptureEndState();
		}

		private void IncrementPan(IPresentationImage image, int xIncrement, int yIncrement)
		{
			SpatialTransform spatialTransform = image.LayerManager.SelectedLayerGroup.SpatialTransform;
			float scale = spatialTransform.Scale;
			Platform.CheckPositive(scale, "spatialTransform.Scale");

			spatialTransform.TranslationX += xIncrement / scale;
			spatialTransform.TranslationY += yIncrement / scale;
			spatialTransform.Calculate();
			image.Draw();
		}

		public override bool Start(MouseInformation mouseInformation)
		{
			base.Start(mouseInformation);

			if (!IsImageValid(mouseInformation.Tile.PresentationImage))
				return true;

			CaptureBeginState(mouseInformation.Tile.PresentationImage);

			return true;
		}

		public override bool Track(MouseInformation mouseInformation)
		{
			base.Track(mouseInformation);

			if (!IsImageValid(mouseInformation.Tile.PresentationImage))
				return true;

			if (_command == null)
				return true;

			this.IncrementPan(mouseInformation.Tile.PresentationImage, base.DeltaX, base.DeltaY);

			return true;
		}

		public override bool Stop(MouseInformation mouseInformation)
		{
			base.Stop(mouseInformation);

			if (!IsImageValid(mouseInformation.Tile.PresentationImage))
				return true;

			CaptureEndState();

			return true;
		}
	}
}
