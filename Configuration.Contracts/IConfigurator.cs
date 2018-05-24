namespace Configuration.Contracts
{
    public interface IConfigurator
    {
        void Load();
        void Save();
        void Set<T>(T Setting);
        T Get<T>();
    }
}
