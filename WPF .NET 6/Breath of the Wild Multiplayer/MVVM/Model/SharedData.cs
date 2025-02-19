using Breath_of_the_Wild_Multiplayer.MVVM.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Breath_of_the_Wild_Multiplayer.MVVM.Model
{
    public static class SharedData
    {
        public static MainViewModel MainView { get; set; }
        public static ServerBrowserModel ServerBrowser { get; set; }
        public static SettingsPanelModel SettingsPanel { get; set; }
        public static ServerEditorModel ServerEditor { get; set; }
        public static ErrorMessageModel ErrorMessage { get; set; }
        public static LoadingModel LoadingMessage { get; set; }
        public static ModelSelectModel ModelSelect { get; set; }
        public static MainMenuModel MainMenu { get; set; }

        public static object TopView { get; set; }

        #region TryFunctions

        public static bool TryFunction(Action function)
        {
            try
            {
                function();
                return true;
            }
            catch (Exception ex)
            {
                LoadErrorMessage(ex.Message);
                return false;
            }
        }

        public static bool TryFunction<In>(Action<In> function, In parameters)
        {
            try
            {
                function(parameters);
                return true;
            }
            catch (Exception ex)
            {
                LoadErrorMessage(ex.Message);
                return false;
            }
        }

        public static bool TryFunction<Out>(Func<Out> function, out Out result)
        {
            try
            {
                Out temp = function();
                result = temp;
                return true;
            }
            catch (Exception ex)
            {
                LoadErrorMessage(ex.Message);
                result = default(Out);
                return false;
            }
        }

        public static bool TryFunction<In, Out>(Func<In, Out> function, In parameters, out Out result)
        {
            try
            {
                Out temp = function(parameters);
                result = temp;
                return true;
            }
            catch (Exception ex)
            {
                LoadErrorMessage(ex.Message);
                result = default(Out);
                return false;
            }
        }

        #endregion

        public static void SetLoadingMessage(string msg = "")
        {
            if(!string.IsNullOrEmpty(msg))
            {
                if (MainView.currentTopView == null || MainView.currentTopView.GetType() != typeof(LoadingModel))
                {
                    SharedData.MainView.updateTopView(SharedData.LoadingMessage);
                }
                
                LoadingMessage.UpdateMessage(msg);
            }
            else
            {
                if (MainView.currentTopView != null && MainView.currentTopView.GetType() == typeof(LoadingModel))
                {
                    SharedData.MainView.closeTopView();
                }
            }
        }

        public static void LoadErrorMessage(string msg)
        {
            var ErrorMessage = new ErrorMessageModel();
            ErrorMessage.Message = $"{msg}";
            SharedData.MainView.updateTopView(ErrorMessage);
        }
    }
}
