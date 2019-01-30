using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Services.Tests
{
    public class MetasServiceTests : BaseServiceTests
    {
        private int _metaCounter = 1;
        private List<MetaData> _datas = new List<MetaData>();
        private readonly IMetasService _metasService;
        private readonly Mock<IMetasDataAccess> _metaDataAccess = new Mock<IMetasDataAccess>();
        public MetasServiceTests()
        {
            _metaDataAccess.Setup(m => m.SaveAsync(It.IsAny<MetaData>(), It.IsAny<DataAccessActions>()))
              .Returns(Task.FromResult(_metaCounter)) //<-- returning the input value from task.
              .Callback(
              (MetaData m, DataAccessActions action) =>
              {
                  if (action == DataAccessActions.Insert)
                  {
                      _datas.Add(m);
                      _metaCounter++;
                  }
                  else if (action == DataAccessActions.Update)
                  {

                  }
                  else
                  {
                      // delete
                  }
              });



            _metasService = new MetasService(new LoggerFactory(),
                _metaDataAccess.Object,
                new MetadataValidator());
        }


        [Fact]
        public async Task CreateMetaData()
        {
            var m = new MetaData()
            {
                Key = "keywords",
                SiteId = SITE_ID,
                Value = "test keyword",
                ItemId = 1,
                Type = MetaDataTypes.Page
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }

        [Fact]
        public async Task CreateMetaDataWithMissingSiteId()
        {
            var m = new MetaData()
            {
                Key = "keywords",
                SiteId = 0,
                Value = "test keyword"
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }
    }
}
