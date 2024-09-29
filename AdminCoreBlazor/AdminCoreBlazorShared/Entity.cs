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


    public interface IArchiveableEntity<TKey> : IEntity<TKey>
    {
        bool IsDeleted { get; }
    }

    public abstract class ArchiveableEntity<TKey> : Entity<TKey>, IArchiveableEntity<TKey>
    {
        public bool IsDeleted { get; set; }
    }
}
