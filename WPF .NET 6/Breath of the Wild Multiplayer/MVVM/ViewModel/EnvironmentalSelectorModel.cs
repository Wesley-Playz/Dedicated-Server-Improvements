using Breath_of_the_Wild_Multiplayer.MVVM.Model;
using Breath_of_the_Wild_Multiplayer.MVVM.Model.DTO;
using Breath_of_the_Wild_Multiplayer.Source_files;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Breath_of_the_Wild_Multiplayer.MVVM.ViewModel
{
    public class EnvironmentalSelectorModel : ObservableObject
    {
		private string _searchTerm;

		public string SearchTerm
		{
			get { 
				return _searchTerm; 
			}
			set { 
				_searchTerm = value;
                this.IsLoading = true;
                OnPropertyChanged();
				OnPropertyChanged("Characters");
            }
		}

		private ObservableCollection<CharacterModel> _characters;

		public ObservableCollection<CharacterModel> Characters
		{
			get {
				ObservableCollection<CharacterModel> result = new ObservableCollection<CharacterModel>(_characters.Where(character => character.Description.ToLower().Contains(_searchTerm.ToLower())));
				this.IsLoading = false;
                return result; 
			}
			set { 
				_characters = value;
				OnPropertyChanged();
			}
		}

		private bool _isLoading;

		public bool IsLoading
		{
			get { return _isLoading; }
			set { 
				_isLoading = value;
				OnPropertyChanged();
			}
		}

        private CharacterModel _selectedModel;

        public CharacterModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                OnPropertyChanged();
            }
        }

        private CharacterModel _lastSelectedModel;

        public CharacterModel LastSelectedModel
        {
            get { return _lastSelectedModel; }
            set
            {
                _lastSelectedModel = value;
                OnPropertyChanged();
            }
        }

        private bool _isMaxOffset;

        public bool isMaxOffset
        {
            get { return _isMaxOffset; }
            set
            {
                _isMaxOffset = value;
                OnPropertyChanged();
            }
        }

        private int _scrollState;

        public int scrollState
        {
            get { return _scrollState; }
            set
            {
                _scrollState = value;
                OnPropertyChanged();
            }
        }

        private bool _confirmEnabled;

        public bool ConfirmEnabled
        {
            get { return _confirmEnabled; }
            set { 
                _confirmEnabled = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand modelButtonClick { get; set; }
		public RelayCommand confirmClick { get; set; }
		public RelayCommand cancelClick { get; set; }

		public EnvironmentalSelectorModel()
		{
			this._searchTerm = "";
			this._characters = new ObservableCollection<CharacterModel>();

            this.isMaxOffset = true;
            this.scrollState = 0;

            this.ConfirmEnabled = false;

            List<EnvironmentalModelDTO> models;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Breath_of_the_Wild_Multiplayer.Source_files.EnvironmentalModels.txt"))
			{
				TextReader tr = new StreamReader(stream);
				string fileContents = tr.ReadToEnd();

				models = JsonConvert.DeserializeObject<EnvironmentalModelDTO[]>(fileContents).ToList();
			}

			CharacterModel.modelsLoaded = 0;

			foreach (EnvironmentalModelDTO model in models)
			{
				foreach (string unit in model.Units)
				{
					this._characters.Add(new CharacterModel(unit, model.Folder, false, model.Folder));
				}
			}

            modelButtonClick = new RelayCommand(o => this.ChangeSelectedModel(_characters[(int)o]));

			cancelClick = new RelayCommand(o => {
                SharedData.ModelSelect.ChangeSelectedModel(SharedData.ModelSelect.LastSelectedModel);
                this.CloseWindow();
                });

            confirmClick = new RelayCommand(o => {
                SharedData.ModelSelect.SetEnvironmental(this.SelectedModel);
                this.CloseWindow();
            });
        }

        private void ChangeSelectedModel(CharacterModel newSelection)
        {
            this.LastSelectedModel = SelectedModel;
            SelectedModel = newSelection;

			if(this.LastSelectedModel != null)
			{
				this.LastSelectedModel.Selected = false;
			}
            newSelection.Selected = true;

            if (SelectedModel.HasCustomAction)
                SelectedModel.CustomAction.Execute(null);

            this.ConfirmEnabled = true;
        }

        public void CloseWindow()
        {
            SharedData.MainView.closeTopView();
        }
    }
}
