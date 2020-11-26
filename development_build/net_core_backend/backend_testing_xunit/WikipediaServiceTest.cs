using Microsoft.AspNetCore.Http;
using net_core_backend.Context;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend_testing_xunit
{
    public class WikipediaServiceTest : DatabaseSeeder
    {
        private readonly IWikipediaDataService service;

        public WikipediaServiceTest(IWikipediaDataService service, IHttpContextAccessor http, IContextFactory factory) : base(http, factory)
        {
            this.service = service;
            this.http = http;
            this.service = service;
        }

        //[Fact]
        //public async Task Test()
        //{
        //    // Inject
        //    // Arrange
        //    // Act
        //    await service.GetWikipediaPage();

        //    // Assert
        //    //Assert.Equal(Serialize(expected), Serialize(result));
        //}
    }
}
