using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using Business.Other;
using Business.Services;

namespace Business
{
    public class Api : IApi
    {
        public IActorService ActorService { get; }

        public ActorInfoService ActorInfoService { get; }

        public IXmlContext<Actor> Context { get; }

        public bool IsSaved { get; set; }

        /// <summary/>
        /// <param name="context"></param>
        /// <param name="saveFile"></param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown when:
        /// The context is null
        /// -or-
        /// The saveFile is null or empty
        /// </exception>
        public Api(IXmlContext<Actor> context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            var handler = new XDocumentHandler(context);
            ActorInfoService = new ActorInfoService(handler);
            ActorService = new ActorService(context);
            void changeSaved() 
            { 
                IsSaved = false;
                handler.NeedsRegeneration = true;
            }
            ActorService.OnChange += changeSaved;
        }

        /// <summary>
        /// A path to file to which the content should be saved
        /// </summary>
        public string SaveFile
        {
            get
            {
                return _saveFile;
            }
            set
            {
                _saveFile = value;
                IsSaved = false;
            }
        }

        private string _saveFile = string.Empty;

        /// <summary>
        /// Saves the context to a file which path is specified in the SaveFile property
        /// </summary>
        /// <exception cref="StackOverflowException">
        /// More likely, your objects contains loop(s)
        /// -or-
        /// the property object type is derived from property's type, it contains loop(s)
        /// and no attribute [XmlIgnoreInheritance] has been specified to such a property(ies)
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Path is an empty string (""), contains only white space, 
        /// or contains one or more invalid characters.
        /// -or- path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.
        /// in an NTFS environment.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        /// non-NTFS environment.
        /// </exception>
        /// <exception cref="ArgumentNullException"> Path is null.</exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">Path specifies a file that is read-only.</exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        public void Save()
        {
            Context.Save(SaveFile);
            IsSaved = true;
        }
    }
}
