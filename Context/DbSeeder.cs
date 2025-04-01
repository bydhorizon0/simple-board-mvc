using Dapper;

namespace SimpleFreeBoard.Contexts;

public class DbSeeder
{
    public static void Seed(DapperContext? context)
    {
        if (context is null)
        {
            return;
        }
        
        var conn = context.GetConnection();

        string createUserSql = @"CREATE TABLE IF NOT EXISTS Users(
            email varchar(200) NOT NULL PRIMARY KEY,
            nickname varchar(100) NOT NULL UNIQUE,
            passwordHash varchar(200) NOT NULL,
            role ENUM('admin', 'manager', 'user') NOT NULL DEFAULT 'user',
            createdAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
        );";

        string createFreeBoardSql = @"CREATE TABLE IF NOT EXISTS FreeBoards(
            seq INTEGER PRIMARY KEY AUTO_INCREMENT,
            title TEXT NOT NULL,
            content TEXT NOT NULL,
            userEmail varchar(200) NOT NULL,
            createdAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            FOREIGN KEY(userEmail) REFERENCES Users(email) ON DELETE CASCADE
        );";

        string createAdminBoardSql = @"CREATE TABLE IF NOT EXISTS FreeBoards(
            seq INTEGER PRIMARY KEY AUTO_INCREMENT,
            title TEXT NOT NULL,
            content TEXT NOT NULL,
            userEmail varchar(200) NOT NULL,
            createdAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            FOREIGN KEY(userEmail) REFERENCES Users(email) ON DELETE CASCADE
        );";
        
        conn.Execute(createUserSql);
        conn.Execute(createFreeBoardSql);
        conn.Execute(createAdminBoardSql);
    }
}