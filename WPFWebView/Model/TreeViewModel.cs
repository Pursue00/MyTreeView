using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFWebView.Common;

namespace WPFWebView.Model
{
    public class TreeViewModel : BindingModelBase
    {
        #region Property
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(() => Id); }
        }

        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; OnPropertyChanged(() => ParentId); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(() => Name); }
        }

        private double fontsize;

        public double FontSize
        {
            get { return fontsize; }
            set { fontsize = value; OnPropertyChanged(() => FontSize); }
        }

        private ImageSource _CameraSource;

        public ImageSource CameraSource
        {
            get { return _CameraSource; }
            set { _CameraSource = value; OnPropertyChanged(() => CameraSource); }
        }

        private string _CameraImgUrl;
        public string CameraImgUrl
        {
            get { return _CameraImgUrl; }
            set
            {
                if (value == null)
                    return;
                _CameraImgUrl = value;
                var bitmap = BitmapEncoderHelper.PathToBitmapThumbImage(_CameraImgUrl);
                if (bitmap != null)
                    CameraSource = bitmap;
            }
        }

        private ObservableCollection<TreeViewModel> _Children;
        /// <summary>
        /// 下级内容
        /// </summary>
        public ObservableCollection<TreeViewModel> Children
        {
            get
            {
                if (_Children == null)
                    _Children = new ObservableCollection<TreeViewModel>();
                return _Children;
            }
            set
            {
                this._Children = value;
                OnPropertyChanged(() => Children);
            }
        }
        #endregion

        public TreeViewModel(int id,string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
