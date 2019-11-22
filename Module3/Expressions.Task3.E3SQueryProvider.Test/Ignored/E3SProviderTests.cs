using Expressions.Task3.E3SQueryProvider.Client;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Expressions.Task3.E3SQueryProvider.QueryProvider;
using Expressions.Task3.E3SQueryProvider.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Expressions.Task3.E3SQueryProvider.Test
{
    /// <summary>
    /// Please ignore this integration test set, because the E3S emulator is not currently available.
    /// </summary>
    [Ignore]
    [TestClass]
    public class E3SProviderTests
    {
        private static IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private static string User = config["api:user"];
        private static string Password = config["api:password"];
        private static string BaseUrl = config["api:apiBaseUrl"];

        private static readonly Lazy<E3SSearchService> searchService = new Lazy<E3SSearchService>(() =>
        {
            HttpClient httpClient = HttpClientHelper.CreateClient(User, Password);
            return new E3SSearchService(httpClient, BaseUrl);
        });

        #region public tests

        [TestMethod]
        public void WithoutProvider()
        {
            IEnumerable<EmployeeEntity> res = searchService.Value.SearchFTS<EmployeeEntity>("workstation:(EPRUIZHW0249)", 0, 1);

            foreach (var emp in res)
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        [TestMethod]
        public void WithoutProviderNonGeneric()
        {
            var res = searchService.Value.SearchFTS(typeof(EmployeeEntity), "workstation:(EPRUIZHW0249)", 0, 10);

            foreach (var emp in res.OfType<EmployeeEntity>())
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }
        
        [TestMethod]
        public void WithProvider()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(searchService.Value);

            foreach (var emp in employees.Where(e => e.Workstation == "EPRUIZHW0249"))
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        #endregion
    }
}
