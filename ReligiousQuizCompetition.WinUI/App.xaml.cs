using Microsoft.Extensions.DependencyInjection;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetitionWinUI.Services;
using ReligiousQuizCompetitionWinUI.ViewModels;
using ReligiousQuizCompetitionWinUI.Views;
using System.Net.Http;

namespace ReligiousQuizCompetitionWinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public Window? Window { get; private set; }

        public static IServiceProvider Services { get; set; }
        public static Frame RootFrame { get; set; }
        public App()
        {
            InitializeComponent();

            var services = new ServiceCollection();

            ConfigureServices(services);

            Services = services.BuildServiceProvider();
        }

        void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<QuizService>();
            services.AddTransient<QuizViewModel>();

            services.AddTransient<HomeViewModel>();

            services.AddSingleton<AuthService>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<SignupViewModel>();

            services.AddSingleton<ISecureStorage, SecureStorage>();
            services.AddSingleton<ITokenService, TokenService>();

            services.AddTransient<BaseViewModel>();
            
            services.AddTransient<RanksViewModel>();
            services.AddSingleton<RanksService>();

            services.AddSingleton<ApiClient>();
            services.AddSingleton(new HttpClient(
                new HttpClientHandler
                {
                    UseProxy = false
                }));

            services.AddSingleton<NavigationService>();
            services.AddSingleton<MainWindow>();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Window = Services.GetRequiredService<MainWindow>();
            Window.Activate();
        }
    }
}
