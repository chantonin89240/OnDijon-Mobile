using OnDijon.CG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OnDijon.UnitTest.CG.Services.Mocks
{
    class NavigationMockService : NavigationService
    {
        public INavigation Navigation { get; }

        public Page CurrentPage => Navigation.NavigationStack.LastOrDefault();

        public NavigationMockService()
        {
            Navigation = new NavigationMockProxy();
            Initialize(Navigation);
        }
    }

    class NavigationMockProxy : INavigation
    {
        public IReadOnlyList<Page> ModalStack => throw new NotImplementedException();

        private readonly Stack<Page> _navigationStack = new Stack<Page>();
        public IReadOnlyList<Page> NavigationStack
        {
            get
            {
                var list = _navigationStack.ToList();
                list.Reverse();
                return list;
            }
        }

        public void InsertPageBefore(Page page, Page before)
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopAsync()
        {
            return Task.Run(() => { return _navigationStack.Pop(); });
        }

        public Task<Page> PopAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopModalAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Page> PopModalAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PopToRootAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopToRootAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PushAsync(Page page)
        {
            return Task.Run(() => { _navigationStack.Push(page); });
        }

        public Task PushAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(Page page)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public void RemovePage(Page page)
        {
            throw new NotImplementedException();
        }
    }

    class MockPage : Page
    {

    }

    class MockPageWithParam<T> : Page
    {
        public T Parameter { get; }

        public MockPageWithParam(T parameter)
        {
            Parameter = parameter;
        }
    }
}
