using System;
using System.Collections.Generic;
using System.Text;

namespace MyBodyTemperature.Controls
{
    public interface ISearchPage
    {
        void OnSearchBarTextChanged(in string text);
        event EventHandler<string> SearchBarTextChanged;
    }
}
