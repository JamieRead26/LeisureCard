// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Reflection;
using Castle.DynamicProxy;
using FluentNHibernate.Cfg;
using GRG.LeisureCards.Data;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using GRG.LeisureCards.Persistence.NHibernate.ClassMaps;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.Service.BusinessLogic;
using GRG.LeisureCards.WebAPI.Interceptors;
using StructureMap.Pipeline;

namespace GRG.LeisureCards.WebAPI.DependencyResolution
{
    using StructureMap.Configuration.DSL;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.Assembly("GRG.LeisureCards.Service");
                    scan.Assembly("GRG.LeisureCards.WebAPI");
                    scan.WithDefaultConventions();
                });

            var classMapAssembly = Assembly.GetAssembly(typeof(LeisureCardClassMap));

            var sessionFactory = Fluently.Configure()
                .Database(Database.GetPersistenceConfigurer(Config.DbConnectionDetails))
                .Mappings(m => m.FluentMappings.AddFromAssembly(classMapAssembly))
#if DEBUG
.ExposeConfiguration(x => x.SetInterceptor(new SqlStatementInterceptor()))
#endif
.BuildSessionFactory();

            var proxyGenerator = new ProxyGenerator();

            var interceptor = new UnitOfWorkInterceptor(sessionFactory);

            ConfigureIntercepts(proxyGenerator, interceptor);
        }

        private void ConfigureIntercepts(ProxyGenerator proxyGenerator, IInterceptor interceptor)
        {
            ConfigureRepositoryIntercepts(proxyGenerator, interceptor);
            ConfigureServiceIntercepts(proxyGenerator, interceptor);
            ConfigureBusinessLogic();
            ConfigureProviders();
        }

        private void ConfigureRepositoryIntercepts(ProxyGenerator proxyGenerator, IInterceptor interceptor)
        {
            For<ILeisureCardRepository>().Use<LeisureCardRepository>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();

            For<ISettingRepository>().Use<SettingRepository>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();

            For<IRedLetterProductRepository>().Use<RedLetterProductRepository>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();

            For<IDataImportJournalEntryRepository>().Use<DataImportJournalEntryRepository>()
               .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
               .SetLifecycleTo<SingletonLifecycle>();

            For<ITwoForOneRepository>().Use<TwoForOneRepository>()
               .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
               .SetLifecycleTo<SingletonLifecycle>();

            For<ILeisureCardUsageRepository>().Use<LeisureCardUsageRepository>()
              .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
              .SetLifecycleTo<SingletonLifecycle>();

            For<ISelectedOfferRepository>().Use<SelectedOfferRepository>()
             .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
             .SetLifecycleTo<SingletonLifecycle>();

            For<IOfferCategoryRepository>().Use<OfferCategoryRepository>()
            .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
            .SetLifecycleTo<SingletonLifecycle>();

            For<ILocationRepository>().Use<LocationRepository>()
            .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
            .SetLifecycleTo<SingletonLifecycle>();

            For<ICardGenerationLogRepository>().Use<CardGenerationLogRepository>()
            .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
            .SetLifecycleTo<SingletonLifecycle>();
        }

        private void ConfigureServiceIntercepts(ProxyGenerator proxyGenerator, IInterceptor interceptor)
        {
            For<ILeisureCardService>().Use<LeisureCardService>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();

#if DEBUG
            For<ITestService>().Use<TestService>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();
#endif
            For<IDataImportService>().Use<DataImportService>()
                .DecorateWith(i => proxyGenerator.CreateInterfaceProxyWithTargetInterface(i, interceptor))
                .SetLifecycleTo<SingletonLifecycle>();

            For<IGoogleLocationService>()
                .Use(() => new GoogleLocationService(true, ConfigurationManager.AppSettings["GoogleApiKey"]))
                .SetLifecycleTo<SingletonLifecycle>();
        }

        public void ConfigureBusinessLogic()
        {
            For<ICardRenewalLogic>()
                .Use(() => new CardRenewalLogic(int.Parse(ConfigurationManager.AppSettings["DefaultCardRenewalPeriodMonths"])))
                .SetLifecycleTo<SingletonLifecycle>();
        }

        public void ConfigureProviders()
        {
            For<IAdminCodeProvider>()
                .Use(() => new AdminCodeProvider(ConfigurationManager.AppSettings["AdminCode"]))
                .SetLifecycleTo<SingletonLifecycle>();

            var fileImportManager = new FileImportManager(
                ConfigurationManager.AppSettings["RedLetterFtpPath"],
                ConfigurationManager.AppSettings["RedLetterFtpUid"],
                ConfigurationManager.AppSettings["RedLetterFtpPassword"]);

            For<IFileImportManager>()
                .Use(() => fileImportManager )
                .SetLifecycleTo<SingletonLifecycle>();
        }

        #endregion
    }
}