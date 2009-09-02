#region License

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.View.WinForms;
using ClearCanvas.ImageViewer.Tools.Standard.ImageProperties;

namespace ClearCanvas.ImageViewer.Tools.Standard.View.WinForms
{
	/// <summary>
	/// Provides a Windows Forms user-interface for <see cref="ImagePropertiesApplicationComponent"/>.
	/// </summary>
	public partial class ImagePropertiesApplicationComponentControl : ApplicationComponentUserControl
	{
		private readonly ImagePropertiesApplicationComponent _component;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ImagePropertiesApplicationComponentControl(ImagePropertiesApplicationComponent component)
			: base(component)
		{
			_component = component;
			InitializeComponent();

			_component.PropertyChanged += Update;

			Update(null, null);
		}
/*
		//Not currently used.
		private void UpdateCollapsedCategories()
		{
			GridItem topLevel = _properties.SelectedGridItem;

			if (topLevel != null)
			{
				while (topLevel.Parent != null)
					topLevel = topLevel.Parent;
			}

			List<string> collapsedCategories = new List<string>();
			if (topLevel != null && topLevel != _properties.SelectedGridItem)
			{
				foreach (GridItem gridItem in topLevel.GridItems)
				{
					if (gridItem.Expanded)
						collapsedCategories.Add(gridItem.Label);
				}
			}

			_component.CollapsedCategories = collapsedCategories.ToArray();
		}
*/
		private void Update(object sender, PropertyChangedEventArgs e)
		{
			_properties.SelectedObject = new ImageProperties(_component.ImageProperties);
		}
	}

	//hack to get rid of the "Property Pages" toolbar item and make the text edit box read-only
	internal class CustomPropertyGrid : PropertyGrid
	{
		public CustomPropertyGrid()
		{
			base.ToolStripRenderer = new CustomToolStripRenderer();
		}

		protected override void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
		{
			base.OnSelectedGridItemChanged(e);
			MakeAllTextBoxesReadOnly(this);
		}

		protected override void OnSelectedObjectsChanged(EventArgs e)
		{
			base.OnSelectedObjectsChanged(e);
			MakeAllTextBoxesReadOnly(this);
		}

		private static void MakeAllTextBoxesReadOnly(Control parent)
		{
			//force all TextBox controls owned by the parent to be read-only.
			foreach (Control control in parent.Controls)
			{
				if (control is TextBox)
				{
					TextBox box = (TextBox) control;
					box.ReadOnly = true;
				}
				else
				{
					MakeAllTextBoxesReadOnly(control);
				}
			}
		}
	}

	//hack to hide the "Property Pages" toolbar item
	internal class CustomToolStripRenderer : ToolStripRenderer
	{
		private string _propertyPagesTooltip;

		protected override void Initialize(ToolStrip toolStrip)
		{
			if (toolStrip.Items.Count == 5) //reflector shows the toolbar is fixed, so this is ok.
				_propertyPagesTooltip = toolStrip.Items[4].ToolTipText;

			base.Initialize(toolStrip);
		}

		protected override void InitializeItem(ToolStripItem item)
		{
			base.InitializeItem(item);

			if (item.ToolTipText == _propertyPagesTooltip)
			{
				item.Visible = false;
				item.Enabled = false;
			}
		}
	}

	internal class ShowValueEditor : UITypeEditor
	{
		public ShowValueEditor()
		{
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			string stringValue = "";

			if (value != null)
			{
				if (value is string)
				{
					stringValue = value as string;
				}
				else
				{
					TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
					if (converter != null)
					{
						if (converter.CanConvertTo(typeof(string)))
							stringValue = converter.ConvertToString(value);
						else
							stringValue = value.ToString();
					}
					else
						stringValue = value.ToString();
				}
			}

			ImagePropertyDescriptor descriptor = (ImagePropertyDescriptor)context.PropertyDescriptor;
			ShowValueDialog.Show(descriptor.Name, descriptor.Description, stringValue);
			return value; //no edits allowed
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}

	internal class ImagePropertyDescriptor : PropertyDescriptor
	{
		public readonly IImageProperty ImageProperty;

		public ImagePropertyDescriptor(IImageProperty imageProperty)
			: base(imageProperty.Name, CreateAttributes(imageProperty))
		{
			ImageProperty = imageProperty;
		}

		private static Attribute[] CreateAttributes(IImageProperty imageProperty)
		{
			CategoryAttribute category = new CategoryAttribute(imageProperty.Category);
			DescriptionAttribute description = new DescriptionAttribute(imageProperty.Description);
			EditorAttribute editor = new EditorAttribute(typeof(ShowValueEditor), typeof(UITypeEditor));

			return new Attribute[] { category, description, editor };
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override Type ComponentType
		{
			get { return ImageProperty.GetType(); }
		}

		public override object GetValue(object component)
		{
			return ImageProperty.Value;
		}

		public override bool IsReadOnly
		{
			//has to be false, otherwise the text renders too light.
			get { return false; }
		}

		public override Type PropertyType
		{
			get { return ImageProperty.ValueType; }
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}

	internal class ImageProperties : ICustomTypeDescriptor
	{
		private readonly PropertyDescriptorCollection _propertyDescriptors;

		public ImageProperties(IList<IImageProperty> properties)
		{
			_propertyDescriptors = new PropertyDescriptorCollection(
				CollectionUtils.Map(properties,
								delegate(IImageProperty property)
									{
										return new ImagePropertyDescriptor(property);
									}).ToArray()
				);
		}

		#region ICustomTypeDescriptor Members

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public object GetEditor(Type editorBaseType)
		{
			return new ShowValueEditor();
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return _propertyDescriptors;
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		#endregion
	}
}