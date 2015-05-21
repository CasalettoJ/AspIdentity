namespace AngularandCSS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class password_recovery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordRecovers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        RecoveryToken = c.String(),
                        ValidUntil = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PasswordRecovers");
        }
    }
}
