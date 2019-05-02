namespace WPF_HW5_Beer_Pack.Interfaces
{
    public interface IReader<T>
    {
        T Read(string filename);
    }
}
