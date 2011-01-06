#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Imaging;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// Defines a factory for getting a VOI LUT appropriate for an <see cref="ImageGraphic"/>.
	/// </summary>
	public interface IGraphicVoiLutFactory
	{
		/// <summary>
		/// Creates a Voi LUT suitable for the given <paramref name="imageGraphic"/>.
		/// </summary>
		/// <returns>The VOI LUT as an <see cref="IComposableLut"/>.</returns>
		IComposableLut CreateVoiLut(ImageGraphic imageGraphic);
	}

	/// <summary>
	/// A base class defines a factory for getting a VOI LUT appropriate for an <see cref="ImageGraphic"/>.
	/// </summary>
	public abstract class GraphicVoiLutFactory : IGraphicVoiLutFactory
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		protected GraphicVoiLutFactory() {}

		/// <summary>
		/// Creates a Voi LUT suitable for the given <paramref name="imageGraphic"/>.
		/// </summary>
		/// <returns>The VOI LUT as an <see cref="IComposableLut"/>.</returns>
		public abstract IComposableLut CreateVoiLut(ImageGraphic imageGraphic);

		/// <summary>
		/// Defines the method for creating a Voi LUT suitable for the given <paramref name="imageGraphic"/>.
		/// </summary>
		/// <returns>The VOI LUT as an <see cref="IComposableLut"/>.</returns>
		public delegate IComposableLut CreateVoiLutDelegate(ImageGraphic imageGraphic);

		/// <summary>
		/// Creates a new factory that wraps the given delegate.
		/// </summary>
		/// <param name="createVoiLutDelegate">A <see cref="CreateVoiLutDelegate"/> delegate to
		/// get a VOI LUT appropriate for the given <see cref="ImageGraphic"/>.
		/// This method should generally be static, as the factory may only be reference-copied when the parent graphic is cloned.</param>
		/// <returns>The VOI LUT as an <see cref="IComposableLut"/>.</returns>
		public static GraphicVoiLutFactory Create(CreateVoiLutDelegate createVoiLutDelegate)
		{
			return new DelegateGraphicVoiLutFactory(createVoiLutDelegate);
		}

		private class DelegateGraphicVoiLutFactory : GraphicVoiLutFactory
		{
			private readonly CreateVoiLutDelegate _createVoiLutDelegate;

			public DelegateGraphicVoiLutFactory(CreateVoiLutDelegate createVoiLutDelegate)
			{
				Platform.CheckForNullReference(createVoiLutDelegate, "createVoiLutDelegate");
				_createVoiLutDelegate = createVoiLutDelegate;
			}

			public override IComposableLut CreateVoiLut(ImageGraphic imageGraphic)
			{
				return _createVoiLutDelegate(imageGraphic);
			}
		}
	}
}