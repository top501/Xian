using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.Ris.Application.Common.Admin
{
    [DataContract]
    public class AddFacilityRequest : DataContractBase
    {
        [DataMember]
        public string Name;

        [DataMember]
        public string Code;
    }
}
