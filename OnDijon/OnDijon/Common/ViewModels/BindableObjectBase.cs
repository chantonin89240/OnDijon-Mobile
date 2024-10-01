using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace OnDijon.Common.ViewModels
{
	public class BindableObjectBase : BindableBase
	{
		protected virtual bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			return SetProperty<T>(ref storage, value, propertyName);
		}
	}
}