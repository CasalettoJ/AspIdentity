namespace AngularandCSS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changing_users_table : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", "UserNameIndex");
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Users", "UserName", unique: true, name: "UserNameIndex");
            DropColumn("dbo.Users", "UserID");
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "Salt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Salt", c => c.String());
            AddColumn("dbo.Users", "Password", c => c.String());
            AddColumn("dbo.Users", "UserID", c => c.Int(nullable: false));
            DropIndex("dbo.Users", "UserNameIndex");
            AlterColumn("dbo.Users", "UserName", c => c.String());
            CreateIndex("dbo.Users", "UserName", unique: true, name: "UserNameIndex");
        }
    }
}
