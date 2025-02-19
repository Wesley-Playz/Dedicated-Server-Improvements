using Breath_of_the_Wild_Multiplayer.MVVM.Model.DTO;
using Breath_of_the_Wild_Multiplayer.Source_files;
using System.Printing;

namespace Breath_of_the_Wild_Multiplayer.MVVM.Model
{
    public class CharacterModel : ObservableObject
    {
		public static int modelsLoaded = 0;

		private string _name;

		public string Name
		{
			get { return _name; }
			set { 
				_name = value;
				OnPropertyChanged();
			}
		}

		private string _description;

		public string Description
		{
			get { return _description; }
			set { 
				_description = value;
				OnPropertyChanged();
			}
		}

		private bool _isArmorSync;

		public bool IsArmorSync
		{
			get { return _isArmorSync; }
			set { _isArmorSync = value; }
		}

		private string _model;

		public string Model
		{
			get { return _model; }
			set {
                _model = value;
                OnPropertyChanged();
            }
		}

		private BumiiDTO _bumiiData;

		public BumiiDTO BumiiData
		{
			get { return _bumiiData; }
			set { _bumiiData = value; }
		}

		private string _alternativeModel;

		public string AlternativeModel
		{
			get { return _alternativeModel; }
			set { _alternativeModel = value; }
		}

		public string BustPic
		{
			get
			{
				string imageName = string.IsNullOrEmpty(_alternativeModel) ? Name : _alternativeModel;

				if(Model.StartsWith("Npc_"))
					imageName = Model.Substring(4);

				return $"/Images/Bust/{imageName}.png";
			}
		}

		public string BodyPic
		{
			get
			{
                string imageName = string.IsNullOrEmpty(_alternativeModel) ? Name : _alternativeModel;

                if (Model.StartsWith("Npc_"))
                    imageName = Model.Substring(4);

                return $"/Images/Body/{imageName}.png";
            }
		}

		private int _modelIndex;

		public int ModelIndex
		{
			get { return _modelIndex; }
			set { _modelIndex = value; }
		}

		private bool _hasCustomAction;

		public bool HasCustomAction
		{
			get { return _hasCustomAction; }
			set { _hasCustomAction = value; }
		}

		private bool _selected;

		public bool Selected
		{
			get { return _selected; }
			set { 
				_selected = value;
				OnPropertyChanged();
			}
		}

        public enum ModelType : byte
        {
            ArmorSync = 0,
            CustomModel = 1,
            Mii = 2
        }

		private ModelType _type;

		public ModelType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string UmiiPath;

		public RelayCommand CustomAction { get; set; }

		public CharacterModel(string name, string model, bool isArmorSync, string description = "", RelayCommand? customAction = null, string alternativeModel = "")
        {
			this._modelIndex = modelsLoaded;
			this._name = name;
			this._model = model;
			this._isArmorSync = isArmorSync;
			this.AlternativeModel = alternativeModel;

			if(this.IsArmorSync)
			{
				this._type = ModelType.ArmorSync;
			}
			else
			{
				this._type = ModelType.CustomModel;
			}

			if (description == "")
				this._description = "This model does " + (IsArmorSync ? "" : "not ") + "allow for armor sync.";
			else
				this._description = description;

			if(customAction != null)
			{
				this._hasCustomAction = true;
				this.CustomAction = customAction;
			}
			else
			{
				this._hasCustomAction = false;
			}

			modelsLoaded++;
        }
    }
}
