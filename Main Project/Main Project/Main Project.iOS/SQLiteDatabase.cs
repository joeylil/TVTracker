using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Main_Project.iOS.SQLiteDatabase))]

namespace Main_Project.iOS
{
    class SQLiteDatabase : SQLiteInterface
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath =
           Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "favourite.db2");
            return new SQLiteAsyncConnection(path);
        }
    }
}