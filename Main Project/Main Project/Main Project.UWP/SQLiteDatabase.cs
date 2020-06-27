using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Main_Project.UWP.SQLiteDatabase))]

namespace Main_Project.UWP
{

    class SQLiteDatabase : SQLiteInterface
    {
        public SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection("Favourite.db2");
        }
    }
}
