using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Enterprise.Common;
using System.Runtime.Serialization;

namespace ClearCanvas.Ris.Application.Common.Admin.ProcedureTypeAdmin
{
	[DataContract]
	public class LoadProcedureTypeForEditRequest : DataContractBase
	{
		public LoadProcedureTypeForEditRequest(EntityRef entityRef)
		{
			this.ProcedureTypeRef = entityRef;
		}

		[DataMember]
		public EntityRef ProcedureTypeRef;
	}
}
