using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WPFWebView.Model
{
    public class BindingModelBase : INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(((MemberExpression)expression.Body).Member.Name));
            }
        }
    }
}
