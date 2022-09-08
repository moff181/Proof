namespace Proof.Core.DataStructures
{
    public static class QueueExtensions
    {
        public static void ForEachDequeue<T>(this Queue<T> queue, Action<T> action)
        {
            while(queue.Count > 0)
            {
                T t = queue.Dequeue();
                action(t);
            }
        }
    }
}
