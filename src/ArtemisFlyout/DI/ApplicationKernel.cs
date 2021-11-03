using System.Reflection;
using Ninject;

namespace ArtemisFlyout.DI
{
    public class ApplicationKernel
    {
        private static StandardKernel _kernel;

         static ApplicationKernel()
        {
            _kernel = new StandardKernel();
            _kernel.Load(Assembly.GetExecutingAssembly());
        }
        public static StandardKernel GetKernel()
        {
            return _kernel;
        }
    }
}
