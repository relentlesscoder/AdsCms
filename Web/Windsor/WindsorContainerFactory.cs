using Castle.Windsor;

namespace AdCms.Web.Windsor
{
    public sealed class WindsorContainerFactory
    {
        private WindsorContainerFactory()
        {
        }

        public static WindsorContainer Instance { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
                _instance.Install(new WebWindsorInstaller());
            }

            internal static readonly WindsorContainer _instance = new WindsorContainer();
        }
    }
}