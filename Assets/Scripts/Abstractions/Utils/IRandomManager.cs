namespace Infrastructure
{
    internal interface IRandomManager
    {
        int GetRandomExcludingMax(int max = int.MaxValue, int min = 0);
        int GetRandomIncludingMax(int max = int.MaxValue, int min = 0);
    }
}