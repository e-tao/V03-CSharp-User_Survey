global using System.Collections.Generic;
global using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using WebUsers;


namespace Survey
{
    public partial class MainWindow : Window
    {
        private List<string> questions = new();
        private ObservableCollection<User> users;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task InitDB()
        {
            SurveyModel.DBName = "V03_Survey_Cheng";
            await SurveyModel.CreateDatabase();
            if (await SurveyModel.GetNumberQuestion()==0)
            {
                questions.Add("It is easy to navigate through the app.");
                questions.Add("It is easy to create a new user account.");
                questions.Add("The booking process is smooth and easy.");
                questions.Add("I will use this service again.");
                questions.Add("I will recommand FireRnR to my family and friends.");
                await SurveyModel.AddQuestions(questions);
            }
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitDB();

            users = await RandomUsers.GetUsers(100);
            //randomusers.ItemsSource = users;
        }
    }
}
