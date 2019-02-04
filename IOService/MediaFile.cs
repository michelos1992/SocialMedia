using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MediaFile
{
    public class MediaFile
    {
        public byte[] _fileBytes;
        public string _filePath;
        public OpenFileDialog _openFileDialog;

        public void AddFile() 
        {
            _openFileDialog = new OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|MP4 Files (*.mp4)|*.mp4" };
            var result = _openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    _filePath = _openFileDialog.FileName;
                    _fileBytes = File.ReadAllBytes(_filePath);
                    break;
                case DialogResult.Cancel:
                    break;
            }           
        }
    }
}