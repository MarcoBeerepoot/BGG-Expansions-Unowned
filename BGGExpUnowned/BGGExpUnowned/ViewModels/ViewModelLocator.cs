using System;
using System.Collections.Generic;
using System.Text;
using com.mbpro.BGGExpUnowned.API;
using GalaSoft.MvvmLight.Ioc;

namespace com.mbpro.BGGExpUnowned.ViewModels
{
    class ViewModelLocator
    {

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new MainViewModel(new XMLapi2()));
        }

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
