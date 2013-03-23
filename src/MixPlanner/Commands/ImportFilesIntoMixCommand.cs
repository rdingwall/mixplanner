using System;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoMixCommand : AsyncCommandBase<DropInfo>
    {
        readonly ITrackLibrary library;
        readonly IMix mix;

        public ImportFilesIntoMixCommand(ITrackLibrary library, IMix mix)
        {
            if (library == null) throw new ArgumentNullException("library");
            if (mix == null) throw new ArgumentNullException("mix");
            this.library = library;
            this.mix = mix;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null) return false;
            if (mix.IsLocked) return false;
            var data = (IDataObject)parameter.Data;
            return data.GetDataPresent(DataFormats.FileDrop); 
        }

        protected override async Task DoExecute(DropInfo parameter)
        {
            var data = (IDataObject)parameter.Data;

            var filenames = (string[])data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            var tracks = await library.ImportAsync(filenames);

            mix.Insert(tracks, parameter.InsertIndex);
        }
    }
}