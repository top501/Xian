﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3620
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageServer.Web.Common {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.ImageServer.Web.Common.SR", typeof(SR).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is some pending work queue item(s) for this study..
        /// </summary>
        internal static string ActionNotAllowed_StudyHasPendingWorkQueue {
            get {
                return ResourceManager.GetString("ActionNotAllowed_StudyHasPendingWorkQueue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Study is being processed..
        /// </summary>
        internal static string ActionNotAllowed_StudyIsBeingProcessing {
            get {
                return ResourceManager.GetString("ActionNotAllowed_StudyIsBeingProcessing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Study has been locked..
        /// </summary>
        internal static string ActionNotAllowed_StudyIsLocked {
            get {
                return ResourceManager.GetString("ActionNotAllowed_StudyIsLocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Study has been archived as lossless but is currently in lossy compression state. It must be restored first..
        /// </summary>
        internal static string ActionNotAllowed_StudyIsLossyOnline {
            get {
                return ResourceManager.GetString("ActionNotAllowed_StudyIsLossyOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Study is nearline..
        /// </summary>
        internal static string ActionNotAllowed_StudyIsNearline {
            get {
                return ResourceManager.GetString("ActionNotAllowed_StudyIsNearline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current session is no longer valid..
        /// </summary>
        internal static string MessageCurrentSessionNoLongerValid {
            get {
                return ResourceManager.GetString("MessageCurrentSessionNoLongerValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Web (@{0},{1} {2}, User:{3}).
        /// </summary>
        internal static string WebGUILogHeader {
            get {
                return ResourceManager.GetString("WebGUILogHeader", resourceCulture);
            }
        }
    }
}
