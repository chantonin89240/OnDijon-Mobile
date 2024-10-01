using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Essentials;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace OnDijon.Common.ViewModels
{
    public abstract class BaseViewModel : BindableObjectBase, IInitialize, INavigatedAware
    {
	    #region private and protected members
	    protected readonly ILoggerService Logger;
	    protected readonly INavigationService NavigationService;
	    protected readonly ITranslationService TranslationService;
	    public readonly IPopupService PopupService;
        #endregion
        public ICommand NavigateBack { get; }
        public ICommand CleanUpAndQuit { get; }

        #region Properties

        bool _isLoading;
        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private static bool _displayErrorPopup = true;

        public bool IsOffline => Connectivity.NetworkAccess != NetworkAccess.Internet;

        #endregion


        #region Ctor

        protected BaseViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, ILoggerService loggerService)
        {
	        Logger = loggerService;
	        NavigationService = navigationService;
            TranslationService = translationService;
            PopupService = popupService;

            NavigateBack = new AsyncCommand(NavigationService.GoBackAsync);
            
            // TODO : REFACTO should be removed as the same as NavigateBack ??? 
            CleanUpAndQuit = new AsyncCommand(NavigationService.GoBackAsync);
        }

        #endregion
        
        #region Life Cycle
        public void Initialize(INavigationParameters parameters)
        {
	        // Vous pouvez potentiellement utiliser cette fonction qui sera appelée à chaque fois que vous initialiserez  cette page (donc à la création)
        }

        /// <summary>
        /// OnNavigatedTo est appellé à chaque fois que le page est affichér (donc à la création et à chaque fois que vous revenez dessus)
        /// </summary>
        /// <param name="parameters">parametres de navigation</param>
        public void OnNavigatedTo(INavigationParameters parameters)
        {
	        App.CurrentNavigationService = NavigationService;
	        Logger.Info($" Navigated to {GetType().Name}");
	        OnNavigatedToAsync(parameters)
		        .SafeFireAndForget(exception => Logger.Error(info:$"{this.GetType().Name} error in OnNavigatedToAsync", ex: exception));
        }

        public virtual Task OnNavigatedToAsyncNew(INavigationParameters parameters) { return Task.CompletedTask; }
        public virtual Task OnNavigatedToAsyncBack(INavigationParameters parameters) { return Task.CompletedTask; }

        /// <summary>
        /// OnNavigatedToAsync est appellé (par OnNavigatedTo) à chaque fois que le page est affichée (donc à la création et à chaque fois que vous revenez dessus)
        /// C'est cette fonction qu'il faut overrider dans les ViewModels où vous auriez des ressources à charger à son affichage ou à recharger quand vous revenerz sur la page
        /// Bous pouvez détecter le fait que ce soit un nouvel affichage ou un retour en arrière en appelant l'extension GetNavigationMode() sur l'objet parameters
        /// </summary>
        /// <param name="parameters">parametres de navigation</param>
        public virtual Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            // exemple de gestion de la navigation
            NavigationMode navigationMode = parameters.GetNavigationMode();

            if (navigationMode == NavigationMode.New)
            {
                return OnNavigatedToAsyncNew(parameters);
            }
            else if (navigationMode == NavigationMode.Back)
            {
                return OnNavigatedToAsyncBack(parameters);
            }

            return Task.CompletedTask;

            //switch (navigationMode)
            //{
            //    case NavigationMode.New:
            //        // c'est un nouvel affichage
            //        return OnNavigatedToAsyncNew(parameters);
            //    case NavigationMode.Back:
            //        return OnNavigatedToAsyncBack(parameters);
            //    default:
	           //     return Task.CompletedTask;
            //}
        }

        /// <summary>
        /// OnNavigatedFrom est appellé  à chaque fois que le page est "cachée" (donc à chaque fois que vous quittez la page)
        /// </summary>
        /// <param name="parameters">parametres de navigation</param>
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
	        Logger.Info($"Navigated from {GetType().Name}");
	        OnNavigatedFromAsync(parameters)
		        .SafeFireAndForget(exception => Logger.Error(info:$"{this.GetType().Name} error in OnNavigatedFromAsync", ex: exception));
        }

        /// <summary>
        /// OnNavigatedFromAsync est appellé (par OnNavigatedFrom) à chaque fois que le page est "cachée" (donc à chaque fois que vous quittez la page)
        /// C'est cette fonction qu'il faut overrider dans les ViewModels ou vous auriez des ressources à libérer quand ils ne sont pas actif (ex: des timers, des websockets, des listeners, etc...)
        /// </summary>
        /// <param name="parameters">parametres de navigation</param>
        public virtual Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
	        return Task.CompletedTask;
        }

        public virtual void Cleanup() { }
        #endregion
        
        /// <summary>
        /// Calls the API.
        /// </summary>
        /// <param name="apiCall">API call.</param>
        protected async void CallApi(Func<Task> apiCall)
        {
            IsLoading = true;
            try
            {
                await apiCall.Invoke();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }


            /// <summary>
            /// Manages the API responses.
            /// </summary>
            /// <param name="result">Result.</param>
            /// <param name="callbackManager">Callback manager.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            protected void ManageApiResponses<T>(T result, CallbackManager<T> callbackManager) where T : Response
            {
                IsLoading = false;

                Action<T> method;

                switch (result.State)
                {
                    case CallStatusEnum.Success:
                        method = callbackManager.OnSuccess;
                        break;
                    case CallStatusEnum.InternalServerError:
                        method = callbackManager.OnError ?? OnInternalServerError;
                        break;
                    case CallStatusEnum.ServiceUnavailable:
                        method = OnServiceUnavailableError;
                        break;
                    case CallStatusEnum.InvalidInformations:
                        method = callbackManager.OnInvalidInformations;
                        break;
                    case CallStatusEnum.InvalidCredentials:
                        method = callbackManager.OnInvalidCredentials;
                        break;
                    case CallStatusEnum.NotFound:
                        method = callbackManager.OnNotFound;
                        break;
                    case CallStatusEnum.InvalidPassword:
                        method = OnInvalidPasswordError;
                        break;
                    case CallStatusEnum.InvalidMail:
                        method = OnInvalidMailError;
                        break;
                    case CallStatusEnum.AuthError:
                        method = callbackManager.AuthError;
                        break;
                    case CallStatusEnum.ProfileNotFound:
                        method = callbackManager.ProfileNotFound;
                        break;
                    default:
                        method = OnUnknownError;
                        break;
                }

                method = method ?? OnUnknownError;
                method.Invoke(result);
            }

            #region Base errors

            void OnUnknownError(Response obj)
            {
                Trace(obj, "Un problème est survenu", null, false);
            }

            void OnInternalServerError(Response obj)
            {
                Trace(obj, "Un problème de connexion avec le serveur est survenu", null, false);
            }

            void OnServiceUnavailableError(Response obj)
            {
                Trace(obj, "La connexion avec le serveur semble impossible, verifiez votre connexion internet", null, true, "Oups !");
            }

            void OnInvalidPasswordError(Response obj)
            {
                Trace(obj, "Le mot de passe choisi est trop court, il doit contenir au moins 8 caractères dont au moins une majuscule, une minuscule et un chiffre", null, true, "Action impossible");
            }

            void OnInvalidMailError(Response obj)
            {
                Trace(obj, "Il existe déjà un compte avec cette adresse mail", null, true, "Action impossible");
            }

            public void Trace(Response obj, string message, Exception exception = null, bool displayPopUp = true, string popupTitle = "Action impossible")
            {
	            
                StackTrace stackTrace = new StackTrace();
                string log = obj != null ? obj.Message : "";
                ShowError(message, popupTitle, exception, displayPopUp, string.Format("{0}\n{1}", log, stackTrace.ToString()));
            }

            public void ShowError(string message, Exception exception = null, bool displayPopUp = true, string stackTrace = null)
            {
	            Logger.Error(info: "Error : " + message, ex: exception);
                ShowError(message, "Action impossible", exception, displayPopUp, stackTrace);
            }

            /// <summary>
            /// Show a popup with an error message
            /// </summary>
            /// <param name="message">Error message</param>
            /// <param name="exception">Exception to log</param>
            public void ShowError(string message, string popupTitle, Exception exception = null, bool displayPopUp = true, string stackTrace = null)
            {
                if (exception != null)
                {
                    LogException(exception, message);
                }
                else
                {
                    Debug.Fail(message);
                    Crashes.TrackError(new Exception("Exception non gérée : " + message + "\n" + stackTrace));
                }

                if (displayPopUp)
                {
                    if (_displayErrorPopup)
                    {
                        PopupService.Show(PopupEnum.PopupError, popupTitle, message, "Retour", () => _displayErrorPopup = true);
                        _displayErrorPopup = false;
                    }
                }
            }

            /// <summary>
            /// Log an exception in the console and in App Center
            /// </summary>
            /// <param name="exception"></param>
            /// <param name="message"></param>
            public void LogException(Exception exception, string message = null)
            {
                message = message ?? exception.Message;
                Debug.Fail(message, exception.ToString());
                Crashes.TrackError(exception);
            }

        #endregion

        #region Navigation 
        public virtual Task<INavigationResult> NavigateTo(string pageKey, bool popIfPageKeyExists = false, INavigationParameters parameters = null)
        {
            return NavigationService.NavigateTo(pageKey, popIfPageKeyExists, parameters);
        }
        
        public const string GenericNavigationParameterKey = nameof(GenericNavigationParameterKey);
        public static NavigationParameters NavigationParametersFactory(object parameter, string key = GenericNavigationParameterKey)
        {
	        var parameters = new NavigationParameters();
	        parameters.Add(key, parameter);
	        return parameters;
        }
		#endregion 
        public string GetString(string key)
        {
            return TranslationService.GetString(key);
        }

        public void StartNetworkMonitoring()
        {
            RaisePropertyChanged(nameof(IsOffline));
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public void StopNetworkMonitoring()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsOffline));
        }

        
        
    }
}
