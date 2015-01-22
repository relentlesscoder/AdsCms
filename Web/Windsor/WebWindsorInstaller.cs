using System.Web.Mvc;
using AdCme.Service.Interface;
using AdCme.Service.Service;
using AdCms.AdsWcfService.Repository;
using AdCms.Domain.Interface;
using AdCms.Web.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AdCms.Web.Windsor
{
    public class WebWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            #region Repositories
            container.Register(
                Component.For<IAdsRepository>().ImplementedBy<WcfAdsRepository>().LifestyleTransient());
            #endregion

            #region
            container.Register(
                Component.For<IAdsService>().ImplementedBy<AdsService>().LifestyleTransient());
            #endregion

            #region Controllers
            // Both Classes and Types are new in Windsor 3. Previous versions had just one type to do the job - AllTypes. 
            // In Windsor 3, usage of AllTypes is discouraged, because its name is misleading. 
            // While it suggests that it behaves like Types, truth is it's exactly the same as Classes, pre-filtering all types to just non-abstract classes. To avoid confusion, use one of the two new types. 
            container.Register(
                Classes.FromAssemblyContaining<HomeController>()
                .BasedOn<IController>()
                .LifestyleTransient());
            #endregion
        }
    }
}