namespace ITCLib
{
    public interface IRecord<T>
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        T Item { get; set; }
        int SaveRecord();
    }
}
