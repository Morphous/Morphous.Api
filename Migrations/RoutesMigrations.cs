using Morphous.Api.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Morphous.Api.Migrations {
    [OrchardFeature("Morphous.Api.Routes")]
    public class Migrations : DataMigrationImpl {

        public int Create() {

            ContentDefinitionManager.AlterPartDefinition(typeof(ApiRoutePart).Name, part => part.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ApiProjection",
              cfg => cfg
                  .WithPart("ApiRoutePart")
                  .WithPart("CommonPart")
                  .WithPart("TitlePart")
                  .WithPart("ProjectionPart")
              );

            return 1;
        }
    }
}