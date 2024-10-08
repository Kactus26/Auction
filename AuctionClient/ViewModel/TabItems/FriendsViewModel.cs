﻿using AuctionClient.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Input;
using CommonDTO;
using System.Collections.ObjectModel;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class FriendsViewModel : ObservableObject
    {
        #region ObservableProperties
        [ObservableProperty]
        public ObservableCollection<UserDataWithImageDTO> friends1 = new ObservableCollection<UserDataWithImageDTO>();
        [ObservableProperty]
        public ObservableCollection<UserDataWithImageDTO> friends2 = new ObservableCollection<UserDataWithImageDTO>();
        [ObservableProperty]
        public string? name;
        [ObservableProperty]
        public string? surname;
        [ObservableProperty]
        public string currentPage = "1";
        #endregion

        private const int pageSize = 6;
        private TypeOfUserFriendsOnPage typeOfUsers = TypeOfUserFriendsOnPage.Friends;//After trying to change page, OnCurPageChanged method executes, that's why this variable exists 
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public enum TypeOfUserFriendsOnPage
        {
            Friends,
            Search,
            Invitations
        }

        partial void OnCurrentPageChanged(string value)
        {
            GetUserFriends();
        }

        public FriendsViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                GetUserFriends();
            }
            else
                Name = "Guest can't have any friends(";

        }

        [RelayCommand]
        public async Task UserFriendsButton()
        {
            CurrentPage = "1";
            typeOfUsers = TypeOfUserFriendsOnPage.Friends;
            await GetUserFriends();
        }

        [RelayCommand]
        public async Task UserInvitationsButton()
        {
            CurrentPage = "1";
            typeOfUsers = TypeOfUserFriendsOnPage.Invitations;
            await UserInvitations();
        }

        private async Task UserInvitations()
        {
            PaginationDTO paginationDTO = new PaginationDTO() { CurrentPage = int.Parse(CurrentPage), PageSize = pageSize };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/FindUserInvitations", paginationDTO);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<UserDataWithImageDTO>? all = JsonConvert.DeserializeObject<List<UserDataWithImageDTO>>(responseContent);
                UsersAllocation(all);
                typeOfUsers = TypeOfUserFriendsOnPage.Invitations;
            }
            else
                MessageBox.Show($"{responseContent}");

            return;

        }

        [RelayCommand]
        public async Task SearchButton()
        {
            CurrentPage = "1";
            await Search();
        }

        private async Task GetUserFriends()
        {
            switch (typeOfUsers)
            {
                case TypeOfUserFriendsOnPage.Search:
                    await Search();
                    return;
                case TypeOfUserFriendsOnPage.Invitations:
                    await UserInvitations();
                    return;
            }
                
                
            if (CurrentPage.Any(d => !char.IsDigit(d)))
                return;

            PaginationDTO paginationDTO = new PaginationDTO() { CurrentPage = System.Convert.ToInt32(CurrentPage), PageSize = pageSize};
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetUserFriends", paginationDTO);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<UserDataWithImageDTO>? all = JsonConvert.DeserializeObject<List<UserDataWithImageDTO>>(responseContent);
                UsersAllocation(all);
                typeOfUsers = TypeOfUserFriendsOnPage.Friends;
            }
            else
                MessageBox.Show($"{responseContent}");
        }

        private void UsersAllocation(List<UserDataWithImageDTO> all)
        {
            Friends1.Clear();
            Friends2.Clear();

            int index = 0;
            while (index < all.Count)
            {
                if (index <= 2)
                    Friends1.Add(all[index]);
                else
                    Friends2.Add(all[index]);
                index++;
            }
        }

        [RelayCommand]
        public void NextPage()
        {
            if (CurrentPage.Any(d => !char.IsDigit(d)))
                return;

            int curPage = int.Parse(CurrentPage);
            curPage++;
            CurrentPage = curPage.ToString();
        }

        [RelayCommand]
        public void PreviousPage()
        {
            if (CurrentPage.Any(d => !char.IsDigit(d)))
                return;

            int curPage = int.Parse(CurrentPage);
            if (curPage <= 1)
                return;

            curPage--;
            CurrentPage = curPage.ToString();
        }

        private async Task Search()
        {
            PaginationUserSearchDTO userSearchDTO = new PaginationUserSearchDTO() { Name = this.Name, Surname = this.Surname, CurrentPage = int.Parse(CurrentPage), PageSize = pageSize };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/FindUser", userSearchDTO);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<UserDataWithImageDTO>? all = JsonConvert.DeserializeObject<List<UserDataWithImageDTO>>(responseContent);
                UsersAllocation(all);
                typeOfUsers = TypeOfUserFriendsOnPage.Search;
            }
            else
                MessageBox.Show($"{responseContent}");
        }

    }
}
