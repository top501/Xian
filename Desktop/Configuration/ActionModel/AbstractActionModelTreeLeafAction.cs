#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Trees;
using System;

namespace ClearCanvas.Desktop.Configuration.ActionModel
{
	// TODO CR (Apr 10): If this class remains member-less, convert to an abstract IsLeaf flag on the base node class
	public abstract class AbstractActionModelTreeLeaf : AbstractActionModelTreeNode
	{
		protected AbstractActionModelTreeLeaf(PathSegment pathSegment)
			: base(pathSegment) {}
	}

	public class AbstractActionModelTreeLeafAction : AbstractActionModelTreeLeaf
	{
		private readonly AbstractAction _action;

		public AbstractActionModelTreeLeafAction(IAction action)
			: base(action.Path.LastSegment)
		{
			Platform.CheckForNullReference(action, "action");
			Platform.CheckTrue(action.Persistent, "Action must be persistent.");

			// this allows us to keep a "clone" that is independent of the live action objects
			// that might (probably are) in use or cached in some tool or component somewhere.
			_action = AbstractAction.Create(action);

			CheckState = _action.Available ? CheckState.Checked : CheckState.Unchecked;

			IconSet iconSet;
			if (action.IconSet == null || action.ResourceResolver == null)
			{
				iconSet = new IconSet(IconScheme.Colour, "Icons.ActionModelNullSmall.png", "Icons.ActionModelNullMedium.png", "Icons.ActionModelNullLarge.png");
				ResourceResolver = new ResourceResolver(typeof(AbstractActionModelTreeLeafAction).Assembly, action.ResourceResolver);
			}
			else
			{
				iconSet = _action.IconSet;
				ResourceResolver = _action.ResourceResolver;
			}

			if (_action.Permissible)
			{
				IconSet = iconSet;
			}
			else
			{
				IconSet = new GrayscaleIconSet(iconSet);
				Tooltip = String.IsNullOrEmpty(CanonicalLabel) ?
					SR.TooltipActionNotPermitted : String.Format(SR.TooltipFormatActionNotPermitted, CanonicalLabel);
			}
		}

		public string ActionId
		{
			get { return _action.ActionId; }
		}

		protected IAction Action
		{
			get { return _action; }
		}

		protected override void OnCheckStateChanged()
		{
			base.OnCheckStateChanged();

			_action.Available = CheckState == CheckState.Checked;
		}

		internal IAction BuildAction()
		{
			IAction action = AbstractAction.Create(_action);

			Stack<PathSegment> stack = new Stack<PathSegment>();
			AbstractActionModelTreeNode current = this;
			do
			{
				stack.Push(current.PathSegment);
				current = current.Parent;
			} while (current != null);

			Path path = new Path(stack.Pop()); // the first path segment is the site, which is never processed through the resource resolver
			while (stack.Count > 0)
			{
				// for each subsequent segment, ensure the action's resolver will resolve the string in the expected way
				PathSegment pathSegment = stack.Pop();
				string localizedString = action.ResourceResolver.LocalizeString(pathSegment.ResourceKey);
				if (localizedString == pathSegment.LocalizedText)
					path = path.Append(pathSegment);
				else
					path = path.Append(new PathSegment(pathSegment.LocalizedText, pathSegment.LocalizedText));
			}

			action.Path = new ActionPath(path.ToString(), action.ResourceResolver);
			return action;
		}
	}
}