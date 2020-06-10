namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAttendance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Attendee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attendances", "GigId", "dbo.Gigs");
            DropIndex("dbo.Attendances", new[] { "GigId" });
            DropIndex("dbo.Attendances", new[] { "Attendee_Id" });
            DropTable("dbo.Attendances");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        GigId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Attendee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GigId, t.UserId });
            
            CreateIndex("dbo.Attendances", "Attendee_Id");
            CreateIndex("dbo.Attendances", "GigId");
            AddForeignKey("dbo.Attendances", "GigId", "dbo.Gigs", "Id");
            AddForeignKey("dbo.Attendances", "Attendee_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
