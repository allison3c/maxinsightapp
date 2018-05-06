using Plugin.FilePicker.Abstractions;
using System;
using XLabs.Ioc;

namespace Plugin.FilePicker
{
    /// <summary>
    /// Cross platform FilePicker implemenations
    /// </summary>
    public class CrossFilePicker
    {
        static Lazy<IFilePicker> Implementation = new Lazy<IFilePicker>(() => CreateFilePicker(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IFilePicker Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IFilePicker CreateFilePicker()
        {
            return Resolver.Resolve<IFilePicker>();
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
