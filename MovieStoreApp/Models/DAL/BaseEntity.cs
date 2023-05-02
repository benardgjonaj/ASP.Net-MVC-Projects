namespace MovieStoreApp.Models.DAL
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public bool IsDeleted { get; set; }
        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

    }
}
