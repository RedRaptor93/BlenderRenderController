﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BRClib.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BRClib.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read output path, using project location..
        /// </summary>
        public static string AppErr_BlendOutputInvalid {
            get {
                return ResourceManager.GetString("AppErr_BlendOutputInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to read project info, no information was received..
        /// </summary>
        public static string AppErr_NoInfoReceived {
            get {
                return ResourceManager.GetString("AppErr_NoInfoReceived", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The render format is a {0} image.
        ///You can render an image sequence with this tool but you will need to make a video with other SW..
        /// </summary>
        public static string AppErr_RenderFormatIsImage {
            get {
                return ResourceManager.GetString("AppErr_RenderFormatIsImage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more required program(s) were not found (Path invalid OR first time run), set the paths in the Settings window.
        /// </summary>
        public static string AppErr_RequiredProgramsNotFound {
            get {
                return ResourceManager.GetString("AppErr_RequiredProgramsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to read project info. Unexpected output..
        /// </summary>
        public static string AppErr_UnexpectedOutput {
            get {
                return ResourceManager.GetString("AppErr_UnexpectedOutput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Join chunks w/ Mixdown audio.
        /// </summary>
        public static string AR_JoinMixdown {
            get {
                return ResourceManager.GetString("AR_JoinMixdown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Join chunks.
        /// </summary>
        public static string AR_JoinOnly {
            get {
                return ResourceManager.GetString("AR_JoinOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No extra action.
        /// </summary>
        public static string AR_NoAction {
            get {
                return ResourceManager.GetString("AR_NoAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Blender Renderer.
        /// </summary>
        public static string Renderer_Blender {
            get {
                return ResourceManager.GetString("Renderer_Blender", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cycles Renderer.
        /// </summary>
        public static string Renderer_Cycles {
            get {
                return ResourceManager.GetString("Renderer_Cycles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error ocurred while rendering, check the output folder for a log file.
        /// </summary>
        public static string RM_unexpected_error {
            get {
                return ResourceManager.GetString("RM_unexpected_error", resourceCulture);
            }
        }
    }
}
