using System;

namespace QLQuanNet
{
    public enum DataChangeType
    {
        Other = 0,
        TopUp = 1,
        AddPlayTime = 2,
        ServiceUsed = 3
    }

    public static class AppEvents
    {
        public static event Action<DataChangeType> DataChanged;

        public static void RaiseDataChanged(DataChangeType type)
        {
            try { DataChanged?.Invoke(type); } catch { }
        }

        // Backwards-compatible parameterless method (maps to Other)
        public static void RaiseDataChanged()
        {
            RaiseDataChanged(DataChangeType.Other);
        }
    }
}
