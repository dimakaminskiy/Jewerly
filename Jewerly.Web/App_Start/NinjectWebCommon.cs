using Jewerly.Domain;
using Jewerly.Domain.Entities;
using Jewerly.Domain.Repository;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Jewerly.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Jewerly.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Jewerly.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
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

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IGenericRepository<Category>>().To<GenericRepository<Category>>();
            kernel.Bind<IGenericRepository<Currency>>().To<GenericRepository<Currency>>();
            kernel.Bind<IGenericRepository<Picture>>().To<GenericRepository<Picture>>();
            kernel.Bind<IGenericRepository<CategoryPicture>>().To<GenericRepository<CategoryPicture>>();
            kernel.Bind<IGenericRepository<Product>>().To<ProductsRepository>();
            kernel.Bind<IGenericRepository<ProductSpecificationAttribute>>().To<GenericRepository<ProductSpecificationAttribute>>();
            kernel.Bind<IGenericRepository<SpecificationAttributeOption>>().To<GenericRepository<SpecificationAttributeOption>>();
            kernel.Bind<IGenericRepository<MappingProductSpecificationAttributeToProduct>>().To<GenericRepository<MappingProductSpecificationAttributeToProduct>>();



            kernel.Bind<IGenericRepository<MappingProductChoiceAttributeToProduct>>().To<GenericRepository<MappingProductChoiceAttributeToProduct>>();
            kernel.Bind<IGenericRepository<AvalibleChoiceAttributeOption>>().To<GenericRepository<AvalibleChoiceAttributeOption>>();
            kernel.Bind<IGenericRepository<ProductChoiceAttribute>>().To<GenericRepository<ProductChoiceAttribute>>();
            kernel.Bind<IGenericRepository<ChoiceAttributeOption>>().To<GenericRepository<ChoiceAttributeOption>>();

            kernel.Bind<IGenericRepository<Discount>>().To<GenericRepository<Discount>>();
            kernel.Bind<IGenericRepository<Markup>>().To<GenericRepository<Markup>>();

            kernel.Bind<IGenericRepository<Review>>().To<GenericRepository<Review>>();
            kernel.Bind<IGenericRepository<Country>>().To<GenericRepository<Country>>();

            kernel.Bind<IGenericRepository<Order>>().To<GenericRepository<Order>>();
            kernel.Bind<IGenericRepository<OrderDetail>>().To<GenericRepository<OrderDetail>>();

            kernel.Bind<IGenericRepository<OrderStatus>>().To<GenericRepository<OrderStatus>>();
            kernel.Bind<IGenericRepository<Cart>>().To<GenericRepository<Cart>>();
            kernel.Bind<IGenericRepository<SliderPicture>>().To<GenericRepository<SliderPicture>>();
            
        }        
    }
}
