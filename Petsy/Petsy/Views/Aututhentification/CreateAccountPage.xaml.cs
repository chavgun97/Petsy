﻿using Petsy.Cache;
using Petsy.Interfaces;
using Petsy.Models;
using Petsy.Models.DTO;
using Petsy.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Petsy.Models.RequestJSON.RequestsJSON;

namespace Petsy.Views.Aututhentification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountPage : ContentPage
    {
        private bool EntryesIsValidate;
        private IClientRestApi RestClient;
        private ICache<User_x> currentUser;
        public CreateAccountPage()
        {
            currentUser = CurrentUser.GetInstance();
            InitializeComponent();
        }
   

        void OnTextEntryChanged(object sender, TextChangedEventArgs e)
        {
            ValidateLogic();
        }
        void OnCheckBoxChanged(object sender, EventArgs e)
        {
            ValidateLogic();
            if (CheckBoxPrivatePolicy.IsChecked)
            {
                CheckBoxPrivatePolicy.Color = Color.FromHex("#F0cdb1");
            }
            else
            {
                CheckBoxPrivatePolicy.Color = Color.FromHex("#BABABA");

            }
        }


        async void OnCreateAccBtn(object sender, EventArgs e)
        {
            if (EntryesIsValidate)
            {
                var name = NameEntry.Text.Trim();
                var email = EmailEntry.Text.Trim();
                var psw = PSWEntry.Text.Trim();

                var FdImpl = DependencyService.Get<IFireBaseAuth>(); //Factory
                ResultAuth result = await FdImpl.RegisteredWithEP(name, email, psw);
                if (result.isError)
                {
                    ErrorMsgLable.Text = result.ErrorMsg;
                }
                else
                {
                    
                    var token = result.Token;
                    var userName = result.Name;

                    currentUser.Get().Name = userName;
                    currentUser.Get().UID = result.UID;

                    RestClient = ClientRestApi.CreateNewInstance(token);
                    int i = RestClient.CreateUser(new UserRequest(currentUser.Get())).Result.ResultExecution;
                    if (i == 0)
                    {
                        currentUser.Update(RestClient.GetUserByToken().Result.UserExternal);                        
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                        ErrorMsgLable.Text = ("Ошибка создания нового пользователя на стороне сервера");
                }
            }

        }
        private void ValidateLogic()
        {
            var name = NameEntry.Text;
            var email = EmailEntry.Text;
            var psw = PSWEntry.Text;
            var checkBox = CheckBoxPrivatePolicy.IsChecked;

            if ((email != null && psw != null && name != null) && psw.Length > 5 && email.Contains('@') && checkBox)
            {
                CreateAccBtn.BackgroundColor = Color.FromHex("#F0cdb1");
                EntryesIsValidate = true;

            }
            else
            {
                CreateAccBtn.BackgroundColor = Color.FromHex("#BABABA");
                EntryesIsValidate = false;
            }
        }

    }
}