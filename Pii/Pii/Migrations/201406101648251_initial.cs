namespace Pii.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Variables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Attention = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            
        }
    }
}
