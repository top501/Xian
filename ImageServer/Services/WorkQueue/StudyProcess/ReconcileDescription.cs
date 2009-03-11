using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Services.WorkQueue.WebEditStudy;

namespace ClearCanvas.ImageServer.Services.WorkQueue.StudyProcess
{
    public abstract class ReconcileDescription
    {
        public ReconcileDescription()
        {
            
        }

        private bool _auto;
        private List<BaseImageLevelUpdateCommand> _commands;// = new List<BaseImageLevelUpdateCommand>();
        private ReconcileAction _action;
        private StudyInformation _existingStudyInfo;
        private ImageSetDescriptor _imageSet;

        public ReconcileAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public StudyInformation ExistingStudy
        {
            get { return _existingStudyInfo; }
            set { _existingStudyInfo = value; }
        }

        public ImageSetDescriptor ImageSetData
        {
            get { return _imageSet; }
            set { _imageSet = value; }
        }

        [XmlArray("Commands")]
        [XmlArrayItem("Command", Type = typeof(AbstractProperty<BaseImageLevelUpdateCommand>))]
        public List<BaseImageLevelUpdateCommand> Commands
        {
            get { return _commands; }
            set { _commands = value; }
        }

        public bool Automatic
        {
            get { return _auto; }
            set { _auto = value; }
        }
    }
}

namespace ClearCanvas.ImageServer.Services.WorkQueue.StudyProcess
{
    public enum ReconcileAction
    {
        Ignore,
        Discard,
        Merge,
        CreateNewStudy
    }
}