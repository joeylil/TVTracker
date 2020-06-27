
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Main_Project.Droid.SQLiteDatabase))]

namespace Main_Project.Droid
{
    class SQLiteDatabase : SQLiteInterface
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath =
           Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Favourite.db2");
            return new SQLiteAsyncConnection(path);
        }
    }
}