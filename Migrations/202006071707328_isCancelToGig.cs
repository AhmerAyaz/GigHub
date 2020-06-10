namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isCancelToGig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "isCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gigs", "isCancelled");
        }
    }
}
