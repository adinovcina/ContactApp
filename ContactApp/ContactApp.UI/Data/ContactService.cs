using ContactApp.Models;
using ContactApp.Models.ApiModels;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ContactApp.UI.Data
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;
        private readonly NavigationManager _navigationManager;

        public ContactService(HttpClient httpClient, TokenProvider tokenProvider, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _navigationManager = navigationManager;
            SetAuthorizationHeader();
        }

        public async Task CreateContact(Contact model)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/v1/contacts/create", model);

            if (result.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("contacts");
            }
        }

        public async Task UpdateContact(Contact model)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/v1/contacts/edit", model);

            if (result.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("contacts");
            }
        }

        public async Task DeleteContact(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/v1/contacts/delete/{id}");

            if (result.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("contacts");
            }
        }

        public async Task<IEnumerable<Contact>> GetFavourites()
        {
            var result = await _httpClient.GetAsync($"api/v1/contacts/favourites");

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadFromJsonAsync<ContactApiResponse>();
                return apiResponse.Data;
            }

            return Enumerable.Empty<Contact>();
        }

        public async Task<IEnumerable<Contact>> GetAllContacts(string? searchPhrase)
        {
            var result = new HttpResponseMessage();

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                result = await _httpClient.GetAsync($"api/v1/contacts?searchPhrase={searchPhrase}");
            }
            else
            {
                result = await _httpClient.GetAsync($"api/v1/contacts");
            }

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadFromJsonAsync<ContactApiResponse>();
                return apiResponse.Data;
            }

            return Enumerable.Empty<Contact>();
        }

        public async Task<Contact> GetContactDetails(Guid id)
        {
            var result = await _httpClient.GetAsync($"api/v1/contacts/{id}");

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadFromJsonAsync<ContactApiResponse>();
                return apiResponse.Data.FirstOrDefault();
            }

            return new Contact();
        }


        private void SetAuthorizationHeader()
        {
            var accessToken = _tokenProvider.AccessToken;

            if (accessToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
