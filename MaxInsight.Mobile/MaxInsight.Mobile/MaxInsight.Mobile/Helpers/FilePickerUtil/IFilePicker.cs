using System;
using System.Threading.Tasks;

namespace Plugin.FilePicker.Abstractions
{
	/// <summary>
	/// Interface for FilePicker
	/// </summary>
	public interface IFilePicker
	{
		Task<FileData> PickFile();

        bool IsExistFile(string fileName, string viewLocation);


        bool SaveFile(byte[] fileData, string path);

        Task<bool> SaveFile(FileData fileToSave);
        void OpenFile(string fileToOpen,string viewLocation);
        void OpenFile(string fileToOpen);

        void OpenFile(FileData fileToOpen);
	}
}
