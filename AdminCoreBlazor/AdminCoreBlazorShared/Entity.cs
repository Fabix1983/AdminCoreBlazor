namespace AdminCoreBlazorShared
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; } = default!;

    }
}
