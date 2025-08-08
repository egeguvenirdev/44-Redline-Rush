
public interface IPoolable
{
    public PoolType PoolType { get; }
    public void OnTakenFromPool();
    public void OnReturnedToPool();
}
