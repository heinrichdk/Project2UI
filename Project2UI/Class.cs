using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Project2UI
{
    public class BrowserStorageService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public BrowserStorageService(ProtectedLocalStorage protectedLocalStorage)//, AuthenticationStateProvider authenticationStateProvider
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _protectedLocalStorage.GetAsync<T>(key);

            if (json.Success == false)
                return default;

            return json.Value;
        }

        public async Task SetItem<T>(string key, T value)
        {
            await _protectedLocalStorage.SetAsync(key, value);
        }

        public async Task RemoveItem(string key)
        {
            await _protectedLocalStorage.DeleteAsync(key);
        }



    }
}
