using Features.GameResources.Relics;

namespace Features.Common
{
    public static class Provider
    {
        private static PlayerPrefsRelicsRepository _playerPrefsRelicsRepository;

        public static PlayerPrefsRelicsRepository PlayerPrefsRelicsRepository() =>
            _playerPrefsRelicsRepository ??= new PlayerPrefsRelicsRepository();

    }
}