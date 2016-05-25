namespace MongoCrud
{
    public interface ICache
    {
        object Get(string key);

        void Add(string key, object entity);

        void Remove(string key);
    }
}
