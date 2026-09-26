namespace LearnForge.Domain.Common;

internal static class OrderedList
{
    public static bool Remove<T>(List<T> items, Guid id, Action<T, int> reorder) where T : Entity
    {
        var index = items.FindIndex(x => x.Id == id);
        if (index == -1) return false;

        items.RemoveAt(index);
        for (var i = index; i < items.Count; i++)
            reorder(items[i], i + 1);

        return true;
    }
}