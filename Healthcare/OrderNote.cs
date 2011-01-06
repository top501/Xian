#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Collections.Generic;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Enterprise.Core;


namespace ClearCanvas.Healthcare {


    /// <summary>
    /// OrderNote component
    /// </summary>
	public partial class OrderNote
	{
		/// <summary>
		/// Gets all order notes associated with the specified order.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public static IList<OrderNote> GetNotesForOrder(Order order)
		{
			return GetNotesForOrder(order, null);
		}

		/// <summary>
		/// Gets all order notes of the specified category associated with the specified order.
		/// </summary>
		/// <param name="order"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		public static IList<OrderNote> GetNotesForOrder(Order order, string category)
		{
			var where = new OrderNoteSearchCriteria();
			where.Order.EqualTo(order);
			where.PostTime.IsNotNull(); // only posted notes
			if(!string.IsNullOrEmpty(category))
			{
				where.Category.EqualTo(category);
			}

			//run a query to find order notes
			//TODO: using PersistenceScope is maybe not ideal but no other option right now (fix #3472)
			return PersistenceScope.CurrentContext.GetBroker<IOrderNoteBroker>().Find(where);
		}

        /// <summary>
        /// Constructor for creating a new note with recipients.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="category"></param>
        /// <param name="author"></param>
        /// <param name="onBehalfOf"></param>
        /// <param name="body"></param>
        /// <param name="urgent"></param>
        public OrderNote(Order order, string category, Staff author, StaffGroup onBehalfOf, string body, bool urgent)
            :base(category, author, onBehalfOf, body, urgent)
        {
            _order = order;
        }

		/* Commented out for ticket #3709, where it is explicitly requested that user can post new notes without acknowledging previous notes. */
		///// <summary>
		///// Overridden to validate that the order does not have any notes that are pending acknowledgement
		///// that could be acknowledged by the author of this note.
		///// </summary>
		//protected override void BeforePost()
		//{
		//    // does the order have any notes, in the same category as this note,
		//    // that could have been acknowledged by the author of this note but haven't been?
		//    IList<OrderNote> allNotes = GetNotesForOrder(_order, this.Category);
		//    bool unAckedNotes = CollectionUtils.Contains(allNotes,
		//        delegate(OrderNote note)
		//        {
		//            // ignore this note
		//            return !Equals(this, note) && note.CanAcknowledge(this.Author);
		//        });

		//    if (unAckedNotes)
		//        throw new NoteAcknowledgementException("Order has associated notes that must be acknowledged by this author prior to posting a new note.");

		//    base.BeforePost();
		//}


		/// <summary>
		/// This method is called from the constructor.  Use this method to implement any custom
		/// object initialization.
		/// </summary>
		private void CustomInitialize()
		{
		}
	}
}