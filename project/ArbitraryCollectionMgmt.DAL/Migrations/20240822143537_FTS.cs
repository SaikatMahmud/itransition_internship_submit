using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArbitraryCollectionMgmt.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FTS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                             @"CREATE FULLTEXT CATALOG FTCatalog AS DEFAULT;",
                             true);

            migrationBuilder.Sql(
                        @"CREATE FULLTEXT INDEX ON Comments(CommentText) 
                               KEY INDEX PK_Comments
                               WITH STOPLIST = OFF",
                        true);

            migrationBuilder.Sql(
                             @"CREATE FULLTEXT INDEX ON Items(Name) 
                               KEY INDEX PK_Items
                               WITH STOPLIST = OFF",
                             true);
            migrationBuilder.Sql(
                            @"CREATE FULLTEXT INDEX ON CustomValues(FieldValue) 
                               KEY INDEX PK_CustomValues
                               WITH STOPLIST = OFF",
                            true);

            migrationBuilder.Sql(
                             @"CREATE FULLTEXT INDEX ON Collections(Name) 
                               KEY INDEX PK_Collections
                               WITH STOPLIST = OFF",
                             true);
            migrationBuilder.Sql(
                           @"CREATE FULLTEXT INDEX ON Categories(Name) 
                               KEY INDEX PK_Categories
                               WITH STOPLIST = OFF",
                           true);

            migrationBuilder.Sql(
                             @"CREATE FULLTEXT INDEX ON Tags(Name) 
                               KEY INDEX PK_Tags
                               WITH STOPLIST = OFF",
                             true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON Comments", true);
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON Items", true);
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON CustomValues", true);
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON Collections", true);
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON Categories", true);
            migrationBuilder.Sql(@"DROP FULLTEXT INDEX ON Tags", true);
            migrationBuilder.Sql(@"DROP FULLTEXT CATALOG FTCatalog", true);
        }
    }
}
