namespace PhotoConverterWebAppv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConvertedPhotosToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConvertedPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateConverted = c.DateTime(nullable: false),
                        FilePath = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConvertedPhotoes");
        }
    }
}
