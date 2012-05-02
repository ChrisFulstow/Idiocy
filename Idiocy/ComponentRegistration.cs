using System;

namespace Idiocy
{
    public class ComponentRegistration
    {
        public ComponentRegistration(Func<object> activator, Lifetime? lifetime = null)
        {
            Activator = activator;
            ComponentLifetime = lifetime ?? Lifetime.Transient;
        }

        public Func<object> Activator { get; set; }
        public Lifetime ComponentLifetime { get; set; }
    }
}