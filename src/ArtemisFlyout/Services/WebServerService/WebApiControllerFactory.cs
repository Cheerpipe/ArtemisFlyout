using System;
using ArtemisFlyout.IoC;
using EmbedIO.WebApi;

namespace ArtemisFlyout.Services
{
    internal class WebApiControllerRegistration<T> : WebApiControllerRegistration where T : WebApiController
    {
        public WebApiControllerRegistration() : base(typeof(T))
        {
            Factory = Kernel.Get<T>;
        }

        public Func<T> Factory { get; set; }
        public override object UntypedFactory => Factory;
    }

    internal abstract class WebApiControllerRegistration
    {
        protected WebApiControllerRegistration(Type controllerType)
        {
            ControllerType = controllerType;
        }

        public abstract object UntypedFactory { get; }
        public Type ControllerType { get; set; }
    }
}