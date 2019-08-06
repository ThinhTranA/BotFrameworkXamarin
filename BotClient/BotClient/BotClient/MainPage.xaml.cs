using BotClient.Bot;
using BotClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BotClient
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private BotConnector _botConnector;
        public ObservableCollection<BotMessage> ConversationList { get; }
        private bool _progressVisible;
        public bool ProgressVisible
        {
            get => _progressVisible;
            set
            {
                _progressVisible = value;
                OnPropertyChanged("ProgressVisible");
            }
        }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            ConversationList = new ObservableCollection<BotMessage>();
            _botConnector = new BotConnector();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _botConnector.Setup();
        }

        private async void  SendButton_Clicked(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            ProgressVisible = true;
            var botResponse = await _botConnector.SendMessage("Daniel", Message.Text);
            ProgressVisible = false;
            ConversationList.Add(botResponse);
        }

      
    }
}
