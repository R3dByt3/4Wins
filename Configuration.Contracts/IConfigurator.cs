namespace Configuration.Contracts
{
    public interface IConfigurator
    {
        void Load();
        void Set<T>(T Setting);
        T Get<T>();
    }
}
