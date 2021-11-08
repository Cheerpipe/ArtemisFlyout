using System.Collections.Generic;
using Ninject;
using Ninject.Modules;

namespace ArtemisFlyout.IoC
{
    public static class Kernel
    {
        private static StandardKernel _kernel;

        public static T Get<T>()
        {
            return _kernel.Get<T>();

        }

        public static bool Release(object instance)
        {
            return _kernel.Release(instance);
        }

        public static void Initialize(params INinjectModule[] modules)
        {
            _kernel ??= new StandardKernel(modules);
        }
    }
}