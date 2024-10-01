using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace OnDijon.Common.Services
{
    public class NavigationService 
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private INavigation _navigation;

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    var currentPage = _navigation.NavigationStack.LastOrDefault();
                    if (currentPage == null)
                    {
                        return null;
                    }

                    var pageType = currentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public void GoBack()
        {
            _navigation.PopAsync(true);
        }


        public void NavigateTo(string pageKey, bool popIfPageKeyExists = true)
        {
            // on récupère la stack de navigation (en récupérant les pages key plutot que les pages)
            IList<string> navigationPageKey = GetListNavigationPageKey();

            // est-ce la page à pusher est déjà dans la stack de navigation ?
            if (!navigationPageKey.Contains(pageKey))
            {
                // la page n'est pas présente donc on push notre page
                NavigateTo(pageKey);
            }
            else
            {
                // la page est déjà présente donc on pop jusqu'à cette page
                GoBackToPageKey(pageKey);
            }
        }

        public async void GoBackToPageKey(string pageKey)
        {

            // on récupère la stack de navigation (en récupérant les pages key plutot que les pages)
            IList<string> navigationPageKey = GetListNavigationPageKey();


            string currentPageKey;
            int backCount = 0;
            // On parcourt la stack des pages pour compter le nombre de fois qu'il faut poper
            for (int i = navigationPageKey.Count - 1; i > 0; i--)
            {
                currentPageKey = navigationPageKey.LastOrDefault();
                // Tant que la currentPageKey n'est pas la page souhaitée,
                // on incrémente le compteur de back
                if (!string.Equals(currentPageKey, pageKey, StringComparison.OrdinalIgnoreCase))
                {
                    navigationPageKey.RemoveAt(i);
                    backCount++;
                }
            }

            // On supprime les pages intermédiaires (on garde la derniere volontairement) puis on pop la derniere avec PopAsync()
            // cela permet de ne pas voir s'afficher chaque page pendant les pop
            for (var counter = 1; counter < backCount; counter++)
            {
                _navigation.RemovePage(_navigation.NavigationStack[_navigation.NavigationStack.Count - 2]);
            }
            if (pageKey != Locator.DashboardView || CurrentPageKey != Locator.DashboardView)
            {
                await _navigation.PopAsync();
            }


        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                        parameter
                    };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    _navigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }




        #region private methods


        private IList<string> GetListNavigationPageKey()
        {
            List<string> navigationPageKey = new List<string>();
            // on récupère la stack de navigation (en récupérant les pages key plutot que les pages)
            foreach (var item in _navigation.NavigationStack)
            {
                var pageType = item.GetType();

                if (_pagesByKey.ContainsValue(pageType))
                {
                    navigationPageKey.Add(_pagesByKey.First(p => p.Value == pageType).Key);
                }
            }
            return navigationPageKey;
        }

        #endregion
    }
}