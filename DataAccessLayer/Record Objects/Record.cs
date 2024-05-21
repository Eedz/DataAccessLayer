
namespace ITCLib
{
    public interface IRecord
    {
        bool NewRecord { get; set; }
        bool Dirty { get; set; }
        int SaveRecord();
    }
}
