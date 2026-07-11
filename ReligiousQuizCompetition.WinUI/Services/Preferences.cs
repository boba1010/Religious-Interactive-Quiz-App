using Windows.Storage;

namespace ReligiousQuizCompetitionWinUI.Services
{
    public static class Preferences<T>
    {
        private static readonly ApplicationDataContainer _settings =
            ApplicationData.Current.LocalSettings;

        public static bool ContainsKey(string key)
        {
            return _settings.Values.ContainsKey(key);
        }

        public static void Clear()
        {
            _settings.Values.Clear();
        }

        public static T? Get(string key, T? defaultValue = default)
        {
            return _settings.Values.TryGetValue(key, out var value)
                ? (T)value
                : defaultValue;
        }

        public static void Remove(string key)
        {
            _settings.Values.Remove(key);
        }

        public static void Set(string key, T value)
        {
            _settings.Values[key] = value;
        }
    }
}
