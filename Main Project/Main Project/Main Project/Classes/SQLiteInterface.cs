using SQLite;

namespace Main_Project
{
    public interface SQLiteInterface
    {
        SQLiteAsyncConnection GetConnection();
    }
}
