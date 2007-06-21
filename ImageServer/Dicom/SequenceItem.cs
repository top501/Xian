using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.ImageServer.Dicom.Exceptions;
using ClearCanvas.Dicom.OffisWrapper;
using ClearCanvas.ImageServer.Dicom.Offis;

namespace ClearCanvas.ImageServer.Dicom
{
    public class SequenceItem : AttributeCollection
    {
        public SequenceItem() : base()
        {
        }

        internal SequenceItem(DcmItem theSet, DcmFileFormat fileFormat) :
            base(theSet,fileFormat)
        {
        }
        internal SequenceItem(AttributeCollection source, bool copyBinary)
            : base(source, copyBinary)
        {
        }

        public override AttributeCollection Copy()
        {
            return Copy(true);
        }

        public override AttributeCollection Copy(bool copyBinary)
        {
            return new SequenceItem(this,copyBinary);
        }

    }
}
