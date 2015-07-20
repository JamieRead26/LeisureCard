// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
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
using System.Web;
using System.Web.Mvc;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Service;
using GRG.LeisureCards.WebAPI.App_Start;
using log4net;
using NHibernate.Cfg;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace GRG.LeisureCards.WebAPI.App_Start {

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

	using GRG.LeisureCards.WebAPI.DependencyResolution;

    using StructureMap;

    
    
	public static class StructuremapMvc {
        #region Public Properties

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        #endregion
		
		#region Public Methods and Operators

        private static readonly ILog Log = LogManager.GetLogger(typeof(StructuremapMvc));
		
		public static void End() {
            StructureMapDependencyScope.Dispose();
        }
		
        public static void Start() {
            var container = IoC.Initialize();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));

            var dataImportService = container.GetInstance<IDataImportService>();
            var fileImportManager = container.GetInstance<IFileImportManager>();
            var dataImportJournalEntryRepository = container.GetInstance<IDataImportJournalEntryRepository>();

            
            var minutes = int.Parse(ConfigurationManager.AppSettings["RedLetterAutoDownloadDayMinutes"]);
            Log.Info("Initialisaing daily task scheduler with RedLetterAutoDownloadDayMinutes = " +minutes);

            DailyTaskScheduler.Instance.ScheduleTask(() =>
                {
                    var journalEntry = fileImportManager.StoreDataFile(DataImportKey.RedLetter,()=>fileImportManager.GetRedLetterData());

                    dataImportJournalEntryRepository.SaveOrUpdate(journalEntry);

                    if (journalEntry.Success)
                        dataImportService.Import(DataImportKey.RedLetter, path => WebApiApplication.AppRoot + path.Substring(1));
                },
                minutes
            );
        }

        #endregion
    }
}