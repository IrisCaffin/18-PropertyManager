namespace PropertyManager.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "State", c => c.String());
            AddColumn("dbo.Properties", "HasOutdoorSpace", c => c.Boolean(nullable: false));
            AddColumn("dbo.WorkOrders", "OpenedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Addresses", "Region");
            DropColumn("dbo.WorkOrders", "OpenDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrders", "OpenDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Addresses", "Region", c => c.String());
            DropColumn("dbo.WorkOrders", "OpenedDate");
            DropColumn("dbo.Properties", "HasOutdoorSpace");
            DropColumn("dbo.Addresses", "State");
        }
    }
}
