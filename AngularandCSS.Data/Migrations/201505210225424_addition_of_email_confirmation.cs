namespace AngularandCSS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addition_of_email_confirmation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailConfirmations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        ConfirmationToken = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "CustomEmailConfirmation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CustomEmailConfirmation");
            DropTable("dbo.EmailConfirmations");
        }
    }
}
