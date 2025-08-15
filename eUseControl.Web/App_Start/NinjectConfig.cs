[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(eUseControl.Web.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(eUseControl.Web.App_Start.NinjectConfig), "Stop")]

namespace eUseControl.Web.App_Start
{
    using System;
    using System.Web;
    using AutoMapper;
    using eUseControl.Web.Mappings;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectConfig
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AccountProfile>();
                cfg.AddProfile<CartProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<ContactProfile>();
                cfg.AddProfile<CouponProfile>();
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<ReviewProfile>();
                cfg.AddProfile<TransactionProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<ChatProfile>();
            });

            var mapper = mapperConfig.CreateMapper();
            kernel.Bind<IMapper>().ToConstant(mapper);
        }
    }
}